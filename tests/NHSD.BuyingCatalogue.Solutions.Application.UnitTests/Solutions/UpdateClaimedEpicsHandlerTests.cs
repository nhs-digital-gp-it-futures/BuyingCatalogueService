using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateClaimedEpics;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Epics;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class UpdateClaimedEpicsHandlerTests
    {
        private const string ValidSolutionId = "Sln1";
        private const string InvalidSolutionId = "Sln123";

        private TestContext context;

        [SetUp]
        public void Setup()
        {
            context = new TestContext();
            context.MockSolutionRepository
                .Setup(r => r.CheckExists(ValidSolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            context.MockSolutionRepository
                .Setup(r => r.CheckExists(InvalidSolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
        }

        [Test]
        public async Task ShouldUpdateEpicsAsync()
        {
            const int expectedEpicsCount = 1;

            var claimedEpics = new HashSet<IClaimedEpic>
            {
                Mock.Of<IClaimedEpic>(e => e.EpicId == "Epic1" && e.StatusName == "Passed"),
            };

            context.MockEpicRepository
                .Setup(e => e.CountMatchingEpicIdsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEpicsCount);

            context.MockSolutionEpicStatusRepository
                .Setup(e => e.CountMatchingEpicStatusAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEpicsCount);

            var validationResult = await UpdateClaimedEpicsAsync(ValidSolutionId, claimedEpics);
            validationResult.IsValid.Should().BeTrue();

            context.MockSolutionRepository.Verify(s => s.CheckExists(ValidSolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IEnumerable<string>, bool>> epicIdMatch = s =>
                s.SequenceEqual(claimedEpics.Select(e => e.EpicId));

            context.MockEpicRepository.Verify(
                e => e.CountMatchingEpicIdsAsync(It.Is(epicIdMatch), It.IsAny<CancellationToken>()));

            Expression<Func<IEnumerable<string>, bool>> epicStatusMatch = s =>
                s.SequenceEqual(claimedEpics.Select(e => e.StatusName));

            context.MockSolutionEpicStatusRepository.Verify(
                e => e.CountMatchingEpicStatusAsync(It.Is(epicStatusMatch), It.IsAny<CancellationToken>()));

            context.MockSolutionEpicRepository.Verify(r => r.UpdateSolutionEpicAsync(
                ValidSolutionId,
                It.IsAny<IUpdateClaimedEpicListRequest>(),
                It.IsAny<CancellationToken>()));
        }

        [TestCase(null, null)]
        [TestCase("Id", null)]
        [TestCase(null, "StatusName")]
        public void ShouldThrowNullExceptionWhenTheEpicsInformationContainsANullProperty(string epicId, string statusName)
        {
            var claimedEpics = new HashSet<IClaimedEpic>
            {
                Mock.Of<IClaimedEpic>(e => e.EpicId == epicId && e.StatusName == statusName),
            };

            Assert.ThrowsAsync<ArgumentNullException>(() => UpdateClaimedEpicsAsync(ValidSolutionId, claimedEpics));
        }

        [Test]
        public void ShouldThrowNotFoundExceptionWhenSolutionIsNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateClaimedEpicsAsync(
                InvalidSolutionId,
                new HashSet<IClaimedEpic>()));
        }

        [Test]
        public async Task ShouldValidateIfDuplicateEpicIds()
        {
            var claimedEpics = new HashSet<IClaimedEpic>
            {
                Mock.Of<IClaimedEpic>(e => e.EpicId == "Epic1" && e.StatusName == "Passed"),
                Mock.Of<IClaimedEpic>(e => e.EpicId == "Epic2" && e.StatusName == "Not Evidenced"),
                Mock.Of<IClaimedEpic>(e => e.EpicId == "Epic1" && e.StatusName == "Passed"),
            };

            var validationResult = await UpdateClaimedEpicsAsync(ValidSolutionId, claimedEpics);
            validationResult.IsValid.Should().BeFalse();

            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["epics"].Should().Be("epicsInvalid");

            context.MockEpicRepository.Verify(
                e => e.CountMatchingEpicIdsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()),
                Times.Never());

            context.MockSolutionEpicStatusRepository.Verify(
                e => e.CountMatchingEpicStatusAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()),
                Times.Never());

            Expression<Func<ISolutionEpicRepository, Task>> expression = r => r.UpdateSolutionEpicAsync(
                ValidSolutionId,
                It.IsAny<IUpdateClaimedEpicListRequest>(),
                It.IsAny<CancellationToken>());

            context.MockSolutionEpicRepository.Verify(expression, Times.Never());
        }

        [Test]
        public async Task ShouldValidateCountIsCorrect()
        {
            const int expectedEpicsCount = 0;

            var claimedEpics = new HashSet<IClaimedEpic>
            {
                Mock.Of<IClaimedEpic>(e => e.EpicId == "Test" && e.StatusName == "Passed"),
                Mock.Of<IClaimedEpic>(e => e.EpicId == "Epic2" && e.StatusName == "Unknown"),
            };

            context.MockEpicRepository
                .Setup(e => e.CountMatchingEpicIdsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEpicsCount);

            context.MockSolutionEpicStatusRepository
                .Setup(e => e.CountMatchingEpicStatusAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEpicsCount);

            var validationResult = await UpdateClaimedEpicsAsync(ValidSolutionId, claimedEpics);
            validationResult.IsValid.Should().BeFalse();

            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["epics"].Should().Be("epicsInvalid");

            context.MockEpicRepository.Verify(
                e => e.CountMatchingEpicIdsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()));

            context.MockSolutionEpicStatusRepository.Verify(
                e => e.CountMatchingEpicStatusAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()));

            Expression<Func<ISolutionEpicRepository, Task>> expression = r => r.UpdateSolutionEpicAsync(
                ValidSolutionId,
                It.IsAny<IUpdateClaimedEpicListRequest>(),
                It.IsAny<CancellationToken>());

            context.MockSolutionEpicRepository.Verify(expression, Times.Never());
        }

        private async Task<ISimpleResult> UpdateClaimedEpicsAsync(string solutionId, IEnumerable<IClaimedEpic> claimedEpics)
        {
            return await context.UpdateClaimedEpicsHandler.Handle(
                new UpdateClaimedEpicsCommand(solutionId, claimedEpics),
                CancellationToken.None);
        }
    }
}
