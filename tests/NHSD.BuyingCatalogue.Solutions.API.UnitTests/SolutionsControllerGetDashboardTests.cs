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
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hosting;
using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class SolutionsControllerGetDashboardTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private SolutionsController solutionsController;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            solutionsController = new SolutionsController(mockMediator.Object);
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            var result = (await solutionsController.Dashboard(SolutionId)).Result as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            result.Value.Should().BeOfType<SolutionDashboardResult>();
            result.Value.As<SolutionDashboardResult>().Id.Should().BeNull();
        }

        [Test]
        public void NullSolutionShouldThrowNullExceptionDashboardSection()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new SolutionDashboardSections(null));
        }

        [Test]
        public void NullSolutionShouldReturnEmptyDashboardResult()
        {
            var dashboard = new SolutionDashboardResult(null);
            dashboard.Id.Should().BeNull();
            dashboard.Name.Should().BeNull();
            dashboard.SolutionDashboardSections.Should().BeNull();
        }

        [TestCase(null, null, null)]
        [TestCase(null, "Bob", "Supplier A")]
        [TestCase("Sln2", null, null)]
        [TestCase("Sln2", "Bob", null)]
        [TestCase("Sln2", null, "Supplier A")]
        [TestCase("Sln2", "Bob", "Supplier A")]
        public async Task ShouldReturnNameIdSupplierName(string id, string name, string supplierName)
        {
            Expression<Func<ISolution, bool>> solution = s =>
                s.Id == id
                && s.Name == name
                && s.SupplierName == supplierName;

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));
            dashboardResult.Id.Should().Be(id);
            dashboardResult.Name.Should().Be(name);
            dashboardResult.SupplierName.Should().Be(supplierName);
        }

        [Test]
        public async Task ShouldReturnSolutionDashboardStaticData()
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>());
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
            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>());
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
        public async Task ShouldGetDashboardCalculateSolutionDescription(
            string summary,
            string description,
            string link,
            string result)
        {
            Expression<Func<ISolution, bool>> solution = s =>
                s.Summary == summary
                && s.Description == description
                && s.AboutUrl == link;

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));
            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.SolutionDescriptionSection.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.SolutionDescriptionSection.Status.Should().Be(result);
        }

        [TestCase(false, "INCOMPLETE")]
        [TestCase(true, "COMPLETE")]
        public async Task ShouldGetDashboardCalculateCompleteFeatures(bool isFeatures, string result)
        {
            var features = isFeatures ? new[] { "Feature1", "Feature2" } : Array.Empty<string>();

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of<ISolution>(s => s.Features == features));
            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.FeaturesSection.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.FeaturesSection.Status.Should().Be(result);
        }

        [Test]
        public async Task ShouldGetDashboardCalculateClientApplicationTypesBrowserBasedNull()
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "browser-based" };

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section
                .Should()
                .BeOfType<ClientApplicationTypesSubSections>();

            var clientApplicationTypesSubSections = (ClientApplicationTypesSubSections)dashboardResult
                .SolutionDashboardSections
                .ClientApplicationTypesSection
                .Section;

            clientApplicationTypesSubSections.BrowserBasedSection.Should().NotBeNull();
            clientApplicationTypesSubSections.BrowserBasedSection.Status.Should().Be("INCOMPLETE");
        }

        [Test]
        public async Task ShouldGetDashboardCalculateClientApplicationTypesNativeMobileNull()
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-mobile" };

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section
                .Should()
                .BeOfType<ClientApplicationTypesSubSections>();

            var clientApplicationTypesSubSections = (ClientApplicationTypesSubSections)dashboardResult
                .SolutionDashboardSections
                .ClientApplicationTypesSection
                .Section;

            clientApplicationTypesSubSections.NativeMobileSection.Should().NotBeNull();
            clientApplicationTypesSubSections.NativeMobileSection.Status.Should().Be("INCOMPLETE");
        }

        [Test]
        public async Task ShouldGetDashboardCalculateClientApplicationTypesNativeDesktopNull()
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-desktop" };

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section
                .Should()
                .BeOfType<ClientApplicationTypesSubSections>();

            var clientApplicationTypesSubSections = (ClientApplicationTypesSubSections)dashboardResult
                .SolutionDashboardSections
                .ClientApplicationTypesSection
                .Section;

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
            Expression<Func<IPlugins, bool>> plugins = p => p.Required == isPlugins;

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "browser-based" }
                && c.BrowsersSupported == (someBrowsersSupported ? new HashSet<string> { "Edge", "Chrome" } : new HashSet<string>())
                && c.MobileResponsive == mobileResponsive
                && c.MobileFirstDesign == isMobileFirst
                && c.Plugins == (isPlugins ? Mock.Of(plugins) : null)
                && c.MinimumConnectionSpeed == (isConnectivity ? "Some Connectivity" : null);

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section
                .Should()
                .BeOfType<ClientApplicationTypesSubSections>();

            var clientApplicationTypesSubSections = (ClientApplicationTypesSubSections)dashboardResult.
                SolutionDashboardSections
                .ClientApplicationTypesSection
                .Section;

            clientApplicationTypesSubSections.BrowserBasedSection.Should().NotBeNull();
            clientApplicationTypesSubSections.BrowserBasedSection.Status.Should().Be(result);
        }

        [Test]
        public async Task ShouldGetDashboardWithContacts()
        {
            Expression<Func<IContact, bool>> contact = c => c.Name == "Cool McRule";

            var contactMock = new List<IContact>
            {
                Mock.Of(contact),
            };

            // ReSharper disable once PossibleUnintendedReferenceComparison (mock set-up)
            Expression<Func<ISolution, bool>> solution = s => s.Contacts == contactMock;

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));
            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ContactDetailsSection.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ContactDetailsSection.Status.Should().Be("COMPLETE");
        }

        [Test]
        public async Task ShouldGetDashboardWithNoContacts()
        {
            var contactMock = new List<IContact>();

            // ReSharper disable once PossibleUnintendedReferenceComparison (mock set-up)
            Expression<Func<ISolution, bool>> solution = s => s.Contacts == contactMock;

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));
            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ContactDetailsSection.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ContactDetailsSection.Status.Should().Be("INCOMPLETE");
        }

        [Test]
        public async Task ShouldGetDashboardWithEmptyContacts()
        {
#pragma warning disable CA1820 // Test for empty strings using string length (mock set-up)
            Expression<Func<IContact, bool>> contact = c =>
                c.Name == string.Empty
                && c.Department == "            ";
#pragma warning restore CA1820 // Test for empty strings using string length

            var contactMock = new List<IContact>
            {
                Mock.Of(contact),
            };

            // ReSharper disable once PossibleUnintendedReferenceComparison (mock set-up)
            Expression<Func<ISolution, bool>> solution = s => s.Contacts == contactMock;

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));
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
            Expression<Func<IMobileOperatingSystems, bool>> mobileOperatingSystems = m =>
                m.OperatingSystems == new HashSet<string> { "Windows 10", "OSX" }
                && m.OperatingSystemsDescription == "Some OS";

            Expression<Func<IMobileMemoryAndStorage, bool>> mobileMemoryAndStorage = s =>
                s.MinimumMemoryRequirement == "Min requirement"
                && s.Description == "Some memory description";

            Expression<Func<IMobileConnectionDetails, bool>> mobileConnectionDetails = c =>
                c.ConnectionType == new HashSet<string> { "1GB" }
                && c.Description == "Some connection description"
                && c.MinimumConnectionSpeed == "1Mbps";

            Expression<Func<IMobileThirdParty, bool>> mobileThirdParty = t =>
                t.DeviceCapabilities == "Some device cap"
                && t.ThirdPartyComponents == "Some third party components";

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-mobile" }
                && c.MobileOperatingSystems == (hasMobileOperatingSystems ? Mock.Of(mobileOperatingSystems) : null)
                && c.NativeMobileFirstDesign == nativeMobileFirstDesign
                && c.MobileMemoryAndStorage == (hasMobileMemoryAndStorage ? Mock.Of(mobileMemoryAndStorage) : null)
                && c.MobileConnectionDetails == (hasMobileConnectionDetails ? Mock.Of(mobileConnectionDetails) : null)
                && c.NativeMobileHardwareRequirements == (hasHardwareRequirement ? "Some Hardware" : null)
                && c.MobileThirdParty == (hasThirdParty ? Mock.Of(mobileThirdParty) : null)
                && c.NativeMobileAdditionalInformation == (hasNativeMobileAdditionalInformation ? "NativeMobileAdditionalInformation" : null);

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section
                .Should()
                .BeOfType<ClientApplicationTypesSubSections>();

            var clientApplicationTypesSubSections = (ClientApplicationTypesSubSections)dashboardResult
                .SolutionDashboardSections
                .ClientApplicationTypesSection
                .Section;

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
            Expression<Func<INativeDesktopMemoryAndStorage, bool>> nativeDesktopMemoryAndStorage = d =>
                d.MinimumMemoryRequirement == "500Mb"
                && d.StorageRequirementsDescription == "Desc"
                && d.MinimumCpu == "Min"
                && d.RecommendedResolution == "Res";

            Expression<Func<INativeDesktopThirdParty, bool>> nativeDesktopThirdParty = t =>
                t.ThirdPartyComponents == "Components"
                && t.DeviceCapabilities == "Capabilities";

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-desktop" }
                && c.NativeDesktopOperatingSystemsDescription == (hasOperatingSystem ? "Operating System" : null)
                && c.NativeDesktopMinimumConnectionSpeed == (hasConnectionDetails ? "6Mbps" : null)
                && c.NativeDesktopMemoryAndStorage == (hasMemoryAndStorage ? Mock.Of(nativeDesktopMemoryAndStorage) : null)
                && c.NativeDesktopThirdParty == (hasThirdParty ? Mock.Of(nativeDesktopThirdParty) : null)
                && c.NativeDesktopHardwareRequirements == (hasHardwareRequirements ? "A hardware requirement" : null)
                && c.NativeDesktopAdditionalInformation == (hasAdditionalInformation ? "Some additional info" : null);

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section
                .Should()
                .BeOfType<ClientApplicationTypesSubSections>();

            var clientApplicationTypesSubSections = (ClientApplicationTypesSubSections)dashboardResult
                .SolutionDashboardSections
                .ClientApplicationTypesSection
                .Section;

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
        public async Task ShouldGetDashboardToCalculateIfPublicCloudComplete(
            string summary,
            string link,
            string requiresHscn,
            string complete)
        {
            Expression<Func<IPublicCloud, bool>> publicCloud = p =>
                p.Summary == summary
                && p.Link == link
                && p.RequiresHscn == requiresHscn;

            Expression<Func<IHosting, bool>> hosting = h => h.PublicCloud == Mock.Of(publicCloud);
            Expression<Func<ISolution, bool>> solution = s => s.Hosting == Mock.Of(hosting);

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));

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
        public async Task ShouldGetDashboardToCalculateIfPrivateCloudComplete(
            string summary,
            string link,
            string hosting,
            string requiresHscn,
            string complete)
        {
            Expression<Func<IPrivateCloud, bool>> privateCloud = p =>
                p.Summary == summary
                && p.Link == link
                && p.HostingModel == hosting
                && p.RequiresHscn == requiresHscn;

            Expression<Func<IHosting, bool>> hostingExpression = h => h.PrivateCloud == Mock.Of(privateCloud);
            Expression<Func<ISolution, bool>> solution = s => s.Hosting == Mock.Of(hostingExpression);

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));

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
        public async Task ShouldGetDashboardToCalculateIfOnPremiseComplete(
            string summary,
            string link,
            string hosting,
            string requiresHscn,
            string complete)
        {
            Expression<Func<IOnPremise, bool>> onPremise = p =>
                p.Summary == summary
                && p.Link == link
                && p.HostingModel == hosting
                && p.RequiresHscn == requiresHscn;

            Expression<Func<IHosting, bool>> hostingExpression = h => h.OnPremise == Mock.Of(onPremise);
            Expression<Func<ISolution, bool>> solution = s => s.Hosting == Mock.Of(hostingExpression);

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.HostingTypeOnPremiseSection.Status.Should().Be(complete);
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
        public async Task ShouldGetDashboardToCalculateIfHybridComplete(
            string summary,
            string link,
            string hosting,
            string requiresHscn,
            string complete)
        {
            Expression<Func<IHybridHostingType, bool>> hybridHostingType = p =>
                p.Summary == summary
                && p.Link == link
                && p.HostingModel == hosting
                && p.RequiresHscn == requiresHscn;

            Expression<Func<IHosting, bool>> hostingExpression = h => h.HybridHostingType == Mock.Of(hybridHostingType);
            Expression<Func<ISolution, bool>> solution = s => s.Hosting == Mock.Of(hostingExpression);

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.HostingTypeHybridSection.Status.Should().Be(complete);
        }

        [TestCase("", "INCOMPLETE")]
        [TestCase("   ", "INCOMPLETE")]
        [TestCase(null, "INCOMPLETE")]
        [TestCase("roadMap", "COMPLETE")]
        public async Task ShouldGetDashboardCalculateCompleteRoadMap(string roadMap, string result)
        {
            Expression<Func<ISolution, bool>> solution = s => s.RoadMap.Summary == roadMap;

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));
            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.RoadMapSection.Status.Should().Be(result);
        }

        [TestCase("", "INCOMPLETE")]
        [TestCase("   ", "INCOMPLETE")]
        [TestCase(null, "INCOMPLETE")]
        [TestCase("integrations url", "COMPLETE")]
        public async Task ShouldGetDashboardCalculateCompleteIntegrations(string integrationsUrl, string result)
        {
            Expression<Func<ISolution, bool>> solution = s => s.Integrations.Url == integrationsUrl;

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));
            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.IntegrationsSection.Status.Should().Be(result);
        }

        [TestCase("", "INCOMPLETE")]
        [TestCase("   ", "INCOMPLETE")]
        [TestCase(null, "INCOMPLETE")]
        [TestCase("implementation timescales description", "COMPLETE")]
        public async Task ShouldGetDashboardCalculateCompleteImplementationTimescales(
            string implementationTimescales,
            string result)
        {
            Expression<Func<ISolution, bool>> solution = s => s.ImplementationTimescales.Description == implementationTimescales;

            var dashboardResult = await GetSolutionDashboardSectionAsync(Mock.Of(solution));
            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ImplementationTimescalesSection.Status.Should().Be(result);
        }

        private async Task<SolutionDashboardResult> GetSolutionDashboardSectionAsync(ISolution solution)
        {
            mockMediator
                .Setup(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await solutionsController.Dashboard(SolutionId)).Result as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()));

            return result.Value as SolutionDashboardResult;
        }
    }
}
