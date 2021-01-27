using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Capabilities;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hosting;
using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class SolutionsControllerGetPublicTests
    {
        private const string SolutionId1 = "Sln1";
        private const string SolutionId2 = "Sln2";

        private readonly DateTime lastUpdated = DateTime.Today;

        private Mock<IMediator> mockMediator;
        private SolutionsController solutionsController;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            solutionsController = new SolutionsController(mockMediator.Object);
        }

        [TestCase(null, null, null, false)]
        [TestCase("Sln2", null, null, false)]
        [TestCase(null, "Bob", "Supplier1", false)]
        [TestCase("Sln1", "Bob", "Supplier1", false)]
        [TestCase("Sln1", null, "Supplier1", false)]
        [TestCase("Sln1", "Bob", "Supplier1", false)]
        [TestCase("Sln1", "Bob", "Supplier1", true)]
        public async Task ShouldReturnGetValues(string id, string name, string supplierName, bool isFoundation)
        {
            Expression<Func<ISolution, bool>> solution = s =>
                s.Id == id
                && s.Name == name
                && s.SupplierName == supplierName
                && s.IsFoundation == isFoundation
                && s.LastUpdated == lastUpdated
                && s.PublishedStatus == PublishedStatus.Published;

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            publicResult.Id.Should().Be(id);
            publicResult.Name.Should().Be(name);
            publicResult.SupplierName.Should().Be(supplierName);
            publicResult.IsFoundation.Should().Be(isFoundation);
            publicResult.LastUpdated.Should().Be(lastUpdated);
        }

        [TestCase(null, null, null)]
        [TestCase("summary", null, null)]
        [TestCase(null, "description", null)]
        [TestCase(null, null, "link")]
        [TestCase("summary", "description", null)]
        [TestCase("summary", null, "link")]
        [TestCase(null, "description", "link")]
        [TestCase("summary", "description", "link")]
        public async Task ShouldGetSolutionDescriptionForSolution(string summary, string description, string link)
        {
            Expression<Func<ISolution, bool>> solution = s =>
                s.Id == SolutionId1
                && s.Summary == summary
                && s.Description == description
                && s.AboutUrl == link
                && s.PublishedStatus == PublishedStatus.Published;

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            publicResult.Id.Should().Be(SolutionId1);

            if (summary == null && description == null && link == null)
            {
                publicResult.Sections.SolutionDescription.Should().BeNull();
            }
            else
            {
                publicResult.Sections.SolutionDescription.Answers.Summary.Should().Be(summary);
                publicResult.Sections.SolutionDescription.Answers.Description.Should().Be(description);
                publicResult.Sections.SolutionDescription.Answers.Link.Should().Be(link);
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldGetFeaturesForSolution(bool hasFeature)
        {
            var feature = hasFeature ? new List<string> { "feature1", "feature2" } : new List<string>();

            // ReSharper disable once PossibleUnintendedReferenceComparison (mock set-up)
            Expression<Func<ISolution, bool>> solution = s =>
                s.Id == SolutionId1
                && s.Features == feature
                && s.PublishedStatus == PublishedStatus.Published;

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            publicResult.Id.Should().Be(SolutionId1);

            if (hasFeature)
            {
                publicResult.Sections.Features.Answers.Listing.Should().BeEquivalentTo(
                    new List<string> { "feature1", "feature2" });
            }
            else
            {
                publicResult.Sections.Features.Should().BeNull();
            }
        }

        [TestCase(null, null, false)]
        [TestCase("some description", null, true)]
        [TestCase(" some description    ", null, true)]
        [TestCase(" ", null, false)]
        [TestCase("", null, false)]
        [TestCase("some description", "roadMap.pdf", true)]
        [TestCase(null, "roadMap.pdf", true)]
        public async Task ShouldGetPublicCalculateRoadMap(string description, string documentName, bool hasData)
        {
            Expression<Func<ISolution, bool>> solution = s =>
                s.Id == SolutionId1
                && s.PublishedStatus == PublishedStatus.Published
                && s.RoadMap.Summary == description
                && s.RoadMap.DocumentName == documentName;

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            publicResult.Id.Should().Be(SolutionId1);

            if (hasData)
            {
                publicResult.Sections.RoadMap.Should().NotBe(null);
                publicResult.Sections.RoadMap.Answers.HasData.Should().Be(true);
                publicResult.Sections.RoadMap.Answers.Summary.Should().Be(description);
                publicResult.Sections.RoadMap.Answers.DocumentName.Should().Be(documentName);
            }
            else
            {
                publicResult.Sections.RoadMap.Should().BeNull();
            }
        }

        [TestCase(null, null, false)]
        [TestCase("some integrations url", null, true)]
        [TestCase(" some integrations url    ", null, true)]
        [TestCase(" ", null, false)]
        [TestCase("", null, false)]
        [TestCase("some integrations url", "integration.pdf", true)]
        [TestCase(null, "integration.pdf", true)]
        public async Task ShouldGetPublicCalculateIntegrations(string url, string documentName, bool hasData)
        {
            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.Integrations == Mock.Of<IIntegrations>(i => i.Url == url && i.DocumentName == documentName);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            if (hasData)
            {
                publicResult.Sections.Integrations.Should().NotBe(null);
                publicResult.Sections.Integrations.Answers.HasData.Should().Be(true);
                publicResult.Sections.Integrations.Answers.IntegrationsUrl.Should().Be(url);
                publicResult.Sections.Integrations.Answers.DocumentName.Should().Be(documentName);
            }
            else
            {
                publicResult.Sections.Integrations.Should().BeNull();
            }
        }

        [TestCase(null, false)]
        [TestCase("some implementation timescales description", true)]
        [TestCase(" some implementation timescales description    ", true)]
        [TestCase(" ", false)]
        [TestCase("", false)]
        public async Task ShouldGetPublicCalculateImplementationTimescales(string description, bool hasData)
        {
            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.ImplementationTimescales == Mock.Of<IImplementationTimescales>(i => i.Description == description);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            if (hasData)
            {
                publicResult.Sections.ImplementationTimescales.Should().NotBe(null);
                publicResult.Sections.ImplementationTimescales.Answers.HasData.Should().Be(true);
                publicResult.Sections.ImplementationTimescales.Answers.Description.Should().Be(description);
            }
            else
            {
                publicResult.Sections.ImplementationTimescales.Should().BeNull();
            }
        }

        [TestCase(false, false, null, false)]
        [TestCase(true, false, null, false)]
        [TestCase(true, true, null, true)]
        [TestCase(true, false, false, true)]
        [TestCase(true, false, true, true)]
        [TestCase(true, true, false, true)]
        [TestCase(true, true, true, true)]
        public async Task ShouldGetPublicCalculateClientApplication(
            bool isClientApplication,
            bool isBrowserSupported,
            bool? mobileResponsive,
            bool expectData)
        {
            var clientApplicationTypes = isClientApplication
                ? new HashSet<string> { "browser-based", "native-mobile" }
                : new HashSet<string>();

            var browsersSupported = isBrowserSupported ? new HashSet<string> { "Chrome", "Edge" } : new HashSet<string>();

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == clientApplicationTypes
                && c.BrowsersSupported == browsersSupported
                && c.MobileResponsive == mobileResponsive;

            Expression<Func<ISolution, bool>> solution = s =>
                s.ClientApplication == Mock.Of(clientApplication)
                && s.PublishedStatus == PublishedStatus.Published;

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            if (expectData)
            {
                publicResult.Sections.ClientApplicationTypes.Sections.HasData.Should().Be(true);

                if (mobileResponsive.HasValue)
                {
                    publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported
                        .Answers.MobileResponsive
                        .Should()
                        .Be(mobileResponsive.GetValueOrDefault() ? "Yes" : "No");
                }
                else
                {
                    publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported
                        .Answers
                        .MobileResponsive.Should().BeNull();
                }

                if (isBrowserSupported)
                {
                    publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported
                        .Answers.SupportedBrowsers
                        .Should().BeEquivalentTo(isClientApplication
                            ? new HashSet<string> { "Chrome", "Edge" }
                            : new HashSet<string>());
                }
                else
                {
                    publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported
                        .Answers
                        .SupportedBrowsers.Should().BeNull();
                }
            }
            else
            {
                publicResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldGetCapabilitiesOnlyForSolution(bool hasCapability)
        {
            var capabilityData = GetClaimedCapabilityTestData().Take(2).ToArray();

            var capabilities = hasCapability ? capabilityData.Select(c => c.Capability) : Array.Empty<IClaimedCapability>();

            // ReSharper disable once PossibleUnintendedReferenceComparison (mock set-up)
            Expression<Func<ISolution, bool>> solution = s =>
                s.Id == SolutionId1
                && s.Capabilities == capabilities
                && s.PublishedStatus == PublishedStatus.Published;

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            publicResult.Id.Should().Be(SolutionId1);
            if (hasCapability)
            {
                publicResult.Sections.Capabilities.Answers.CapabilitiesMet.Should().BeEquivalentTo(
                    capabilityData.Select(c => c.Section));
            }
            else
            {
                publicResult.Sections.Capabilities.Should().BeNull();
            }
        }

        [TestCase("1GBps", "1x1", true)]
        [TestCase(null, "1x1", true)]
        [TestCase("1GBps", null, true)]
        [TestCase(null, null, false)]
        [TestCase("    ", "    ", false)]
        [TestCase("", "", false)]
        [TestCase("	", "	", false)]
        public async Task ConnectivityAndResolutionSectionIsSetCorrectly(
            string connectivity,
            string resolution,
            bool hasData)
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "browser-based" }
                && c.MinimumConnectionSpeed == connectivity
                && c.MinimumDesktopResolution == resolution;

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.ClientApplication == Mock.Of(clientApplication);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);
            var connectivitySection = publicResult?.Sections?.ClientApplicationTypes?.Sections?.BrowserBased?.Sections?
                .BrowserConnectivityAndResolutionSection;

            if (!hasData)
            {
                connectivitySection.Should().BeNull();
                return;
            }

            Assert.NotNull(connectivitySection);
            connectivitySection.Answers.HasData.Should().BeTrue();
            connectivitySection.Answers.MinimumConnectionSpeed.Should().Be(connectivity);
            connectivitySection.Answers.MinimumDesktopResolution.Should().Be(resolution);
        }

        [TestCase(false, null, false)]
        [TestCase(false, "Desc", false)]
        [TestCase(true, null, true)]
        [TestCase(true, "Desc", true)]
        public async Task MobileOperatingSystemsIsSetCorrectly(bool isOperatingSystem, string description, bool hasData)
        {
            var operatingSystem = isOperatingSystem ? new HashSet<string> { "IOS", "Windows" } : new HashSet<string>();

            Expression<Func<IMobileOperatingSystems, bool>> mobileOperatingSystems = m =>
                m.OperatingSystems == operatingSystem
                && m.OperatingSystemsDescription == description;

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-mobile" }
                && c.MobileOperatingSystems == Mock.Of(mobileOperatingSystems);

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.ClientApplication == Mock.Of(clientApplication);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1)
                ;

            var operatingSystemResult = publicResult?.Sections?.ClientApplicationTypes?.Sections?.NativeMobile
                ?.Sections?.MobileOperatingSystemsSection;

            if (!hasData)
            {
                operatingSystemResult.Should().BeNull();
                return;
            }

            Assert.NotNull(operatingSystemResult);
            operatingSystemResult.Answers.HasData.Should().BeTrue();
            operatingSystemResult.Answers.OperatingSystemsDescription.Should().Be(description);
            operatingSystemResult.Answers.OperatingSystems.Should().BeEquivalentTo(operatingSystem);
        }

        [TestCase(false, false, false, false)]
        [TestCase(true, true, true, true)]
        [TestCase(false, true, true, true)]
        [TestCase(true, false, true, true)]
        [TestCase(true, true, false, true)]
        [TestCase(false, false, true, true)]
        [TestCase(false, true, false, true)]
        [TestCase(true, false, false, true)]
        public async Task MobileConnectionDetailsIsSetCorrectly(
            bool hasConnectionType,
            bool hasDescription,
            bool hasMinimumConnectionSpeed,
            bool hasData)
        {
            var connectionType = hasConnectionType ? new HashSet<string> { "3G", "4G" } : null;
            var description = hasDescription ? "I am a description" : null;
            var minimumConnectionSpeed = hasMinimumConnectionSpeed ? "1GBps" : null;

            Expression<Func<IMobileConnectionDetails, bool>> mobileConnectionDetails = m =>
                m.ConnectionType == connectionType
                && m.Description == description
                && m.MinimumConnectionSpeed == minimumConnectionSpeed;

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-mobile" }
                && c.MobileConnectionDetails == Mock.Of(mobileConnectionDetails);

            Expression<Func<ISolution, bool>> solution = s =>
                s.Id == SolutionId1
                && s.PublishedStatus == PublishedStatus.Published
                && s.ClientApplication == Mock.Of(clientApplication);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            var mobileConnectionDetailsResult = publicResult?.Sections?.ClientApplicationTypes?.Sections?.NativeMobile
                ?.Sections?.MobileConnectionDetailsSection;

            if (!hasData)
            {
                mobileConnectionDetailsResult.Should().BeNull();
                return;
            }

            Assert.NotNull(mobileConnectionDetailsResult);
            mobileConnectionDetailsResult.Answers.HasData.Should().BeTrue();
            mobileConnectionDetailsResult.Answers.Description.Should().Be(description);
            mobileConnectionDetailsResult.Answers.MinimumConnectionSpeed.Should().Be(minimumConnectionSpeed);
            mobileConnectionDetailsResult.Answers.ConnectionType.Should().BeEquivalentTo(connectionType);
        }

        [TestCase(null, null, false)]
        [TestCase("1GB", null, false)]
        [TestCase(null, "Desc", false)]
        [TestCase("1GB", "Desc", true)]
        public async Task MobileMemoryAndStorageIsSetCorrectly(
            string minMemoryManagement,
            string description,
            bool hasData)
        {
            Expression<Func<IMobileMemoryAndStorage, bool>> mobileMemoryAndStorage = m =>
                m.MinimumMemoryRequirement == minMemoryManagement
                && m.Description == description;

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-mobile" }
                && c.MobileMemoryAndStorage == Mock.Of(mobileMemoryAndStorage);

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.ClientApplication == Mock.Of(clientApplication);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            var mobileMemoryStorageResult = publicResult?.Sections?.ClientApplicationTypes?.Sections?.NativeMobile
                ?.Sections?.MobileMemoryAndStorageSection;

            if (!hasData)
            {
                mobileMemoryStorageResult.Should().BeNull();
                return;
            }

            Assert.NotNull(mobileMemoryStorageResult);
            mobileMemoryStorageResult.Answers.HasData.Should().BeTrue();
            mobileMemoryStorageResult.Answers.MinimumMemoryRequirement.Should().Be(minMemoryManagement);
            mobileMemoryStorageResult.Answers.Description.Should().Be(description);
        }

        [TestCase(null, null)]
        [TestCase(false, "No")]
        [TestCase(true, "Yes")]
        public async Task BrowserMobileFirstIsSetCorrectly(bool? mobileFirst, string result)
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "browser-based" }
                && c.MobileFirstDesign == mobileFirst;

            Expression<Func<ISolution, bool>> solution = s =>
                s.Id == SolutionId1
                && s.ClientApplication == Mock.Of(clientApplication)
                && s.PublishedStatus == PublishedStatus.Published;

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            var mobileFirstSection = publicResult?.Sections?.ClientApplicationTypes?.Sections?.BrowserBased?.Sections
                ?.BrowserMobileFirstSection;

            if (result == null)
            {
                mobileFirstSection.Should().BeNull();
                return;
            }

            Assert.NotNull(mobileFirstSection);
            mobileFirstSection.Answers.MobileFirstDesign.Should().Be(result);
        }

        [TestCase("New hardware", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("      ", false)]
        public async Task IfNativeMobileThenEmptyHardwareRequirementsHasNoData(string requirements, bool hasData)
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-mobile" }
                && c.NativeMobileHardwareRequirements == requirements;

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.ClientApplication == Mock.Of(clientApplication);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            if (hasData)
            {
                publicResult.Sections.ClientApplicationTypes.Sections.NativeMobile.Sections
                    .HardwareRequirementsSection.Answers.HardwareRequirements.Should().Be(requirements);

                publicResult.Sections.ClientApplicationTypes.Sections.NativeMobile.Sections
                    .HardwareRequirementsSection.Answers.HasData.Should().BeTrue();
            }
            else
            {
                publicResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [TestCase(null, null, false)]
        [TestCase(" ", "     ", false)]
        [TestCase("Component", null, true)]
        [TestCase(null, "Capability", true)]
        [TestCase("Component", "Capability", true)]
        public async Task IfNativeMobileThenEmptyMobileThirdPartyHasNoData(
            string component,
            string capability,
            bool hasData)
        {
            Expression<Func<IMobileThirdParty, bool>> mobileThirdParty = m =>
                m.ThirdPartyComponents == component
                && m.DeviceCapabilities == capability;

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-mobile" }
                && c.MobileThirdParty == Mock.Of(mobileThirdParty);

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.ClientApplication == Mock.Of(clientApplication);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            if (hasData)
            {
                publicResult.Sections.ClientApplicationTypes.Sections.NativeMobile.Sections.MobileThirdPartySection
                    .Answers.ThirdPartyComponents.Should().Be(component);

                publicResult.Sections.ClientApplicationTypes.Sections.NativeMobile.Sections.MobileThirdPartySection
                    .Answers.DeviceCapabilities.Should().Be(capability);

                publicResult.Sections.ClientApplicationTypes.Sections.NativeMobile.Sections.MobileThirdPartySection
                    .Answers.HasData.Should().BeTrue();
            }
            else
            {
                publicResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [TestCase("New info", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("      ", false)]
        public async Task IfNativeMobileThenEmptyAdditionalInformationHasNoData(
            string additionalInformation,
            bool hasData)
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-mobile" }
                && c.NativeMobileAdditionalInformation == additionalInformation;

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.ClientApplication == Mock.Of(clientApplication);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            if (hasData)
            {
                publicResult.Sections.ClientApplicationTypes.Sections.NativeMobile.Sections
                    .MobileAdditionalInformationSection.Answers.NativeMobileAdditionalInformation.Should()
                    .Be(additionalInformation);

                publicResult.Sections.ClientApplicationTypes.Sections.NativeMobile.Sections
                    .MobileAdditionalInformationSection.Answers.HasData.Should().BeTrue();
            }
            else
            {
                publicResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [TestCase("New description", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("      ", false)]
        public async Task IfNativeDesktopEmptyThenOperatingSystemsDescriptionHasNoData(string description, bool hasData)
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-desktop" }
                && c.NativeDesktopOperatingSystemsDescription == description;

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.ClientApplication == Mock.Of(clientApplication);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            if (hasData)
            {
                publicResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .OperatingSystemsSection.Answers.OperatingSystemsDescription.Should().Be(description);

                publicResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .OperatingSystemsSection.Answers.HasData.Should().BeTrue();
            }
            else
            {
                publicResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [TestCase("New hardware", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("      ", false)]
        public async Task IfNativeDesktopThenNativeHardwareRequirementsIsSetCorrectly(string requirements, bool hasData)
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-desktop" }
                && c.NativeDesktopHardwareRequirements == requirements;

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.ClientApplication == Mock.Of(clientApplication);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            if (hasData)
            {
                publicResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .HardwareRequirementsSection.Answers.HardwareRequirements.Should().Be(requirements);

                publicResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .HardwareRequirementsSection.Answers.HasData.Should().BeTrue();
            }
            else
            {
                publicResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [TestCase("3 Mbps", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("      ", false)]
        public async Task IfNativeDesktopThenNativeConnectivityDetailsIsSetCorrectly(
            string minimumConnectionSpeed,
            bool hasData)
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-desktop" }
                && c.NativeDesktopMinimumConnectionSpeed == minimumConnectionSpeed;

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.ClientApplication == Mock.Of(clientApplication);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            if (hasData)
            {
                publicResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .NativeDesktopConnectivityDetailsSection.Answers.NativeDesktopMinimumConnectionSpeed.Should()
                    .Be(minimumConnectionSpeed);

                publicResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .NativeDesktopConnectivityDetailsSection.Answers.HasData.Should().BeTrue();
            }
            else
            {
                publicResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [TestCase(null, null, false)]
        [TestCase("", "     ", false)]
        [TestCase("     ", "", false)]
        [TestCase("     ", "        ", false)]
        [TestCase("Component", null, true)]
        [TestCase(null, "Capability", true)]
        [TestCase("Component", "Capability", true)]
        public async Task NativeDesktopThirdPartySectionIsPopulatedCorrectly(
            string component,
            string capability,
            bool hasData)
        {
            Expression<Func<INativeDesktopThirdParty, bool>> nativeDesktopThirdParty = d =>
                d.ThirdPartyComponents == component
                && d.DeviceCapabilities == capability;

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-desktop" }
                && c.NativeDesktopThirdParty == Mock.Of(nativeDesktopThirdParty);

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.ClientApplication == Mock.Of(clientApplication);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            if (hasData)
            {
                publicResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .NativeDesktopThirdPartySection
                    .Answers.ThirdPartyComponents.Should().Be(component);

                publicResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .NativeDesktopThirdPartySection
                    .Answers.DeviceCapabilities.Should().Be(capability);

                publicResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .NativeDesktopThirdPartySection
                    .Answers.HasData.Should().BeTrue();
            }
            else
            {
                publicResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [TestCase("512TB", "1024TB", "1Hz", "1x1 px", true)]
        [TestCase(null, "1024TB", "1Hz", "1x1 px", true)]
        [TestCase("512TB", null, "1Hz", "1x1 px", true)]
        [TestCase("512TB", "1024TB", null, "1x1 px", true)]
        [TestCase("512TB", "1024TB", "1Hz", null, true)]
        [TestCase(null, "1024TB", "1Hz", null, true)]
        [TestCase("512TB", null, null, "1x1 px", true)]
        [TestCase("512TB", null, null, "1x1 px", true)]
        [TestCase(null, "1024TB", "1Hz", null, true)]
        [TestCase(null, "1024TB", null, null, true)]
        [TestCase(null, null, null, "1x1 px", true)]
        [TestCase("512TB", null, null, null, true)]
        [TestCase(null, null, "1Hz", null, true)]
        [TestCase(null, null, null, null, false)]
        public async Task NativeDesktopMemoryAndStorageSectionIsPopulatedCorrectly(
            string memory,
            string storage,
            string cpu,
            string resolution,
            bool hasData)
        {
            Expression<Func<INativeDesktopMemoryAndStorage, bool>> nativeDesktopMemoryAndStorage = d =>
                d.MinimumMemoryRequirement == memory
                && d.StorageRequirementsDescription == storage
                && d.MinimumCpu == cpu
                && d.RecommendedResolution == resolution;

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-desktop" }
                && c.NativeDesktopMemoryAndStorage == Mock.Of(nativeDesktopMemoryAndStorage);

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.ClientApplication == Mock.Of(clientApplication);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            if (hasData)
            {
                publicResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .NativeDesktopMemoryAndStorageSection
                    .Answers.MinimumMemoryRequirement.Should().Be(memory);

                publicResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .NativeDesktopMemoryAndStorageSection
                    .Answers.StorageRequirementsDescription.Should().Be(storage);

                publicResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .NativeDesktopMemoryAndStorageSection
                    .Answers.MinimumCpu.Should().Be(cpu);

                publicResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .NativeDesktopMemoryAndStorageSection
                    .Answers.RecommendedResolution.Should().Be(resolution);

                publicResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .NativeDesktopMemoryAndStorageSection
                    .Answers.HasData.Should().BeTrue();
            }
            else
            {
                publicResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [TestCase("New info", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("      ", false)]
        public async Task IfNativeDesktopEmptyThenAdditionalInformationHasNoData(
            string additionalInformation,
            bool hasData)
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-desktop" }
                && c.NativeDesktopAdditionalInformation == additionalInformation;

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.ClientApplication == Mock.Of(clientApplication);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            if (hasData)
            {
                publicResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .NativeDesktopAdditionalInformationSection.Answers.NativeDesktopAdditionalInformation.Should()
                    .Be(additionalInformation);

                publicResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .NativeDesktopAdditionalInformationSection.Answers.HasData.Should().BeTrue();
            }
            else
            {
                publicResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [TestCase("Some Summary", "Some url", "Some Connectivity", true)]
        [TestCase(" ", "Some url", "Some Connectivity", true)]
        [TestCase("Some Summary", "", "Some Connectivity", true)]
        [TestCase("Some Summary", "Some url", "     ", true)]
        [TestCase("     ", "Some url", null, true)]
        [TestCase("Some Summary", "     ", "     ", true)]
        [TestCase("     ", "     ", "Some Connectivity", true)]
        [TestCase(null, null, "Some Connectivity", true)]
        [TestCase("     ", "", "        ", false)]
        [TestCase(null, null, null, false)]
        public async Task IfPublicCloudEmptyThenItHasNoData(
            string summary,
            string url,
            string requiresHscn,
            bool hasData)
        {
            Expression<Func<IPublicCloud, bool>> publicCloud = p =>
                p.Summary == summary
                && p.Link == url
                && p.RequiresHSCN == requiresHscn;

            Expression<Func<IHosting, bool>> hosting = h =>
                h.PublicCloud == Mock.Of(publicCloud);

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.Hosting == Mock.Of(hosting);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            if (hasData)
            {
                publicResult.Sections.PublicCloud.Answers.Summary.Should().Be(summary);
                publicResult.Sections.PublicCloud.Answers.Link.Should().Be(url);
                publicResult.Sections.PublicCloud.Answers.RequiresHSCN.Should().Be(requiresHscn);

                publicResult.Sections.PublicCloud.Answers.HasData.Should().BeTrue();
            }
            else
            {
                publicResult.Sections.PublicCloud.Should().BeNull();
            }
        }

        [TestCase("Some Summary", "Some url", "Some Hosting", "Some Connectivity", true)]
        [TestCase("Some Summary", "", "Some Hosting", "Some Connectivity", true)]
        [TestCase("     ", "     ", "Some Hosting", "Some Connectivity", true)]
        [TestCase("Some Summary", "Some url", null, "Some Connectivity", true)]
        [TestCase(" ", "Some url", "Some Hosting", "Some Connectivity", true)]
        [TestCase("Some Summary", "Some url", "Some Hosting", "     ", true)]
        [TestCase("Some Summary", "     ", "Some Hosting", "     ", true)]
        [TestCase(null, null, "Some Hosting", "Some Connectivity", true)]
        [TestCase("Some Summary", "", null, "Some Connectivity", true)]
        [TestCase("     ", "     ", null, "Some Connectivity", true)]
        [TestCase(" ", "Some url", null, "Some Connectivity", true)]
        [TestCase("Some Summary", "Some url", null, "     ", true)]
        [TestCase("     ", "Some url", "Some Hosting", null, true)]
        [TestCase("     ", "", "Some Hosting", "        ", true)]
        [TestCase("Some Summary", "     ", null, "     ", true)]
        [TestCase(null, null, null, "Some Connectivity", true)]
        [TestCase(null, null, "Some Hosting", null, true)]
        [TestCase("     ", "", "    ", "        ", false)]
        [TestCase("     ", "Some url", null, null, true)]
        [TestCase(null, null, null, null, false)]
        public async Task IfPrivateCloudEmptyThenItHasNoData(
            string summary,
            string link,
            string hosting,
            string connectivityRequired,
            bool hasData)
        {
            Expression<Func<IPrivateCloud, bool>> privateCloud = p =>
                p.Summary == summary
                && p.Link == link
                && p.HostingModel == hosting
                && p.RequiresHSCN == connectivityRequired;

            Expression<Func<IHosting, bool>> hostingExpression = h =>
                h.PrivateCloud == Mock.Of(privateCloud);

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.Hosting == Mock.Of(hostingExpression);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            if (hasData)
            {
                publicResult.Sections.PrivateCloud.Answers.Summary.Should().Be(summary);
                publicResult.Sections.PrivateCloud.Answers.Link.Should().Be(link);
                publicResult.Sections.PrivateCloud.Answers.HostingModel.Should().Be(hosting);
                publicResult.Sections.PrivateCloud.Answers.RequiresHSCN.Should().Be(connectivityRequired);

                publicResult.Sections.PrivateCloud.Answers.HasData.Should().BeTrue();
            }
            else
            {
                publicResult.Sections.PrivateCloud.Should().BeNull();
            }
        }

        [TestCase("Some Summary", "Some url", "Some Hosting", "Some Connectivity", true)]
        [TestCase("Some Summary", "", "Some Hosting", "Some Connectivity", true)]
        [TestCase("     ", "     ", "Some Hosting", "Some Connectivity", true)]
        [TestCase("Some Summary", "Some url", null, "Some Connectivity", true)]
        [TestCase(" ", "Some url", "Some Hosting", "Some Connectivity", true)]
        [TestCase("Some Summary", "Some url", "Some Hosting", "     ", true)]
        [TestCase("Some Summary", "     ", "Some Hosting", "     ", true)]
        [TestCase(null, null, "Some Hosting", "Some Connectivity", true)]
        [TestCase("Some Summary", "", null, "Some Connectivity", true)]
        [TestCase("     ", "     ", null, "Some Connectivity", true)]
        [TestCase(" ", "Some url", null, "Some Connectivity", true)]
        [TestCase("Some Summary", "Some url", null, "     ", true)]
        [TestCase("     ", "Some url", "Some Hosting", null, true)]
        [TestCase("     ", "", "Some Hosting", "        ", true)]
        [TestCase("Some Summary", "     ", null, "     ", true)]
        [TestCase(null, null, null, "Some Connectivity", true)]
        [TestCase(null, null, "Some Hosting", null, true)]
        [TestCase("     ", "", "    ", "        ", false)]
        [TestCase("     ", "Some url", null, null, true)]
        [TestCase(null, null, null, null, false)]
        public async Task IfOnPremiseIsEmptyThenItHasNoData(
            string summary,
            string link,
            string hosting,
            string requiresHscn,
            bool hasData)
        {
            Expression<Func<IOnPremise, bool>> onPremise = p =>
                p.Summary == summary
                && p.Link == link
                && p.HostingModel == hosting
                && p.RequiresHSCN == requiresHscn;

            Expression<Func<IHosting, bool>> hostingExpression = h =>
                h.OnPremise == Mock.Of(onPremise);

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.Hosting == Mock.Of(hostingExpression);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            if (hasData)
            {
                publicResult.Sections.OnPremise.Answers.Summary.Should().Be(summary);
                publicResult.Sections.OnPremise.Answers.Link.Should().Be(link);
                publicResult.Sections.OnPremise.Answers.HostingModel.Should().Be(hosting);
                publicResult.Sections.OnPremise.Answers.RequiresHSCN.Should().Be(requiresHscn);

                publicResult.Sections.OnPremise.Answers.HasData.Should().BeTrue();
            }
            else
            {
                publicResult.Sections.OnPremise.Should().BeNull();
            }
        }

        [TestCase("Some Summary", "Some url", "Some Hosting", "Some Connectivity", true)]
        [TestCase("Some Summary", "", "Some Hosting", "Some Connectivity", true)]
        [TestCase("     ", "     ", "Some Hosting", "Some Connectivity", true)]
        [TestCase("Some Summary", "Some url", null, "Some Connectivity", true)]
        [TestCase(" ", "Some url", "Some Hosting", "Some Connectivity", true)]
        [TestCase("Some Summary", "Some url", "Some Hosting", "     ", true)]
        [TestCase("Some Summary", "     ", "Some Hosting", "     ", true)]
        [TestCase(null, null, "Some Hosting", "Some Connectivity", true)]
        [TestCase("Some Summary", "", null, "Some Connectivity", true)]
        [TestCase("     ", "     ", null, "Some Connectivity", true)]
        [TestCase(" ", "Some url", null, "Some Connectivity", true)]
        [TestCase("Some Summary", "Some url", null, "     ", true)]
        [TestCase("     ", "Some url", "Some Hosting", null, true)]
        [TestCase("     ", "", "Some Hosting", "        ", true)]
        [TestCase("Some Summary", "     ", null, "     ", true)]
        [TestCase(null, null, null, "Some Connectivity", true)]
        [TestCase(null, null, "Some Hosting", null, true)]
        [TestCase("     ", "", "    ", "        ", false)]
        [TestCase("     ", "Some url", null, null, true)]
        [TestCase(null, null, null, null, false)]
        public async Task IfHybridHostingTypeIsEmptyThenItHasNoData(
            string summary,
            string link,
            string hosting,
            string requiresHscn,
            bool hasData)
        {
            Expression<Func<IHybridHostingType, bool>> hybridHostingType = p =>
                p.Summary == summary
                && p.Link == link
                && p.HostingModel == hosting
                && p.RequiresHSCN == requiresHscn;

            Expression<Func<IHosting, bool>> hostingExpression = h =>
                h.HybridHostingType == Mock.Of(hybridHostingType);

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.Hosting == Mock.Of(hostingExpression);

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            if (hasData)
            {
                publicResult.Sections.HybridHostingType.Answers.Summary.Should().Be(summary);
                publicResult.Sections.HybridHostingType.Answers.Link.Should().Be(link);
                publicResult.Sections.HybridHostingType.Answers.HostingModel.Should().Be(hosting);
                publicResult.Sections.HybridHostingType.Answers.RequiresHSCN.Should().Be(requiresHscn);

                publicResult.Sections.HybridHostingType.Answers.HasData.Should().BeTrue();
            }
            else
            {
                publicResult.Sections.HybridHostingType.Should().BeNull();
            }
        }

        [Test]
        public async Task CapabilitiesIsNullForSolution()
        {
            Expression<Func<ISolution, bool>> solution = s =>
                s.Id == SolutionId1
                && s.Capabilities == null
                && s.PublishedStatus == PublishedStatus.Published;

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            publicResult.Id.Should().Be(SolutionId1);
            publicResult.Sections.Capabilities.Should().BeNull();
        }

        [Test]
        public async Task EmptyContactShouldReturnNoData()
        {
#pragma warning disable CA1820 // Test for empty strings using string length (mock set-up)
            Expression<Func<IContact, bool>> contact1 = m =>
                m.Name == null
                && m.Department == string.Empty
                && m.Email == string.Empty
                && m.PhoneNumber == "   ";

            Expression<Func<IContact, bool>> contact2 = m =>
                m.Name == string.Empty
                && m.Department == string.Empty
                && m.Email == string.Empty
                && m.PhoneNumber == string.Empty;
#pragma warning restore CA1820 // Test for empty strings using string length

            var contacts = new List<IContact>
            {
                Mock.Of(contact1),
                Mock.Of(contact2),
            };

            // ReSharper disable once PossibleUnintendedReferenceComparison (mock set-up)
            Expression<Func<ISolution, bool>> solution = s =>
                s.Id == SolutionId1
                && s.Contacts == contacts
                && s.PublishedStatus == PublishedStatus.Published;

            var contact = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            contact.Id.Should().Be(SolutionId1);
            contact.Sections.ContactDetails.Should().BeNull();
        }

        [Test]
        public async Task IfBrowserBasedThenAdditionalInformationCanBeSet()
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" }
                && c.AdditionalInformation == "Some Additional Info";

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections
                .BrowserAdditionalInformationSection.Answers.AdditionalInformation.Should().Be("Some Additional Info");
        }

        [Test]
        public async Task IfBrowserBasedThenHardwareRequirementsCanBeSet()
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" }
                && c.HardwareRequirements == "New Hardware";

            Expression<Func<ISolution, bool>> solution = s =>
                s.ClientApplication == Mock.Of(clientApplication)
                && s.PublishedStatus == PublishedStatus.Published;

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections
                .BrowserHardwareRequirementsSection.Answers.HardwareRequirements.Should().Be("New Hardware");
        }

        [Test]
        public async Task MultipleCapabilitiesForDifferentSolutions()
        {
            var capabilityData = GetClaimedCapabilityTestData();
            var solution1Capabilities = capabilityData.Take(2);
            var solution2Capabilities = capabilityData.Skip(2).Take(3);

            // ReSharper disable once PossibleUnintendedReferenceComparison (mock set-up)
            Expression<Func<ISolution, bool>> solution1 = s =>
                s.Id == SolutionId1
                && s.Capabilities == solution1Capabilities.Select(c => c.Capability)
                && s.PublishedStatus == PublishedStatus.Published;

            // ReSharper disable once PossibleUnintendedReferenceComparison (mock set-up)
            Expression<Func<ISolution, bool>> solution2 = s =>
                s.Id == SolutionId2
                && s.Capabilities == solution2Capabilities.Select(c => c.Capability)
                && s.PublishedStatus == PublishedStatus.Published;

            var publicResult1 = await GetSolutionPublicResultAsync(Mock.Of(solution1), SolutionId1);
            var publicResult2 = await GetSolutionPublicResultAsync(Mock.Of(solution2), SolutionId2);

            publicResult1.Id.Should().Be(SolutionId1);
            publicResult1.Sections.Capabilities.Answers.CapabilitiesMet.Should()
                .BeEquivalentTo(solution1Capabilities.Select(c => c.Section));

            publicResult2.Id.Should().Be(SolutionId2);
            publicResult2.Sections.Capabilities.Answers.CapabilitiesMet.Should()
                .BeEquivalentTo(solution2Capabilities.Select(c => c.Section));
        }

        [Test]
        public async Task NoSolutionShouldReturnNotFound()
        {
            var result = (await solutionsController.Public(SolutionId1)).Result as NotFoundResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId1), It.IsAny<CancellationToken>()));
        }

        [Test]
        public void NullSolutionShouldThrowNullExceptionPublicResult()
        {
            var solution = new SolutionResult(null);
            solution.Id.Should().BeNull();
            solution.Name.Should().BeNull();
            solution.SupplierName.Should().BeNull();
            solution.LastUpdated.Should().BeNull();
            solution.IsFoundation.Should().BeNull();

            solution.Sections.Should().BeNull();
        }

        [Test]
        public void NullSolutionShouldThrowNullExceptionSolutionDescriptionPublicAnswers()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new SolutionDescriptionSectionAnswers(null));
        }

        [Test]
        public async Task ShouldCheckForNullClientApplicationTypes()
        {
            Expression<Func<ISolution, bool>> solution = s =>
                s.ClientApplication == null
                && s.PublishedStatus == PublishedStatus.Published;

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            publicResult.Sections.ClientApplicationTypes.Should().BeNull();
        }

        [Test]
        public async Task ShouldGetContacts()
        {
            Expression<Func<IContact, bool>> contact1 = m =>
                m.Name == "name1"
                && m.Department == "dep1"
                && m.Email == "test@gmail.com"
                && m.PhoneNumber == "01234567890";

            Expression<Func<IContact, bool>> contact2 = m =>
                m.Name == "name2"
                && m.Department == "dep2"
                && m.Email == "test2@gmail.com"
                && m.PhoneNumber == "12345678901";

            var contacts = new List<IContact>
            {
                Mock.Of(contact1),
                Mock.Of(contact2),
            };

            // ReSharper disable once PossibleUnintendedReferenceComparison (mock set-up)
            Expression<Func<ISolution, bool>> solution = s =>
                s.Id == SolutionId1
                && s.Contacts == contacts
                && s.PublishedStatus == PublishedStatus.Published;

            var contact = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            contact.Id.Should().Be(SolutionId1);

            contact.Sections.ContactDetails.Answers.Contact1.ContactName.Should().BeEquivalentTo(contacts[0].Name);
            contact.Sections.ContactDetails.Answers.Contact1.DepartmentName.Should()
                .BeEquivalentTo(contacts[0].Department);

            contact.Sections.ContactDetails.Answers.Contact1.EmailAddress.Should().BeEquivalentTo(contacts[0].Email);
            contact.Sections.ContactDetails.Answers.Contact1.PhoneNumber.Should()
                .BeEquivalentTo(contacts[0].PhoneNumber);

            contact.Sections.ContactDetails.Answers.Contact2.ContactName.Should().BeEquivalentTo(contacts[1].Name);
            contact.Sections.ContactDetails.Answers.Contact2.DepartmentName.Should()
                .BeEquivalentTo(contacts[1].Department);

            contact.Sections.ContactDetails.Answers.Contact2.EmailAddress.Should().BeEquivalentTo(contacts[1].Email);
            contact.Sections.ContactDetails.Answers.Contact2.PhoneNumber.Should()
                .BeEquivalentTo(contacts[1].PhoneNumber);
        }

        [Test]
        public async Task ShouldIncludeBrowserBasedDataIfClientApplicationTypesIncludeBrowserBased()
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" }
                && c.BrowsersSupported == new HashSet<string> { "Chrome", "Edge" }
                && c.MobileResponsive == true;

            Expression<Func<ISolution, bool>> solution = s =>
                s.ClientApplication == Mock.Of(clientApplication)
                && s.PublishedStatus == PublishedStatus.Published;

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers
                .SupportedBrowsers
                .Should().BeEquivalentTo(new HashSet<string> { "Chrome", "Edge" });

            publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers
                .MobileResponsive
                .Should().Be("Yes");
        }

        [Test]
        public async Task ShouldIncludeBrowserBasedDataIfClientApplicationTypesIncludePluginInformation()
        {
            Expression<Func<IPlugins, bool>> plugins = p =>
                p.Required == true
                && p.AdditionalInformation == "Plugin additional information";

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" }
                && c.Plugins == Mock.Of(plugins);

            Expression<Func<ISolution, bool>> solution = s =>
                s.ClientApplication == Mock.Of(clientApplication)
                && s.PublishedStatus == PublishedStatus.Published;

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.PluginOrExtensionsSection
                .Answers.Required
                .Should().Be("Yes");

            publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.PluginOrExtensionsSection
                .Answers.AdditionalInformation
                .Should().Be("Plugin additional information");
        }

        [Test]
        public async Task ShouldIncludeNativeDesktopDataIfClientApplicationTypesIncludeNativeDesktop()
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-desktop" }
                && c.NativeDesktopHardwareRequirements == "Hardware requirements";

            Expression<Func<ISolution, bool>> solution = s =>
                s.ClientApplication == Mock.Of(clientApplication)
                && s.PublishedStatus == PublishedStatus.Published;

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            publicResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections.HardwareRequirementsSection
                .Answers.HardwareRequirements.Should().Be("Hardware requirements");
        }

        [Test]
        public async Task ShouldNotIncludeBrowserBasedDataIfClientApplicationTypesDoNotIncludeBrowserBased()
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-desktop", "native-mobile" }
                && c.BrowsersSupported == new HashSet<string> { "Chrome", "Edge" }
                && c.MobileResponsive == true;

            Expression<Func<ISolution, bool>> solution = s =>
                s.ClientApplication == Mock.Of(clientApplication)
                && s.PublishedStatus == PublishedStatus.Published;

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            publicResult.Sections.ClientApplicationTypes.Should().BeNull();
        }

        [Test]
        public async Task ShouldNotIncludeNativeDesktopDataIfClientApplicationTypesDoNotIncludeNativeDesktop()
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" }
                && c.NativeDesktopHardwareRequirements == "Hardware requirements";

            Expression<Func<ISolution, bool>> solution = s =>
                s.ClientApplication == Mock.Of(clientApplication)
                && s.PublishedStatus == PublishedStatus.Published;

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            publicResult.Sections.ClientApplicationTypes.Should().BeNull();
        }

        [Test]
        public async Task SingleContactShouldReturnNullForEmptyData()
        {
#pragma warning disable CA1820 // Test for empty strings using string length (mock set-up)
            Expression<Func<IContact, bool>> contact1 = m =>
                m.Name == "Hello"
                && m.Department == string.Empty
                && m.Email == string.Empty
                && m.PhoneNumber == "   ";

            Expression<Func<IContact, bool>> contact2 = m =>
                m.Name == string.Empty
                && m.Department == string.Empty
                && m.Email == string.Empty
                && m.PhoneNumber == string.Empty;
#pragma warning restore CA1820 // Test for empty strings using string length

            var contacts = new List<IContact>
            {
                Mock.Of(contact1),
                Mock.Of(contact2),
            };

            // ReSharper disable once PossibleUnintendedReferenceComparison (mock set-up)
            Expression<Func<ISolution, bool>> solution = s =>
                s.Id == SolutionId1
                && s.Contacts == contacts
                && s.PublishedStatus == PublishedStatus.Published;

            var contact = await GetSolutionPublicResultAsync(Mock.Of(solution), SolutionId1);

            contact.Id.Should().Be(SolutionId1);
            contact.Sections.ContactDetails.Should().NotBeNull();
            contact.Sections.ContactDetails.Answers.HasData().Should().BeTrue();
            contact.Sections.ContactDetails.Answers.Contact2.Should().BeNull();
            contact.Sections.ContactDetails.Answers.Contact1.ContactName.Should().NotBeNull();
            contact.Sections.ContactDetails.Answers.Contact1.PhoneNumber.Should().BeNull();
            contact.Sections.ContactDetails.Answers.Contact1.DepartmentName.Should().BeNull();
            contact.Sections.ContactDetails.Answers.Contact1.EmailAddress.Should().BeNull();
        }

        [Test]
        public async Task UnpublishedSolutionShouldReturnNotFound()
        {
            Expression<Func<ISolution, bool>> solutionExpression = s =>
                s.Id == SolutionId1
                && s.PublishedStatus == PublishedStatus.Draft;

            var solution = Mock.Of(solutionExpression);

            mockMediator
                .Setup(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await solutionsController.Public(SolutionId1)).Result as NotFoundResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId1), It.IsAny<CancellationToken>()));
        }

        private static List<(IClaimedCapability Capability, ClaimedCapabilitySection Section)> GetClaimedCapabilityTestData()
        {
            var data = new List<(IClaimedCapability, ClaimedCapabilitySection)>();
            for (int index = 1; index <= 5; index++)
            {
                var capabilityNumber = index;
                var claimedEpics = new[]
                {
                    Mock.Of<IClaimedCapabilityEpic>(ce =>
                        ce.EpicId == $"{capabilityNumber}E1"
                        && ce.EpicName == $"Cap {capabilityNumber} Epic 1 Name"
                        && ce.IsMet
                        && ce.EpicCompliancyLevel == "MUST"),
                    Mock.Of<IClaimedCapabilityEpic>(ce =>
                        ce.EpicId == $"{capabilityNumber}E2"
                        && ce.EpicName == $"Cap {capabilityNumber} Epic 2 Name"
                        && ce.IsMet
                        && ce.EpicCompliancyLevel == "MAY"),
                    Mock.Of<IClaimedCapabilityEpic>(ce =>
                        ce.EpicId == $"{capabilityNumber}E3"
                        && ce.EpicName == $"Cap {capabilityNumber} Epic 3 Name"
                        && ce.IsMet == false
                        && ce.EpicCompliancyLevel == "MUST"),
                    Mock.Of<IClaimedCapabilityEpic>(ce =>
                        ce.EpicId == $"{capabilityNumber}E4"
                        && ce.EpicName == $"Cap {capabilityNumber} Epic 4 Name"
                        && ce.IsMet == false
                        && ce.EpicCompliancyLevel == "MAY"),
                };

                var ccMock = Mock.Of<IClaimedCapability>(cc =>
                    cc.Name == $"Capability {capabilityNumber}"
                    && cc.Version == $"Version {capabilityNumber}"
                    && cc.Description == $"Description {capabilityNumber}"
                    && cc.Link == $"http://Capability.Link/{capabilityNumber}"
                    && cc.ClaimedEpics == claimedEpics);

                data.Add((ccMock, new ClaimedCapabilitySection(ccMock)));
            }

            return data;
        }

        private async Task<SolutionResult> GetSolutionPublicResultAsync(ISolution solution, string solutionId)
        {
            mockMediator
                .Setup(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == solutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await solutionsController.Public(solutionId)).Result as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == solutionId), It.IsAny<CancellationToken>()));

            return result.Value as SolutionResult;
        }
    }
}
