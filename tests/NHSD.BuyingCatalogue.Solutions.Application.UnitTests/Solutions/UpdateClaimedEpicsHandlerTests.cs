using System.Collections.Generic;
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
            var claimedEpics = new HashSet<IClaimedEpic>
            {
                Mock.Of<IClaimedEpic>(e => e.EpicId == "Epic1" && e.StatusName == "Passed")
            };

            var validationResult = await UpdateClaimedEpicsAsync(ValidSolutionId, claimedEpics).ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();
            _context.MockSolutionRepository.Verify(s => s.CheckExists(ValidSolutionId, It.IsAny<CancellationToken>()),
                Times.Once);
            _context.MockSolutionEpicRepository.Verify(r => r.UpdateSolutionEpicAsync(ValidSolutionId,
                It.IsAny<IUpdateClaimedEpicListRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCase(null, null)]
        [TestCase("Id", null)]
        [TestCase(null, "StatusName")]
        public void ShouldThrowNotFoundExceptionWhenEpicsInfoIsNull(string epicId, string statusName)
        {
            var claimedEpics = new HashSet<IClaimedEpic>
            {
                Mock.Of<IClaimedEpic>(e => e.EpicId == epicId && e.StatusName == statusName)
            };

            Assert.ThrowsAsync<NotFoundException>(() => UpdateClaimedEpicsAsync(InvalidSolutionId, claimedEpics));
        }

        [Test]
        public void ShouldThrowNotFoundExceptionWhenSolutionIsNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateClaimedEpicsAsync(InvalidSolutionId, new HashSet<IClaimedEpic>()));
        }

        private async Task<ISimpleResult> UpdateClaimedEpicsAsync(string solutionId, HashSet<IClaimedEpic> claimedEpics)
        {
            return await _context.UpdateClaimedEpicsHandler
                .Handle(new UpdateClaimedEpicsCommand(solutionId, claimedEpics), new CancellationToken())
                .ConfigureAwait(false);
        }
    }
}
