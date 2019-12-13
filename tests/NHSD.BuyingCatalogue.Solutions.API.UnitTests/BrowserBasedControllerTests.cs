using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class BrowserBasedControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private BrowserBasedController _browserBasedController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _browserBasedController = new BrowserBasedController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            var result = (await _browserBasedController.GetBrowserBasedAsync(SolutionId).ConfigureAwait(false)) as NotFoundResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task ShouldGetBrowserBasedStaticData()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>()).ConfigureAwait(false);

            browserBasedResult.Should().NotBeNull();
            browserBasedResult.BrowserBasedDashboardSections.Should().NotBeNull();

            var browsersSupportedSection = browserBasedResult.BrowserBasedDashboardSections.BrowsersSupportedSection;
            AssertSectionMandatoryAndComplete(browsersSupportedSection, true, false);

            var mobileFirstSection = browserBasedResult.BrowserBasedDashboardSections.BrowserMobileFirstSection;
            AssertSectionMandatoryAndComplete(mobileFirstSection, true, false);

            var plugInsSection = browserBasedResult.BrowserBasedDashboardSections.PluginsOrExtensionsSection;
            AssertSectionMandatoryAndComplete(plugInsSection, true, false);

            var connectivitySection = browserBasedResult.BrowserBasedDashboardSections.ConnectivityAndResolutionSection;
            AssertSectionMandatoryAndComplete(connectivitySection, true, false);

            var hardwareSection = browserBasedResult.BrowserBasedDashboardSections.HardwareRequirementsSection;
            AssertSectionMandatoryAndComplete(hardwareSection, false, false);

            var additionalSection = browserBasedResult.BrowserBasedDashboardSections.BrowserAdditionalInformationSection;
            AssertSectionMandatoryAndComplete(additionalSection, false, false);
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteNullClientApplication()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>()).ConfigureAwait(false);
            AssertBrowsersSupportedSectionComplete(browserBasedResult, false);
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteNullBrowsersSupported()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>())).ConfigureAwait(false);

            AssertBrowsersSupportedSectionComplete(browserBasedResult, false);
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteEmptyBrowsersSupported()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.BrowsersSupported == new HashSet<string>()))).ConfigureAwait(false);

            AssertBrowsersSupportedSectionComplete(browserBasedResult, false);
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteNullMobileResponsive()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.BrowsersSupported == new HashSet<string>{ "A" }))).ConfigureAwait(false);

            AssertBrowsersSupportedSectionComplete(browserBasedResult, false);
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteBrowsersSupportedNullAndMobileResponsive()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.MobileResponsive == false))).ConfigureAwait(false);

            AssertBrowsersSupportedSectionComplete(browserBasedResult, false);
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteBrowsersSupportedAndMobileResponsive()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.BrowsersSupported == new HashSet<string>{ "A" } && c.MobileResponsive == false))).ConfigureAwait(false);

            AssertBrowsersSupportedSectionComplete(browserBasedResult, true);
        }

        [TestCase(null, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public async Task ShouldGetMobileFirstCalculateCompleteMobileFirstRequired(bool? required, bool isComplete)
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication.MobileFirstDesign == required)).ConfigureAwait(false);

            browserBasedResult.BrowserBasedDashboardSections.BrowserMobileFirstSection.Status.Should().Be(isComplete ? "COMPLETE" : "INCOMPLETE");
        }


        [TestCase("1GBps", "1x1", true)]
        [TestCase("1GBps", null, true)]
        [TestCase(null, "1x1", false)]
        [TestCase(null, null, false)]
        public async Task ShouldGetConnectivityAndResolutionCompleteWhenConnectivityIsComplete(string speed, string resolution, bool isComplete)
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c => c.MinimumConnectionSpeed == speed && c.MinimumDesktopResolution == resolution))).ConfigureAwait(false);
            browserBasedResult.BrowserBasedDashboardSections.ConnectivityAndResolutionSection.Status.Should()
                .Be(isComplete ? "COMPLETE" : "INCOMPLETE");
        }

        [TestCase(null, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public async Task ShouldGetBrowserBasedCalculateCompletePluginRequired(bool? pluginRequired, bool complete)
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication.Plugins == Mock.Of<IPlugins>(c => c.Required == pluginRequired && c.AdditionalInformation == null))).ConfigureAwait(false);

            AssertPluginsSectionComplete(browserBasedResult, complete);
        }

        [TestCase(null, false)]
        [TestCase("Some Hardware", true)]
        public async Task ShouldGetBrowserHardwareRequirementIsComplete(string hardware, bool isComplete)
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>(s =>
                    s.ClientApplication == Mock.Of<IClientApplication>(c => c.HardwareRequirements == hardware)))
                .ConfigureAwait(false);

            browserBasedResult.BrowserBasedDashboardSections.HardwareRequirementsSection.Status.Should().Be(isComplete ? "COMPLETE" : "INCOMPLETE");
        }

        [TestCase(null, false)]
        [TestCase("Additional Info", true)]
        public async Task ShouldGetBrowserAdditionalInformationIsComplete(string information, bool isComplete)
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>(s =>
                    s.ClientApplication == Mock.Of<IClientApplication>(c => c.AdditionalInformation == information)))
                .ConfigureAwait(false);

            browserBasedResult.BrowserBasedDashboardSections.BrowserAdditionalInformationSection.Status.Should().Be(isComplete ? "COMPLETE" : "INCOMPLETE");
        }


        private async Task<BrowserBasedResult> GetBrowserBasedSectionAsync(ISolution solution)
        {
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await _browserBasedController.GetBrowserBasedAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
            return result.Value as BrowserBasedResult;
        }

        private void AssertBrowsersSupportedSectionComplete(BrowserBasedResult browserBasedResult, bool shouldBeComplete)
        {
            browserBasedResult.BrowserBasedDashboardSections.BrowsersSupportedSection.Status.Should().Be(shouldBeComplete ? "COMPLETE" : "INCOMPLETE");
        }

        private void AssertPluginsSectionComplete(BrowserBasedResult browserBasedResult, bool shouldBeComplete)
        {
            browserBasedResult.BrowserBasedDashboardSections.PluginsOrExtensionsSection.Status.Should().Be(shouldBeComplete ? "COMPLETE" : "INCOMPLETE");
        }

        private void AssertSectionMandatoryAndComplete(BrowserBasedDashboardSection section, bool shouldBeMandatory, bool shouldBeComplete)
        {
            section.Status.Should().Be(shouldBeComplete ? "COMPLETE" : "INCOMPLETE");
            section.Requirement.Should().Be(shouldBeMandatory ? "Mandatory" : "Optional");
        }
    }
}
