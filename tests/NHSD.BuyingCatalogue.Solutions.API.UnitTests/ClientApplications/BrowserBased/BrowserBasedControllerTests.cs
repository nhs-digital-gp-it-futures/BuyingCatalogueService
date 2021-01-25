using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.BrowserBased
{
    [TestFixture]
    internal sealed class BrowserBasedControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private BrowserBasedController browserBasedController;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            browserBasedController = new BrowserBasedController(mockMediator.Object);
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = (await browserBasedController.GetBrowserBasedAsync(SolutionId)) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            var browserBasedResult = result.Value as BrowserBasedResult;

            Assert.NotNull(browserBasedResult);
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
        public async Task ShouldGetBrowserBasedStaticData()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<IClientApplication>());

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
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<IClientApplication>());
            AssertBrowsersSupportedSectionComplete(browserBasedResult, false);
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteNullBrowsersSupported()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<IClientApplication>());

            AssertBrowsersSupportedSectionComplete(browserBasedResult, false);
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteEmptyBrowsersSupported()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(
                Mock.Of<IClientApplication>(c => c.BrowsersSupported == new HashSet<string>()));

            AssertBrowsersSupportedSectionComplete(browserBasedResult, false);
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteNullMobileResponsive()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(
                Mock.Of<IClientApplication>(c => c.BrowsersSupported == new HashSet<string> { "A" }));

            AssertBrowsersSupportedSectionComplete(browserBasedResult, false);
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteBrowsersSupportedNullAndMobileResponsive()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(
                Mock.Of<IClientApplication>(c => c.MobileResponsive == false));

            AssertBrowsersSupportedSectionComplete(browserBasedResult, false);
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteBrowsersSupportedAndMobileResponsive()
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.BrowsersSupported == new HashSet<string> { "A" }
                && c.MobileResponsive == false;

            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of(clientApplication));

            AssertBrowsersSupportedSectionComplete(browserBasedResult, true);
        }

        [TestCase(null, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public async Task ShouldGetMobileFirstCalculateCompleteMobileFirstRequired(bool? required, bool isComplete)
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(
                Mock.Of<IClientApplication>(c => c.MobileFirstDesign == required));

            browserBasedResult.BrowserBasedDashboardSections.BrowserMobileFirstSection.Status.Should().Be(
                isComplete ? "COMPLETE" : "INCOMPLETE");
        }

        [TestCase("1GBps", "1x1", true)]
        [TestCase("1GBps", null, true)]
        [TestCase(null, "1x1", false)]
        [TestCase(null, null, false)]
        public async Task ShouldGetConnectivityAndResolutionCompleteWhenConnectivityIsComplete(
            string speed,
            string resolution,
            bool isComplete)
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.MinimumConnectionSpeed == speed
                && c.MinimumDesktopResolution == resolution;

            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of(clientApplication));

            browserBasedResult.BrowserBasedDashboardSections.ConnectivityAndResolutionSection.Status.Should().Be(
                isComplete ? "COMPLETE" : "INCOMPLETE");
        }

        [TestCase(null, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public async Task ShouldGetBrowserBasedCalculateCompletePluginRequired(bool? pluginRequired, bool complete)
        {
            Expression<Func<IPlugins, bool>> plugins = p =>
                p.Required == pluginRequired
                && p.AdditionalInformation == null;

            Expression<Func<IClientApplication, bool>> clientApplication = c => c.Plugins == Mock.Of(plugins);

            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of(clientApplication));

            AssertPluginsSectionComplete(browserBasedResult, complete);
        }

        [TestCase(null, false)]
        [TestCase("Some Hardware", true)]
        public async Task ShouldGetBrowserHardwareRequirementIsComplete(string hardware, bool isComplete)
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(
                Mock.Of<IClientApplication>(c => c.HardwareRequirements == hardware));

            browserBasedResult.BrowserBasedDashboardSections.HardwareRequirementsSection.Status.Should().Be(
                isComplete ? "COMPLETE" : "INCOMPLETE");
        }

        [TestCase(null, false)]
        [TestCase("Additional Info", true)]
        public async Task ShouldGetBrowserAdditionalInformationIsComplete(string information, bool isComplete)
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(
                Mock.Of<IClientApplication>(c => c.AdditionalInformation == information));

            browserBasedResult.BrowserBasedDashboardSections.BrowserAdditionalInformationSection.Status.Should().Be(
                isComplete ? "COMPLETE" : "INCOMPLETE");
        }

        private static void AssertBrowsersSupportedSectionComplete(BrowserBasedResult browserBasedResult, bool shouldBeComplete)
        {
            browserBasedResult.BrowserBasedDashboardSections.BrowsersSupportedSection.Status.Should().Be(
                shouldBeComplete ? "COMPLETE" : "INCOMPLETE");
        }

        private static void AssertPluginsSectionComplete(BrowserBasedResult browserBasedResult, bool shouldBeComplete)
        {
            browserBasedResult.BrowserBasedDashboardSections.PluginsOrExtensionsSection.Status.Should().Be(
                shouldBeComplete ? "COMPLETE" : "INCOMPLETE");
        }

        private static void AssertSectionMandatoryAndComplete(
            BrowserBasedDashboardSection section,
            bool shouldBeMandatory,
            bool shouldBeComplete)
        {
            section.Status.Should().Be(shouldBeComplete ? "COMPLETE" : "INCOMPLETE");
            section.Requirement.Should().Be(shouldBeMandatory ? "Mandatory" : "Optional");
        }

        private async Task<BrowserBasedResult> GetBrowserBasedSectionAsync(IClientApplication clientApplication)
        {
            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientApplication);

            var result = (await browserBasedController.GetBrowserBasedAsync(SolutionId)) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            mockMediator.Verify(m => m.Send(
                It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));

            return result.Value as BrowserBasedResult;
        }
    }
}
