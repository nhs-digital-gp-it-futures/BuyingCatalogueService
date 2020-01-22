using System;
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
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;
using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class SolutionsControllerGetDashboardTests
    {
        private Mock<IMediator> _mockMediator;

        private SolutionsController _solutionsController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _solutionsController = new SolutionsController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            var result = (await _solutionsController.Dashboard(SolutionId).ConfigureAwait(false)).Result as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as SolutionDashboardResult).Id.Should().BeNull();
        }

        [Test]
        public void NullSolutionShouldThrowNullExceptionDashboardSection()
        {
            Assert.Throws<ArgumentNullException>(() => new SolutionDashboardSections(null));
        }

        [Test]
        public void NullSolutionShouldReturnEmptyDashboardResult()
        {
            var dashboard = new SolutionDashboardResult(null);
            dashboard.Id.Should().BeNull();
            dashboard.Name.Should().BeNull();
            dashboard.SolutionDashboardSections.Should().BeNull();
        }

        [TestCase(null, null)]
        [TestCase("Sln2", null)]
        [TestCase(null, "Bob")]
        [TestCase("Sln2", "Bob")]
        public async Task ShouldReturnNameId(string id, string name)
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>(s => s.Id == id && s.Name == name)).ConfigureAwait(false);
            dashboardResult.Id.Should().Be(id);
            dashboardResult.Name.Should().Be(name);
        }

        [Test]
        public async Task ShouldReturnSolutionDashboardStaticData()
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>()).ConfigureAwait(false);
            var solutionDashboardSections = dashboardResult.SolutionDashboardSections;

            solutionDashboardSections.Should().NotBeNull();
            solutionDashboardSections.SolutionDescriptionSection.Should().NotBeNull();
            solutionDashboardSections.SolutionDescriptionSection.Requirement.Should().Be("Mandatory");

            solutionDashboardSections.FeaturesSection.Should().NotBeNull();
            solutionDashboardSections.FeaturesSection.Requirement.Should().Be("Optional");

            solutionDashboardSections.ClientApplicationTypesSection.Should().NotBeNull();
            solutionDashboardSections.ClientApplicationTypesSection.Requirement.Should().Be("Mandatory");
        }

        [Test]
        public async Task ShouldGetDashboardCalculateCompleteNull()
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>()).ConfigureAwait(false);
            var solutionDashboardSections = dashboardResult.SolutionDashboardSections;

            solutionDashboardSections.Should().NotBeNull();
            solutionDashboardSections.SolutionDescriptionSection.Should().NotBeNull();
            solutionDashboardSections.SolutionDescriptionSection.Status.Should().Be("INCOMPLETE");

            solutionDashboardSections.FeaturesSection.Should().NotBeNull();
            solutionDashboardSections.FeaturesSection.Status.Should().Be("INCOMPLETE");

            solutionDashboardSections.ClientApplicationTypesSection.Should().NotBeNull();
            solutionDashboardSections.ClientApplicationTypesSection.Status.Should().Be("INCOMPLETE");
        }

        [TestCase(null, "Desc", "", "INCOMPLETE")]
        [TestCase(null, "", "Link", "INCOMPLETE")]
        [TestCase(null, "Desc", "Link", "INCOMPLETE")]
        [TestCase("Summary", null, null, "COMPLETE")]
        [TestCase("Summary", "Desc", "", "COMPLETE")]
        [TestCase("Summary", "Desc", "Link", "COMPLETE")]

        public async Task ShouldGetDashboardCalculateSolutionDescription(string summary, string description, string link, string result)
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>(s => s.Summary == summary && s.Description == description && s.AboutUrl == link)).ConfigureAwait(false);
            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.SolutionDescriptionSection.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.SolutionDescriptionSection.Status.Should().Be(result);
        }

        [TestCase(false, "INCOMPLETE")]
        [TestCase(true, "COMPLETE")]
        public async Task ShouldGetDashboardCalculateCompleteFeatures(bool isFeatures, string result)
        {
            var features = isFeatures ? new[] { "Feature1", "Feature2" } : Array.Empty<string>();

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>(s => s.Features == features)).ConfigureAwait(false);
            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.FeaturesSection.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.FeaturesSection.Status.Should().Be(result);
        }

        [Test]
        public async Task ShouldGetDashboardCalculateClientApplicationTypesBrowserBasedNull()
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "browser-based" }))).ConfigureAwait(false);

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section.Should().BeOfType<ClientApplicationTypesSubSections>();
            var clientApplicationTypesSubSections = (ClientApplicationTypesSubSections) dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section;
            clientApplicationTypesSubSections.BrowserBasedSection.Should().NotBeNull();
            clientApplicationTypesSubSections.BrowserBasedSection.Status.Should().Be("INCOMPLETE");
        }

        [Test]
        public async Task ShouldGetDashboardCalculateClientApplicationTypesNativeMobileNull()
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "native-mobile" }))).ConfigureAwait(false);

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section.Should().BeOfType<ClientApplicationTypesSubSections>();
            var clientApplicationTypesSubSections = (ClientApplicationTypesSubSections) dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section;
            clientApplicationTypesSubSections.NativeMobileSection.Should().NotBeNull();
            clientApplicationTypesSubSections.NativeMobileSection.Status.Should().Be("INCOMPLETE");
        }

        [Test]
        public async Task ShouldGetDashboardCalculateClientApplicationTypesNativeDesktopNull()
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "native-desktop" }))).ConfigureAwait(false);

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section.Should().BeOfType<ClientApplicationTypesSubSections>();
            var clientApplicationTypesSubSections = (ClientApplicationTypesSubSections) dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section;
            clientApplicationTypesSubSections.NativeDesktopSection.Should().NotBeNull();
            clientApplicationTypesSubSections.NativeDesktopSection.Status.Should().Be("INCOMPLETE");
        }

        [TestCase(false, true, true, true, true, "INCOMPLETE")]
        [TestCase(true, null, true, true, true, "INCOMPLETE")]
        [TestCase(true, true, null, true, true, "INCOMPLETE")]
        [TestCase(true, true, true, false, true, "INCOMPLETE")]
        [TestCase(true, true, true, true, false, "INCOMPLETE")]
        [TestCase(true, false, false, true, true, "COMPLETE")]
        [TestCase(true, false, true, true, true, "COMPLETE")]
        [TestCase(true, true, false, true, true, "COMPLETE")]
        [TestCase(true, true, true, true, true, "COMPLETE")]

        public async Task ShouldGetDashboardCalculateClientApplicationTypeBrowserBased(
            bool someBrowsersSupported,
            bool? mobileResponsive,
            bool? isMobileFirst,
            bool isPlugins,
            bool isConnectivity,
            string result)
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "browser-based" } &&
                    c.BrowsersSupported == (someBrowsersSupported ? new HashSet<string> { "Edge", "Chrome" } : new HashSet<string>()) &&
                    c.MobileResponsive == mobileResponsive &&
                    c.MobileFirstDesign == isMobileFirst &&
                    c.Plugins == (isPlugins ? Mock.Of<IPlugins>(p => p.Required == isPlugins) : null) &&
                    c.MinimumConnectionSpeed == (isConnectivity ? "Some Connectivity" : null)))).ConfigureAwait(false);

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section.Should().BeOfType<ClientApplicationTypesSubSections>();
            var clientApplicationTypesSubSections = (ClientApplicationTypesSubSections) dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section;
            clientApplicationTypesSubSections.BrowserBasedSection.Should().NotBeNull();
            clientApplicationTypesSubSections.BrowserBasedSection.Status.Should().Be(result);
        }

        [Test]
        public async Task ShouldGetDashboardWithContacts()
        {
            var contactMock = new List<IContact>
            {
                Mock.Of<IContact>(c => c.Name == "Cool McRule")
            };
            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>(s =>
                s.Contacts == contactMock)).ConfigureAwait(false);
            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ContactDetailsSection.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ContactDetailsSection.Status.Should().Be("COMPLETE");
        }

        [Test]
        public async Task ShouldGetDashboardWithNoContacts()
        {
            var contactMock = new List<IContact>();
            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>(s =>
                s.Contacts == contactMock)).ConfigureAwait(false);
            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ContactDetailsSection.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ContactDetailsSection.Status.Should().Be("INCOMPLETE");
        }


        [Test]
        public async Task ShouldGetDashboardWithEmptyContacts()
        {
            var contactMock = new List<IContact>
            {
                Mock.Of<IContact>(c => c.Name == "" && c.Department == "            ")
            };
            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>(s =>
                s.Contacts == contactMock)).ConfigureAwait(false);
            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ContactDetailsSection.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ContactDetailsSection.Status.Should().Be("INCOMPLETE");
        }

        [TestCase(true, true, true, true, true, true, true, "COMPLETE")]
        [TestCase(true, true, true, true, true, false, true, "COMPLETE")]
        [TestCase(true, true, true, true, true, true, false, "COMPLETE")]
        [TestCase(true, true, true, true, true, false, false, "COMPLETE")]
        [TestCase(true, false, true, true, true, true, true, "COMPLETE")]
        [TestCase(false, true, true, true, true, true, true, "INCOMPLETE")]
        [TestCase(true, null, true, true, true, true, true, "INCOMPLETE")]
        [TestCase(true, true, false, true, true, true, true, "INCOMPLETE")]
        [TestCase(false, false, true, true, true, true, true, "INCOMPLETE")]
        [TestCase(false, false, false, false, false, false, false, "INCOMPLETE")]
        public async Task ShouldGetDashboardCalculateClientApplicationTypeNativeMobile(
            bool hasMobileOperatingSystems,
            bool? nativeMobileFirstDesign,
            bool hasMobileMemoryAndStorage,
            bool hasMobileConnectionDetails,
            bool hasHardwareRequirement,
            bool hasThirdParty,
            bool hasNativeMobileAdditionalInformation,
            string result)
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                   c.ClientApplicationTypes == new HashSet<string> { "native-mobile" }
                && c.MobileOperatingSystems == (hasMobileOperatingSystems ? Mock.Of<IMobileOperatingSystems>(m => m.OperatingSystems == new HashSet<string> { "Windows 10", "OSX" } && m.OperatingSystemsDescription == "Some OS") : null)
                && c.NativeMobileFirstDesign == nativeMobileFirstDesign
                && c.MobileMemoryAndStorage == (hasMobileMemoryAndStorage ? Mock.Of<IMobileMemoryAndStorage>(s => s.MinimumMemoryRequirement == "Min requirement" && s.Description == "Some memory description") : null)
                && c.MobileConnectionDetails == (hasMobileConnectionDetails ? Mock.Of<IMobileConnectionDetails>(c => c.ConnectionType == new HashSet<string> { "1GB" } && c.Description == "Some connection description" && c.MinimumConnectionSpeed == "1Mbps") : null)
                && c.NativeMobileHardwareRequirements == (hasHardwareRequirement ? "Some Hardware" : null)
                && c.MobileThirdParty == (hasThirdParty ? Mock.Of<IMobileThirdParty>(t => t.DeviceCapabilities == "Some device cap" && t.ThirdPartyComponents == "Some third party components") : null)
                && c.NativeMobileAdditionalInformation == (hasNativeMobileAdditionalInformation ? "NativeMobileAdditionalInformation" : null)))).ConfigureAwait(false);
            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section.Should().BeOfType<ClientApplicationTypesSubSections>();
            var clientApplicationTypesSubSections = (ClientApplicationTypesSubSections)dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section;
            clientApplicationTypesSubSections.NativeMobileSection.Should().NotBeNull();
            clientApplicationTypesSubSections.NativeMobileSection.Status.Should().Be(result);
        }

        [TestCase(true, true, true, true, true, true, "COMPLETE")]
        [TestCase(true, true, true, true, false, false, "COMPLETE")]
        [TestCase(true, true, true, false, false, false, "COMPLETE")]
        [TestCase(true, true, false, false, false, false, "INCOMPLETE")]
        [TestCase(true, false, true, false, false, false, "INCOMPLETE")]
        [TestCase(false, true, true, false, false, false, "INCOMPLETE")]
        [TestCase(true, true, false, false, false, false, "INCOMPLETE")]
        [TestCase(false, false, false, false, false, false, "INCOMPLETE")]
        public async Task ShouldGetDashboardToCalculateClientApplicationTypesNativeDesktop(
            bool hasOperatingSystem,
            bool hasConnectionDetails,
            bool hasMemoryAndStorage,
            bool hasThirdParty,
            bool hasHardwareRequirements,
            bool hasAdditionalInformation,
            string result)
        {

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> {"native-desktop"} &&
                    c.NativeDesktopOperatingSystemsDescription ==
                    (hasOperatingSystem ? "Operating System" : null) &&
                    c.NativeDesktopMinimumConnectionSpeed == (hasConnectionDetails ? "6Mbps" : null) &&
                    c.NativeDesktopMemoryAndStorage == (hasMemoryAndStorage
                        ? Mock.Of<INativeDesktopMemoryAndStorage>(m =>
                            m.MinimumMemoryRequirement == "500Mb" && m.StorageRequirementsDescription == "Desc" &&
                            m.MinimumCpu == "Min" && m.RecommendedResolution == "Res")
                        : null) &&
                    c.NativeDesktopThirdParty == (hasThirdParty
                        ? Mock.Of<INativeDesktopThirdParty>(t =>
                            t.ThirdPartyComponents == "Components" && t.DeviceCapabilities == "Capabilities")
                        : null) &&
                    c.NativeDesktopHardwareRequirements == (hasHardwareRequirements ? "A hardware requirement" : null) &&
                    c.NativeDesktopAdditionalInformation == (hasAdditionalInformation ? "Some additional info" : null)
                ))).ConfigureAwait(false);

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section.Should().BeOfType<ClientApplicationTypesSubSections>();
            var clientApplicationTypesSubSections = (ClientApplicationTypesSubSections)dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section;
            clientApplicationTypesSubSections.NativeDesktopSection.Should().NotBeNull();
            clientApplicationTypesSubSections.NativeDesktopSection.Status.Should().Be(result);
        }

        [TestCase(null, null, null, "INCOMPLETE")]
        [TestCase("     ", null, "  ", "INCOMPLETE")]
        [TestCase(null, "       ", " ", "INCOMPLETE")]
        [TestCase("Summary", null, null, "COMPLETE")]
        [TestCase(null, "url", null, "COMPLETE")]
        [TestCase(null, null, "connectivity", "COMPLETE")]
        [TestCase("Summary", "url", "connectivity", "COMPLETE")]
        public async Task ShouldGetDashboardToCalculateIfPublicCloudComplete(string summary, string link, string requiresHscn, string complete)
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>(s =>
                s.Hosting == Mock.Of<IHosting>(h => h.PublicCloud == Mock.Of<IPublicCloud>(p =>
                                                        p.Summary == summary && p.Link == link &&
                                                        p.RequiresHSCN == requiresHscn)))).ConfigureAwait(false);

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.HostingTypePublicCloudSection.Status.Should().Be(complete);
        }

        [TestCase(null, "url", null, null, "COMPLETE")]
        [TestCase(null, null, null, null, "INCOMPLETE")]
        [TestCase("Summary", null, null, null, "COMPLETE")]
        [TestCase(null, "url", "hosting", null, "COMPLETE")]
        [TestCase(null, null, "hosting", null, "COMPLETE")]
        [TestCase(null, "       ", "hosting", " ", "COMPLETE")]
        [TestCase(null, "       ", "     ", " ", "INCOMPLETE")]
        [TestCase("Summary", null, "hosting", null, "COMPLETE")]
        [TestCase(null, null, null, "connectivity", "COMPLETE")]
        [TestCase("     ", null, "hosting", "  ", "COMPLETE")]
        [TestCase(null, null, "hosting", "connectivity", "COMPLETE")]
        [TestCase("Summary", "url", null, "connectivity", "COMPLETE")]
        [TestCase("Summary", "url", "hosting", "connectivity", "COMPLETE")]
        public async Task ShouldGetDashboardToCalculateIfPrivateCloudComplete(string summary, string link, string hosting, string requiresHscn, string complete)
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>(s =>
                s.Hosting == Mock.Of<IHosting>(h => h.PrivateCloud == Mock.Of<IPrivateCloud>(p =>
                                                        p.Summary == summary && p.Link == link && p.HostingModel == hosting &&
                                                        p.RequiresHSCN == requiresHscn)))).ConfigureAwait(false);

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.HostingTypePrivateCloudSection.Status.Should().Be(complete);
        }

        [TestCase(null, "url", null, null, "COMPLETE")]
        [TestCase(null, null, null, null, "INCOMPLETE")]
        [TestCase("Summary", null, null, null, "COMPLETE")]
        [TestCase(null, "url", "hosting", null, "COMPLETE")]
        [TestCase(null, null, "hosting", null, "COMPLETE")]
        [TestCase(null, "       ", "hosting", " ", "COMPLETE")]
        [TestCase(null, "       ", "     ", " ", "INCOMPLETE")]
        [TestCase("Summary", null, "hosting", null, "COMPLETE")]
        [TestCase(null, null, null, "connectivity", "COMPLETE")]
        [TestCase("     ", null, "hosting", "  ", "COMPLETE")]
        [TestCase(null, null, "hosting", "connectivity", "COMPLETE")]
        [TestCase("Summary", "url", null, "connectivity", "COMPLETE")]
        [TestCase("Summary", "url", "hosting", "connectivity", "COMPLETE")]
        public async Task ShouldGetDashboardToCalculateIfOnPremiseComplete(string summary, string link, string hosting, string requiresHscn, string complete)
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>(s =>
                s.Hosting == Mock.Of<IHosting>(h => h.OnPremise == Mock.Of<IOnPremise>(p =>
                                                        p.Summary == summary && p.Link == link && p.HostingModel == hosting &&
                                                        p.RequiresHSCN == requiresHscn)))).ConfigureAwait(false);

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.HostingTypeOnPremiseSection.Status.Should().Be(complete);
        }

        private async Task<SolutionDashboardResult> GetSolutionDashboardSectionAsync(ISolution solution)
        {
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await _solutionsController.Dashboard(SolutionId).ConfigureAwait(false)).Result as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);

            return result.Value as SolutionDashboardResult;
        }
    }
}
