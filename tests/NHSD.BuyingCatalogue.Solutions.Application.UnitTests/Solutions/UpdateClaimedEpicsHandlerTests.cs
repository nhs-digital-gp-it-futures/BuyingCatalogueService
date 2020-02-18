using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateClaimedEpics;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence.Epics;
using NHSD.BuyingCatalogue.Solutions.Contracts.Epics;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    public sealed class UpdateClaimedEpicsHandlerTests
    {
        private const string ValidSolutionId = "Sln1";
        private const string InvalidSolutionId = "Sln123";
        private TestContext _context;

        [SetUp]
        public void Setup()
        {
            _context = new TestContext();
            _context.MockSolutionRepository.Setup(x => x.CheckExists(ValidSolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _context.MockSolutionRepository.Setup(x => x.CheckExists(InvalidSolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
        }

        [Test]
        public async Task ShouldUpdateEpicsAsync()
        {
            const int expectedEpicsCount = 1;

            var claimedEpics = new HashSet<IClaimedEpic>
            {
                Mock.Of<IClaimedEpic>(e => e.EpicId == "Epic1" && e.StatusName == "Passed")
            };

            _context.MockEpicRepository
                .Setup(e => e.CountMatchingEpicIdsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEpicsCount);

            _context.MockSolutionEpicStatusRepository
                .Setup(e => e.CountMatchingEpicStatusAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEpicsCount);

            var validationResult = await UpdateClaimedEpicsAsync(ValidSolutionId, claimedEpics).ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            _context.MockSolutionRepository.Verify(s => s.CheckExists(ValidSolutionId, It.IsAny<CancellationToken>()),
                Times.Once);

            var a = claimedEpics.Select(e => e.EpicId);

            _context.MockEpicRepository.Verify(
                e => e.CountMatchingEpicIdsAsync(It.Is<IEnumerable<string>>(x => x.SequenceEqual(claimedEpics.Select(e => e.EpicId))), It.IsAny<CancellationToken>()), Times.Once);
            _context.MockSolutionEpicStatusRepository.Verify(
                e => e.CountMatchingEpicStatusAsync(It.Is<IEnumerable<string>>(x => x.SequenceEqual(claimedEpics.Select(e => e.StatusName))), It.IsAny<CancellationToken>()), Times.Once);

            _context.MockSolutionEpicRepository.Verify(r => r.UpdateSolutionEpicAsync(ValidSolutionId,
                It.IsAny<IUpdateClaimedEpicListRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCase(null, null)]
        [TestCase("Id", null)]
        [TestCase(null, "StatusName")]
        public void ShouldThrowNullExceptionWhenTheEpicsInformationContainsANullProperty(string epicId, string statusName)
        {
            var claimedEpics = new HashSet<IClaimedEpic>
            {
                Mock.Of<IClaimedEpic>(e => e.EpicId == epicId && e.StatusName == statusName)
            };

            Assert.ThrowsAsync<ArgumentNullException>(() => UpdateClaimedEpicsAsync(ValidSolutionId, claimedEpics));
        }

        [Test]
        public void ShouldThrowNotFoundExceptionWhenSolutionIsNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateClaimedEpicsAsync(InvalidSolutionId, new HashSet<IClaimedEpic>()));
        }

        [Test]
        public async Task ShouldValidateIfDuplicateEpicIds()
        {
            var claimedEpics = new HashSet<IClaimedEpic>
            {
                Mock.Of<IClaimedEpic>(e => e.EpicId == "Epic1" && e.StatusName == "Passed"),
                Mock.Of<IClaimedEpic>(e => e.EpicId == "Epic2" && e.StatusName == "Not Evidenced"),
                Mock.Of<IClaimedEpic>(e => e.EpicId == "Epic1" && e.StatusName == "Passed")
            };

            var validationResult = await UpdateClaimedEpicsAsync(ValidSolutionId, claimedEpics).ConfigureAwait(false);
            validationResult.IsValid.Should().BeFalse();

            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["epics"].Should().Be("epicsInvalid");

            _context.MockEpicRepository.Verify(
                e => e.CountMatchingEpicIdsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()), Times.Never);
            _context.MockSolutionEpicStatusRepository.Verify(
                e => e.CountMatchingEpicStatusAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()), Times.Never);

            _context.MockSolutionEpicRepository.Verify(r => r.UpdateSolutionEpicAsync(ValidSolutionId,
                It.IsAny<IUpdateClaimedEpicListRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task ShouldValidateCountIsCorrect()
        {
            var expectedEpicsCount = 0;

            var claimedEpics = new HashSet<IClaimedEpic>
            {
                Mock.Of<IClaimedEpic>(e => e.EpicId == "Test" && e.StatusName == "Passed"),
                Mock.Of<IClaimedEpic>(e => e.EpicId == "Epic2" && e.StatusName == "Unknown")
            };

            _context.MockEpicRepository
                .Setup(e => e.CountMatchingEpicIdsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEpicsCount);
            _context.MockSolutionEpicStatusRepository
                .Setup(e => e.CountMatchingEpicStatusAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEpicsCount);

            var validationResult = await UpdateClaimedEpicsAsync(ValidSolutionId, claimedEpics).ConfigureAwait(false);
            validationResult.IsValid.Should().BeFalse();

            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["epics"].Should().Be("epicsInvalid");

            _context.MockEpicRepository.Verify(
                e => e.CountMatchingEpicIdsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()), Times.Once);
            _context.MockSolutionEpicStatusRepository.Verify(
                e => e.CountMatchingEpicStatusAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()), Times.Once);

            _context.MockSolutionEpicRepository.Verify(r => r.UpdateSolutionEpicAsync(ValidSolutionId,
                It.IsAny<IUpdateClaimedEpicListRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private async Task<ISimpleResult> UpdateClaimedEpicsAsync(string solutionId, HashSet<IClaimedEpic> claimedEpics)
        {
            return await _context.UpdateClaimedEpicsHandler
                .Handle(new UpdateClaimedEpicsCommand(solutionId, claimedEpics), new CancellationToken())
                .ConfigureAwait(false);
        }
    }
}
