using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.SubmitForReview;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
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
            existingSolution.Setup(s => s.ClientApplication).Returns("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false, 'Plugins': { 'Required': false } }");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            await _context.SubmitSolutionForReviewHandler.Handle(new SubmitSolutionForReviewCommand("Sln1"), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionRepository.Verify(r => r.UpdateSupplierStatusAsync(It.Is<IUpdateSolutionSupplierStatusRequest>(r =>
                r.Id == "Sln1"
                && r.SupplierStatusId == 2
                ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestCase(null, new[] { "SolutionSummaryIsRequired" })]
        [TestCase("", new[] { "SolutionSummaryIsRequired" })]
        public async Task ShouldNotSubmitSolutionForReviewWhenSummaryIsMissing(string summary, string[] errorList)
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Summary).Returns(summary);
            existingSolution.Setup(s => s.ClientApplication).Returns("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false, 'Plugins': { 'Required': false } }");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var result = await _context.SubmitSolutionForReviewHandler.Handle(new SubmitSolutionForReviewCommand("Sln1"), new CancellationToken());

            result.IsFailure.Should().BeTrue();
            result.Errors.Select(s => s.Id).Should().BeEquivalentTo(errorList);

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestCase(null, new[] { "ClientApplicationTypeIsRequired" })]
        [TestCase("", new[] { "ClientApplicationTypeIsRequired" })]
        [TestCase("{ 'ClientApplicationTypes' : [] }", new[] { "ClientApplicationTypeIsRequired" })]
        public async Task ShouldNotSubmitSolutionForReviewWhenClientApplicationTypesAreMissing(string clientApplication, string[] errorList)
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Summary).Returns("Sln1 summary");
            existingSolution.Setup(s => s.ClientApplication).Returns(clientApplication);

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var result = await _context.SubmitSolutionForReviewHandler.Handle(new SubmitSolutionForReviewCommand("Sln1"), new CancellationToken());

            result.IsFailure.Should().BeTrue();
            result.Errors.Select(s => s.Id).Should().BeEquivalentTo(errorList);

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestCase("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'MobileResponsive': false, 'Plugins': { 'Required': false } }", new[] { "SupportedBrowserIsRequired" })]
        [TestCase("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [], 'MobileResponsive': false, 'Plugins': { 'Required': false } }", new[] { "SupportedBrowserIsRequired" })]
        public async Task ShouldNotSubmitSolutionForReviewWhenBrowserSupportedIsMissing(string clientApplication, string[] errorList)
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Summary).Returns("Sln1 summary");
            existingSolution.Setup(s => s.ClientApplication).Returns(clientApplication);

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var result = await _context.SubmitSolutionForReviewHandler.Handle(new SubmitSolutionForReviewCommand("Sln1"), new CancellationToken());

            result.IsFailure.Should().BeTrue();
            result.Errors.Select(s => s.Id).Should().BeEquivalentTo(errorList);

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestCase("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'Plugins': { 'Required': false } }", new[] { "MobileResponsiveIsRequired" })]
        public async Task ShouldNotSubmitSolutionForReviewWhenMobileResponsiveIsMissing(string clientApplication, string[] errorList)
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Summary).Returns("Sln1 summary");
            existingSolution.Setup(s => s.ClientApplication).Returns(clientApplication);

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var result = await _context.SubmitSolutionForReviewHandler.Handle(new SubmitSolutionForReviewCommand("Sln1"), new CancellationToken());

            result.IsFailure.Should().BeTrue();
            result.Errors.Select(s => s.Id).Should().BeEquivalentTo(errorList);

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestCase("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false }", new[] { "PluginRequirementIsRequired" })]
        [TestCase("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false, 'Plugins': { } }", new[] { "PluginRequirementIsRequired" })]
        [TestCase("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false, 'Plugins': { 'Required': null } }", new[] { "PluginRequirementIsRequired" })]
        public async Task ShouldNotSubmitSolutionForReviewWhenPluginRequirementIsMissing(string clientApplication, string[] errorList)
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Summary).Returns("Sln1 summary");
            existingSolution.Setup(s => s.ClientApplication).Returns(clientApplication);

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var result = await _context.SubmitSolutionForReviewHandler.Handle(new SubmitSolutionForReviewCommand("Sln1"), new CancellationToken());

            result.IsFailure.Should().BeTrue();
            result.Errors.Select(s => s.Id).Should().BeEquivalentTo(errorList);

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() =>
                _context.SubmitSolutionForReviewHandler.Handle(new SubmitSolutionForReviewCommand("Sln1"), new CancellationToken()));

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionRepository.Verify(r => r.UpdateSupplierStatusAsync(It.IsAny<IUpdateSolutionSupplierStatusRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}
