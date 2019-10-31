using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;
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
            existingSolution.Setup(s => s.Summary).Returns("Sln1 summary");
            existingSolution.Setup(s => s.ClientApplication).Returns("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false }");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            await _context.SubmitSolutionForReviewHandler.Handle(new SubmitSolutionForReviewCommand("Sln1"), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionRepository.Verify(r => r.UpdateSupplierStatusAsync(It.Is<IUpdateSolutionSupplierStatusRequest>(r =>
                r.Id == "Sln1"
                && r.SupplierStatusId == 2
                ), It.IsAny<CancellationToken>()), Times.Once());
        }

        
        [TestCase("Sln1 summary", null, 1)]
        [TestCase("Sln1 summary", "", 1)]
        [TestCase("Sln1 summary", "{ 'ClientApplicationTypes' : [] }", 1)]
        [TestCase("Sln1 summary", "{ 'ClientApplicationTypes' : [ 'browser-based' ] }", 2)]
        [TestCase("Sln1 summary", "{ 'ClientApplicationTypes' : [ 'browser-based' ], 'BrowsersSupported' : [], 'MobileResponsive': false }", 1)]
        [TestCase("Sln1 summary", "{ 'ClientApplicationTypes' : [ 'browser-based' ], 'BrowsersSupported' : [ 'Mozilla Firefox' ] }", 1)]
        [TestCase(null, "{ 'ClientApplicationTypes' : [ 'browser-based' ], 'BrowsersSupported' : [ 'Mozilla Firefox' ], 'MobileResponsive': false }", 1)]
        [TestCase("", "{ 'ClientApplicationTypes' : [ 'browser-based' ], 'BrowsersSupported' : [ 'Mozilla Firefox' ], 'MobileResponsive': false }", 1)]
        [TestCase(null, null, 2)]
        public async Task ShouldNotSubmitSolutionForReview(string summary, string clientApplication, int expectedErrorCount)
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Summary).Returns(summary);
            existingSolution.Setup(s => s.ClientApplication).Returns(clientApplication);

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var result = await _context.SubmitSolutionForReviewHandler.Handle(new SubmitSolutionForReviewCommand("Sln1"), new CancellationToken());

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().HaveCount(expectedErrorCount);

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionRepository.Verify(r => r.UpdateSupplierStatusAsync(It.Is<IUpdateSolutionSupplierStatusRequest>(r =>
                r.Id == "Sln1"
                && r.SupplierStatusId == 2
            ), It.IsAny<CancellationToken>()), Times.Never);
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
