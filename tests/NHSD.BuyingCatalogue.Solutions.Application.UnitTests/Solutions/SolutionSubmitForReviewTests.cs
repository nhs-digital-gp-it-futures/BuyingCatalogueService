using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.SubmitForReview;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionSubmitForReviewTests
    {
        private TestContext context;

        [SetUp]
        public void SetUpFixture()
        {
            context = new TestContext();
        }

        [Test]
        public async Task ShouldSubmitSolutionForReview()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Summary).Returns("Sln1 summary");

            const string clientApplicationJson = "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], "
                + "'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], "
                + "'MobileResponsive': false, "
                + "'Plugins': { 'Required': false } }";

            existingSolution.Setup(s => s.ClientApplication).Returns(clientApplicationJson);

            context.MockSolutionRepository
                .Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSolution.Object);

            await context.SubmitSolutionForReviewHandler.Handle(
                new SubmitSolutionForReviewCommand("Sln1"),
                CancellationToken.None);

            context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionSupplierStatusRequest, bool>> match = r =>
                r.Id == "Sln1"
                && r.SupplierStatusId == 2;

            context.MockSolutionRepository.Verify(r => r.UpdateSupplierStatusAsync(
                It.Is(match),
                It.IsAny<CancellationToken>()));
        }

        [TestCase(null, new[] { "SolutionSummaryIsRequired" })]
        [TestCase("", new[] { "SolutionSummaryIsRequired" })]
        public async Task ShouldNotSubmitSolutionForReviewWhenSummaryIsMissing(string summary, string[] errorList)
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Summary).Returns(summary);

            const string clientApplicationJson = "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], "
                + "'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], "
                + "'MobileResponsive': false, "
                + "'Plugins': { 'Required': false } }";

            existingSolution.Setup(s => s.ClientApplication).Returns(clientApplicationJson);

            context.MockSolutionRepository
                .Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSolution.Object);

            var result = await context.SubmitSolutionForReviewHandler.Handle(
                new SubmitSolutionForReviewCommand("Sln1"),
                CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Select(s => s.Id).Should().BeEquivalentTo(errorList);

            context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));
        }

        [TestCase(null, new[] { "ClientApplicationTypeIsRequired" })]
        [TestCase("", new[] { "ClientApplicationTypeIsRequired" })]
        [TestCase("{ 'ClientApplicationTypes' : [] }", new[] { "ClientApplicationTypeIsRequired" })]
        public async Task ShouldNotSubmitSolutionForReviewWhenClientApplicationTypesAreMissing(
            string clientApplication,
            string[] errorList)
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Summary).Returns("Sln1 summary");
            existingSolution.Setup(s => s.ClientApplication).Returns(clientApplication);

            context.MockSolutionRepository
                .Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSolution.Object);

            var result = await context.SubmitSolutionForReviewHandler.Handle(
                new SubmitSolutionForReviewCommand("Sln1"),
                CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Select(s => s.Id).Should().BeEquivalentTo(errorList);

            context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));
        }

        [TestCase("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'MobileResponsive': false, 'Plugins': { 'Required': false } }", new[] { "SupportedBrowserIsRequired" })]
        [TestCase("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [], 'MobileResponsive': false, 'Plugins': { 'Required': false } }", new[] { "SupportedBrowserIsRequired" })]
        public async Task ShouldNotSubmitSolutionForReviewWhenBrowserSupportedIsMissing(
            string clientApplication,
            string[] errorList)
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Summary).Returns("Sln1 summary");
            existingSolution.Setup(s => s.ClientApplication).Returns(clientApplication);

            context.MockSolutionRepository
                .Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSolution.Object);

            var result = await context.SubmitSolutionForReviewHandler.Handle(
                new SubmitSolutionForReviewCommand("Sln1"),
                CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Select(s => s.Id).Should().BeEquivalentTo(errorList);

            context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));
        }

        [TestCase("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'Plugins': { 'Required': false } }", new[] { "MobileResponsiveIsRequired" })]
        public async Task ShouldNotSubmitSolutionForReviewWhenMobileResponsiveIsMissing(
            string clientApplication,
            string[] errorList)
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Summary).Returns("Sln1 summary");
            existingSolution.Setup(s => s.ClientApplication).Returns(clientApplication);

            context.MockSolutionRepository
                .Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSolution.Object);

            var result = await context.SubmitSolutionForReviewHandler.Handle(
                new SubmitSolutionForReviewCommand("Sln1"),
                CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Select(s => s.Id).Should().BeEquivalentTo(errorList);

            context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));
        }

        [TestCase("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false }", new[] { "PluginRequirementIsRequired" })]
        [TestCase("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false, 'Plugins': { } }", new[] { "PluginRequirementIsRequired" })]
        [TestCase("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false, 'Plugins': { 'Required': null } }", new[] { "PluginRequirementIsRequired" })]
        public async Task ShouldNotSubmitSolutionForReviewWhenPluginRequirementIsMissing(
            string clientApplication,
            string[] errorList)
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Summary).Returns("Sln1 summary");
            existingSolution.Setup(s => s.ClientApplication).Returns(clientApplication);

            context.MockSolutionRepository
                .Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSolution.Object);

            var result = await context.SubmitSolutionForReviewHandler.Handle(
                new SubmitSolutionForReviewCommand("Sln1"),
                CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Select(s => s.Id).Should().BeEquivalentTo(errorList);

            context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => context.SubmitSolutionForReviewHandler.Handle(
                new SubmitSolutionForReviewCommand("Sln1"),
                CancellationToken.None));

            context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));

            Expression<Func<ISolutionRepository, Task>> expression = r => r.UpdateSupplierStatusAsync(
                It.IsAny<IUpdateSolutionSupplierStatusRequest>(),
                It.IsAny<CancellationToken>());

            context.MockSolutionRepository.Verify(expression, Times.Never());
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("   ")]
        public void ShouldThrowWhenSolutionIdNotPresent(string blanks)
        {
            Assert.Throws<ArgumentException>(() => _ = new SubmitSolutionForReviewCommand(blanks));
        }
    }
}
