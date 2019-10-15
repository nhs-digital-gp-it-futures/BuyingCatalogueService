using System.Threading;
using System.Threading.Tasks;
using Moq;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionUpdateSummaryTests
    {
        private TestContext _context;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
        }

        [Test]
        public async Task ShouldUpdateSolutionSummary()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            await _context.UpdateSolutionSummaryHandler.Handle(new UpdateSolutionSummaryCommand("Sln1",
                new UpdateSolutionSummaryViewModel
                {
                    Description = "Description",
                    AboutUrl = "AboutUrl",
                    Summary = "Summary"
                }), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionRepository.Verify(r => r.UpdateSummaryAsync(It.Is<IUpdateSolutionSummaryRequest>(r =>
                r.Id == "Sln1"
                && r.AboutUrl == "AboutUrl"
                && r.Description == "Description"
                && r.Summary == "Summary"
                ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            var exception = Assert.ThrowsAsync<NotFoundException>(() =>  
             _context.UpdateSolutionSummaryHandler.Handle(new UpdateSolutionSummaryCommand("Sln1",
                new UpdateSolutionSummaryViewModel
                {
                    Description = "Description",
                    AboutUrl = "AboutUrl",
                    Summary = "Summary"
                }), new CancellationToken()));

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionRepository.Verify(r => r.UpdateSummaryAsync(It.IsAny<IUpdateSolutionSummaryRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}
