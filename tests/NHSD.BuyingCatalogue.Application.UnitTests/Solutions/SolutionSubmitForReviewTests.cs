using System.Threading;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionSubmitForReviewTests
    {
        private TestContext _context;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
        }

        [Test]
        public async Task ShouldSubmitSolutionForReview()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            await _context.SubmitSolutionForReviewHandler.Handle(new SubmitSolutionForReviewCommand("Sln1"), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionRepository.Verify(r => r.UpdateSupplierStatusAsync(It.Is<IUpdateSolutionSupplierStatusRequest>(r =>
                r.Id == "Sln1"
                && r.SupplierStatusId == 2
                ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            var exception = Assert.ThrowsAsync<NotFoundException>(() =>
                _context.SubmitSolutionForReviewHandler.Handle(new SubmitSolutionForReviewCommand("Sln1"), new CancellationToken()));

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionRepository.Verify(r => r.UpdateSupplierStatusAsync(It.IsAny<IUpdateSolutionSupplierStatusRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}
