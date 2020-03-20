using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadmap;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    public class SolutionUpdateRoadmapTests
    {
        private TestContext _context;
        private string _existingSolutionId = "Sln1";
        private string _invalidSolutionId = "Sln123";

        [SetUp]
        public void Setup()
        {
            _context = new TestContext();
            _context.MockSolutionRepository.Setup(x => x.CheckExists(_existingSolutionId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _context.MockSolutionRepository.Setup(x => x.CheckExists(_invalidSolutionId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        }

        [Test]
        public async Task ShouldUpdateSolutionRoadMap()
        {
            var expected = "a description";

            var validationResult = await UpdateRoadmapAsync(_existingSolutionId, expected)
                .ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(0);

            _context.MockSolutionRepository.Verify(r => r.CheckExists(_existingSolutionId, It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionDetailRepository.Verify(r => r.UpdateRoadmapAsync(It.Is<IUpdateRoadmapRequest>(r =>
                r.SolutionId == _existingSolutionId
                && r.Description == expected
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldValidateSingleMaxLength()
        {
            var validationResult = await UpdateRoadmapAsync(_existingSolutionId, new string('a', 1001))
                .ConfigureAwait(false);
            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["summary"].Should().Be("maxLength");

            _context.MockSolutionRepository.Verify(r => r.CheckExists(_existingSolutionId, It.IsAny<CancellationToken>()), Times.Never());
            _context.MockSolutionDetailRepository.Verify(r => r.UpdateRoadmapAsync(It.IsAny<IUpdateRoadmapRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void ShouldThrowNotFoundExceptionWhenSolutionIsNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateRoadmapAsync(_invalidSolutionId, "A description"));
        }

        private async Task<ISimpleResult> UpdateRoadmapAsync(string solutionId, string description)
        {
            var validationResult = await _context.UpdateRoadmapHandler.Handle(
                    new UpdateRoadmapCommand(solutionId, description), new CancellationToken())
                .ConfigureAwait(false);
            return validationResult;
        }
    }
}
