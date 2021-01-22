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
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;
using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class SolutionsControllerGetPreviewTests
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
        public async Task ShouldReturnEmpty()
        {
            var result = (await solutionsController.Preview(SolutionId)).Result as ObjectResult;

            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            (result.Value as SolutionResult).Id.Should().BeNull();
        }

        [TestCase(null, null, null)]
        [TestCase("Sln2", null, null)]
        [TestCase(null, "name", null)]
        [TestCase(null, null, "supplier")]
        [TestCase("Sln2", null, "supplier")]
        [TestCase(null, "name", "supplier")]
        [TestCase("Sln2", "name", "supplier")]
        public async Task ShouldReturnGetValues(string id, string name, string supplier)
        {
            Expression<Func<ISolution, bool>> solution = s =>
                s.Id == id
                && s.Name == name
                && s.SupplierName == supplier;

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            previewResult.Id.Should().Be(id);
            previewResult.Name.Should().Be(name);
            previewResult.SupplierName.Should().Be(supplier);
        }

        [TestCase(null, null, null, false)]
        [TestCase("summary", null, null, true)]
        [TestCase(null, "Desc", null, true)]
        [TestCase(null, null, "Link", true)]
        [TestCase("summary", "Desc", null, true)]
        [TestCase("summary", null, "Link", true)]
        [TestCase(null, "Desc", "Link", true)]
        [TestCase("summary", "Desc", "Link", true)]
        public async Task ShouldGetPreviewCalculateSolutionDescription(string summary, string description, string link, bool hasData)
        {
            Expression<Func<ISolution, bool>> solution = s =>
                s.Summary == summary
                && s.Description == description
                && s.AboutUrl == link;

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.SolutionDescription.Answers.HasData.Should().Be(true);
                previewResult.Sections.SolutionDescription.Should().NotBe(null);
                previewResult.Sections.SolutionDescription.Answers.Summary.Should().Be(summary);
                previewResult.Sections.SolutionDescription.Answers.Description.Should().Be(description);
                previewResult.Sections.SolutionDescription.Answers.Link.Should().Be(link);
            }
            else
            {
                previewResult.Sections.SolutionDescription.Should().BeNull();
            }
        }

        [Test]
        public async Task GetPreviewFeaturesListItemsIsNull()
        {
            var features = new List<string> { null };

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s => s.Features == features));
            previewResult.Sections.Features.Should().BeNull();
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public async Task ShouldGetPreviewCalculateFeatures(bool isFeature, bool hasData)
        {
            var features = isFeature ? new List<string> { "Feature1", "Feature2" } : new List<string>();

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s => s.Features == features));

            if (hasData)
            {
                previewResult.Sections.Features.Answers.HasData.Should().BeTrue();
                previewResult.Sections.Features.Answers.Listing.Should().BeEquivalentTo(
                    new List<string> { "Feature1", "Feature2" });
            }
            else
            {
                previewResult.Sections.Features.Should().BeNull();
            }
        }

        [TestCase(null, null, false)]
        [TestCase("some description", null, true)]
        [TestCase(" some description    ", null, true)]
        [TestCase(" ", null, false)]
        [TestCase("", null, false)]
        [TestCase("some description", "roadMap.pdf", true)]
        [TestCase(null, "roadMap.pdf", true)]
        public async Task ShouldGetPreviewCalculateRoadMap(string description, string documentName, bool hasData)
        {
            Expression<Func<ISolution, bool>> solution = s =>
                s.RoadMap.Summary == description
                && s.RoadMap.DocumentName == documentName;

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.RoadMap.Should().NotBe(null);
                previewResult.Sections.RoadMap.Answers.HasData.Should().Be(true);
                previewResult.Sections.RoadMap.Answers.Summary.Should().Be(description);
                previewResult.Sections.RoadMap.Answers.DocumentName.Should().Be(documentName);
            }
            else
            {
                previewResult.Sections.RoadMap.Should().BeNull();
            }
        }

        [TestCase(null, null, false)]
        [TestCase("some integrations url", null, true)]
        [TestCase(" some integrations url    ", null, true)]
        [TestCase(" ", null, false)]
        [TestCase("", null, false)]
        [TestCase("some integrations url", "integration.pdf", true)]
        [TestCase(null, "integration.pdf", true)]
        public async Task ShouldGetPreviewCalculateIntegrations(string url, string documentName, bool hasData)
        {
            Expression<Func<IIntegrations, bool>> integrations = i =>
                i.Url == url
                && i.DocumentName == documentName;

            Expression<Func<ISolution, bool>> solution = s => s.Integrations == Mock.Of(integrations);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.Integrations.Should().NotBe(null);
                previewResult.Sections.Integrations.Answers.HasData.Should().Be(true);
                previewResult.Sections.Integrations.Answers.IntegrationsUrl.Should().Be(url);
                previewResult.Sections.Integrations.Answers.DocumentName.Should().Be(documentName);
            }
            else
            {
                previewResult.Sections.Integrations.Should().BeNull();
            }
        }

        [TestCase(null, false)]
        [TestCase("some implementation timescales description", true)]
        [TestCase(" some implementation timescales description    ", true)]
        [TestCase(" ", false)]
        [TestCase("", false)]
        public async Task ShouldGetPreviewImplementationTimescales(string description, bool hasData)
        {
            Expression<Func<IImplementationTimescales, bool>> implementationTimescales = i => i.Description == description;
            Expression<Func<ISolution, bool>> solution = s => s.ImplementationTimescales == Mock.Of(implementationTimescales);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.ImplementationTimescales.Should().NotBe(null);
                previewResult.Sections.ImplementationTimescales.Answers.HasData.Should().Be(true);
                previewResult.Sections.ImplementationTimescales.Answers.Description.Should().Be(description);
            }
            else
            {
                previewResult.Sections.ImplementationTimescales.Should().BeNull();
            }
        }

        [TestCase(null, false)]
        [TestCase("some integrations timescales description", true)]
        [TestCase(" some integrations timescales description    ", true)]
        [TestCase(" ", false)]
        [TestCase("", false)]
        public async Task ShouldGetPreviewCalculateImplementationTimescales(string description, bool hasData)
        {
            Expression<Func<IImplementationTimescales, bool>> implementationTimescales = i => i.Description == description;
            Expression<Func<ISolution, bool>> solution = s => s.ImplementationTimescales == Mock.Of(implementationTimescales);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.ImplementationTimescales.Should().NotBe(null);
                previewResult.Sections.ImplementationTimescales.Answers.HasData.Should().Be(true);
                previewResult.Sections.ImplementationTimescales.Answers.Description.Should().Be(description);
            }
            else
            {
                previewResult.Sections.Integrations.Should().BeNull();
            }
        }

        [TestCase(false, false, null, false)]
        [TestCase(true, false, null, false)]
        [TestCase(true, true, null, true)]
        [TestCase(true, false, false, true)]
        [TestCase(true, false, true, true)]
        [TestCase(true, true, false, true)]
        [TestCase(true, true, true, true)]
        public async Task ShouldGetPreviewCalculateClientApplication(bool isClientApplication, bool isBrowserSupported, bool? mobileResponsive, bool expectData)
        {
            var clientApplicationTypes = isClientApplication ? new HashSet<string> { "browser-based", "native-mobile" } : new HashSet<string>();
            var browsersSupported = isBrowserSupported ? new HashSet<string> { "Chrome", "Edge" } : new HashSet<string>();

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == clientApplicationTypes
                && c.BrowsersSupported == browsersSupported
                && c.MobileResponsive == mobileResponsive;

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (expectData)
            {
                previewResult.Sections.ClientApplicationTypes.Sections.HasData.Should().Be(true);

                if (mobileResponsive.HasValue)
                {
                    previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers.MobileResponsive
                        .Should()
                        .Be(mobileResponsive.GetValueOrDefault() ? "Yes" : "No");
                }
                else
                {
                    previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers
                        .MobileResponsive.Should().BeNull();
                }

                if (isBrowserSupported)
                {
                    previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers.SupportedBrowsers
                        .Should().BeEquivalentTo(isClientApplication
                            ? new HashSet<string> { "Chrome", "Edge" }
                            : new HashSet<string>());
                }
                else
                {
                    previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers
                        .SupportedBrowsers.Should().BeNull();
                }
            }
            else
            {
                previewResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [Test]
        public async Task ShouldIncludeBrowserBasedDataIfClientApplicationTypesIncludeBrowserBased()
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" }
                && c.BrowsersSupported == new HashSet<string> { "Chrome", "Edge" }
                && c.MobileResponsive == true;

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers.SupportedBrowsers
                .Should().BeEquivalentTo(new HashSet<string> { "Chrome", "Edge" });

            previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers.MobileResponsive
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

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.PluginOrExtensionsSection.Answers.Required
                .Should().Be("Yes");

            previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.PluginOrExtensionsSection.Answers.AdditionalInformation
                .Should().Be("Plugin additional information");
        }

        [Test]
        public async Task IfBrowserBasedThenHardwareRequirementsCanBeSet()
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" }
                && c.HardwareRequirements == "New Hardware";

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections
                .BrowserHardwareRequirementsSection.Answers.HardwareRequirements.Should().Be("New Hardware");
        }

        [Test]
        public async Task IfBrowserBasedThenAdditionalInformationCanBeSet()
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" }
                && c.AdditionalInformation == "Some Additional Info";

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections
                .BrowserAdditionalInformationSection.Answers.AdditionalInformation.Should().Be("Some Additional Info");
        }

        [Test]
        public async Task ShouldNotIncludeBrowserBasedDataIfClientApplicationTypesDoNotIncludeBrowserBased()
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-desktop", "native-mobile" }
                && c.BrowsersSupported == new HashSet<string> { "Chrome", "Edge" }
                && c.MobileResponsive == true;

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            previewResult.Sections.ClientApplicationTypes.Should().BeNull();
        }

        [TestCase("1GBps", "1x1", true)]
        [TestCase(null, "1x1", true)]
        [TestCase("1GBps", null, true)]
        [TestCase(null, null, false)]
        [TestCase("    ", "    ", false)]
        [TestCase("", "", false)]
        [TestCase("	", "	", false)]
        public async Task ConnectivityAndResolutionSectionIsSetCorrectly(string connectivity, string resolution, bool hasData)
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "browser-based" }
                && c.MinimumConnectionSpeed == connectivity
                && c.MinimumDesktopResolution == resolution;

            Expression<Func<ISolution, bool>> solution = s =>
                s.PublishedStatus == PublishedStatus.Published
                && s.ClientApplication == Mock.Of(clientApplication);

            var publicResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));
            var connectivitySection = publicResult?.Sections?.ClientApplicationTypes?.Sections?.BrowserBased?.Sections?
                .BrowserConnectivityAndResolutionSection;

            if (!hasData)
            {
                connectivitySection.Should().BeNull();
                return;
            }

            connectivitySection.Should().NotBeNull();
            connectivitySection.Answers.HasData.Should().Be(hasData);
            connectivitySection.Answers.MinimumConnectionSpeed.Should().Be(connectivity);
            connectivitySection.Answers.MinimumDesktopResolution.Should().Be(resolution);
        }

        [TestCase(null, null)]
        [TestCase(false, "No")]
        [TestCase(true, "Yes")]
        public async Task BrowserMobileFirstIsSetCorrectly(bool? mobileFirst, string result)
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "browser-based" }
                && c.MobileFirstDesign == mobileFirst;

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var publicResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            var mobileFirstSection = publicResult?.Sections?.ClientApplicationTypes?.Sections?.BrowserBased?.Sections
                ?.BrowserMobileFirstSection;

            if (result == null)
            {
                mobileFirstSection.Should().BeNull();
                return;
            }

            mobileFirstSection.Answers.MobileFirstDesign.Should().Be(result);
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
                c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" }
                && c.MobileOperatingSystems == Mock.Of(mobileOperatingSystems);

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            var operatingSystemResult = previewResult?.Sections?.ClientApplicationTypes?.Sections?.NativeMobile
                ?.Sections?.MobileOperatingSystemsSection;

            if (!hasData)
            {
                operatingSystemResult.Should().BeNull();
                return;
            }

            operatingSystemResult.Should().NotBeNull();
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

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            var mobileConnectionDetailsResult = previewResult?.Sections?.ClientApplicationTypes?.Sections?.NativeMobile
                ?.Sections?.MobileConnectionDetailsSection;

            if (!hasData)
            {
                mobileConnectionDetailsResult.Should().BeNull();
                return;
            }

            mobileConnectionDetailsResult.Should().NotBeNull();
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

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            var mobileMemoryStorageResult = previewResult?.Sections?.ClientApplicationTypes?.Sections?.NativeMobile
                ?.Sections?.MobileMemoryAndStorageSection;

            if (!hasData)
            {
                mobileMemoryStorageResult.Should().BeNull();
                return;
            }

            mobileMemoryStorageResult.Should().NotBeNull();
            mobileMemoryStorageResult.Answers.HasData.Should().BeTrue();
            mobileMemoryStorageResult.Answers.MinimumMemoryRequirement.Should().Be(minMemoryManagement);
            mobileMemoryStorageResult.Answers.Description.Should().Be(description);
        }

        [Test]
        public void NullSolutionShouldThrowNullExceptionSolutionDescriptionPreviewAnswers()
        {
            Assert.Throws<ArgumentNullException>(() => new SolutionDescriptionSectionAnswers(null));
        }

        [TestCase("New hardware", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("      ", false)]
        public async Task IfNativeMobileThenHardwareRequirementsIsSetCorrectly(string requirements, bool hasData)
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-mobile" }
                && c.NativeMobileHardwareRequirements == requirements;

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.ClientApplicationTypes.Sections.NativeMobile.Sections
                    .HardwareRequirementsSection.Answers.HardwareRequirements.Should().Be(requirements);

                previewResult.Sections.ClientApplicationTypes.Sections.NativeMobile.Sections
                    .HardwareRequirementsSection.Answers.HasData.Should().BeTrue();
            }
            else
            {
                previewResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [TestCase(null, null, false)]
        [TestCase(" ", "     ", false)]
        [TestCase("Component", null, true)]
        [TestCase(null, "Capability", true)]
        [TestCase("Component", "Capability", true)]
        public async Task IfNativeMobileEmptyThenMobileThirdPartyHasNoData(string component, string capability, bool hasData)
        {
            Expression<Func<IMobileThirdParty, bool>> mobileThirdParty = m =>
                m.ThirdPartyComponents == component
                && m.DeviceCapabilities == capability;

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-mobile" }
                && c.MobileThirdParty == Mock.Of(mobileThirdParty);

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.ClientApplicationTypes.Sections.NativeMobile.Sections.MobileThirdPartySection
                    .Answers.ThirdPartyComponents.Should().Be(component);

                previewResult.Sections.ClientApplicationTypes.Sections.NativeMobile.Sections.MobileThirdPartySection
                    .Answers.DeviceCapabilities.Should().Be(capability);

                previewResult.Sections.ClientApplicationTypes.Sections.NativeMobile.Sections.MobileThirdPartySection
                    .Answers.HasData.Should().BeTrue();
            }
            else
            {
                previewResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [TestCase("New info", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("      ", false)]
        public async Task IfNativeMobileThenAdditionalInformationIsSetCorrectly(string additionalInformation, bool hasData)
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-mobile" }
                && c.NativeMobileAdditionalInformation == additionalInformation;

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.ClientApplicationTypes.Sections.NativeMobile.Sections
                    .MobileAdditionalInformationSection.Answers.NativeMobileAdditionalInformation.Should().Be(additionalInformation);

                previewResult.Sections.ClientApplicationTypes.Sections.NativeMobile.Sections
                    .MobileAdditionalInformationSection.Answers.HasData.Should().BeTrue();
            }
            else
            {
                previewResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [Test]
        public void NullSolutionShouldReturnEmptyPreviewSections()
        {
            var solution = new SolutionResult(null);
            solution.Id.Should().BeNull();
            solution.Name.Should().BeNull();
            solution.SupplierName.Should().BeNull();
            solution.LastUpdated.Should().BeNull();
            solution.IsFoundation.Should().BeNull();

            solution.Sections.Should().BeNull();
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

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .HardwareRequirementsSection.Answers.HardwareRequirements.Should().Be(requirements);

                previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .HardwareRequirementsSection.Answers.HasData.Should().BeTrue();
            }
            else
            {
                previewResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [TestCase("New Summary", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("      ", false)]
        public async Task IfNativeDesktopThenNativeOperatingSystemsDescriptionIsSetCorrectly(string description, bool hasData)
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-desktop" }
                && c.NativeDesktopOperatingSystemsDescription == description;

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .OperatingSystemsSection.Answers.OperatingSystemsDescription.Should().Be(description);

                previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .OperatingSystemsSection.Answers.HasData.Should().BeTrue();
            }
            else
            {
                previewResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [TestCase("3 Mbps", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("      ", false)]
        public async Task IfNativeDesktopThenNativeConnectivityDetailsIsSetCorrectly(string minimumConnectionSpeed, bool hasData)
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-desktop" }
                && c.NativeDesktopMinimumConnectionSpeed == minimumConnectionSpeed;

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .NativeDesktopConnectivityDetailsSection.Answers.NativeDesktopMinimumConnectionSpeed.Should().Be(minimumConnectionSpeed);

                previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .NativeDesktopConnectivityDetailsSection.Answers.HasData.Should().BeTrue();
            }
            else
            {
                previewResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [TestCase(null, null, false)]
        [TestCase("", "     ", false)]
        [TestCase("     ", "", false)]
        [TestCase("     ", "        ", false)]
        [TestCase("Component", null, true)]
        [TestCase(null, "Capability", true)]
        [TestCase("Component", "Capability", true)]
        public async Task NativeDesktopThirdPartySectionIsPopulatedCorrectly(string component, string capability, bool hasData)
        {
            Expression<Func<INativeDesktopThirdParty, bool>> nativeDesktopThirdParty = d =>
                d.ThirdPartyComponents == component
                && d.DeviceCapabilities == capability;

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-desktop" }
                && c.NativeDesktopThirdParty == Mock.Of(nativeDesktopThirdParty);

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections.NativeDesktopThirdPartySection
                    .Answers.ThirdPartyComponents.Should().Be(component);

                previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections.NativeDesktopThirdPartySection
                    .Answers.DeviceCapabilities.Should().Be(capability);

                previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections.NativeDesktopThirdPartySection
                    .Answers.HasData.Should().BeTrue();
            }
            else
            {
                previewResult.Sections.ClientApplicationTypes.Should().BeNull();
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
        public async Task NativeDesktopMemoryAndStorageSectionIsPopulatedCorrectly(string memory, string storage, string cpu, string resolution, bool hasData)
        {
            Expression<Func<INativeDesktopMemoryAndStorage, bool>> nativeDesktopMemoryAndStorage = d =>
                d.MinimumMemoryRequirement == memory
                && d.StorageRequirementsDescription == storage
                && d.MinimumCpu == cpu
                && d.RecommendedResolution == resolution;

            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-desktop" }
                && c.NativeDesktopMemoryAndStorage == Mock.Of(nativeDesktopMemoryAndStorage);

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections.NativeDesktopMemoryAndStorageSection
                    .Answers.MinimumMemoryRequirement.Should().Be(memory);

                previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections.NativeDesktopMemoryAndStorageSection
                    .Answers.StorageRequirementsDescription.Should().Be(storage);

                previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections.NativeDesktopMemoryAndStorageSection
                    .Answers.MinimumCpu.Should().Be(cpu);

                previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections.NativeDesktopMemoryAndStorageSection
                    .Answers.RecommendedResolution.Should().Be(resolution);

                previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections.NativeDesktopMemoryAndStorageSection
                    .Answers.HasData.Should().BeTrue();
            }
            else
            {
                previewResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }

        [TestCase("New info", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("      ", false)]
        public async Task IfNativeDesktopEmptyThenAdditionalInformationHasNoData(string additionalInformation, bool hasData)
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-desktop" }
                && c.NativeDesktopAdditionalInformation == additionalInformation;

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .NativeDesktopAdditionalInformationSection.Answers.NativeDesktopAdditionalInformation.Should().Be(additionalInformation);

                previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections
                    .NativeDesktopAdditionalInformationSection.Answers.HasData.Should().BeTrue();
            }
            else
            {
                previewResult.Sections.ClientApplicationTypes.Should().BeNull();
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
        public async Task IfPublicCloudEmptyThenItHasNoData(string summary, string link, string requiresHscn, bool hasData)
        {
            Expression<Func<IPublicCloud, bool>> publicCloud = p =>
                p.Summary == summary
                && p.Link == link
                && p.RequiresHSCN == requiresHscn;

            Expression<Func<IHosting, bool>> hosting = h => h.PublicCloud == Mock.Of(publicCloud);
            Expression<Func<ISolution, bool>> solution = s => s.Hosting == Mock.Of(hosting);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.PublicCloud.Answers.Summary.Should().Be(summary);
                previewResult.Sections.PublicCloud.Answers.Link.Should().Be(link);
                previewResult.Sections.PublicCloud.Answers.RequiresHSCN.Should().Be(requiresHscn);

                previewResult.Sections.PublicCloud.Answers.HasData.Should().BeTrue();
            }
            else
            {
                previewResult.Sections.PublicCloud.Should().BeNull();
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

            Expression<Func<IHosting, bool>> hostingExpression = h => h.PrivateCloud == Mock.Of(privateCloud);
            Expression<Func<ISolution, bool>> solution = s => s.Hosting == Mock.Of(hostingExpression);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.PrivateCloud.Answers.Summary.Should().Be(summary);
                previewResult.Sections.PrivateCloud.Answers.Link.Should().Be(link);
                previewResult.Sections.PrivateCloud.Answers.HostingModel.Should().Be(hosting);
                previewResult.Sections.PrivateCloud.Answers.RequiresHSCN.Should().Be(connectivityRequired);

                previewResult.Sections.PrivateCloud.Answers.HasData.Should().BeTrue();
            }
            else
            {
                previewResult.Sections.PrivateCloud.Should().BeNull();
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

            Expression<Func<IHosting, bool>> hostingExpression = h => h.OnPremise == Mock.Of(onPremise);
            Expression<Func<ISolution, bool>> solution = s => s.Hosting == Mock.Of(hostingExpression);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.OnPremise.Answers.Summary.Should().Be(summary);
                previewResult.Sections.OnPremise.Answers.Link.Should().Be(link);
                previewResult.Sections.OnPremise.Answers.HostingModel.Should().Be(hosting);
                previewResult.Sections.OnPremise.Answers.RequiresHSCN.Should().Be(requiresHscn);

                previewResult.Sections.OnPremise.Answers.HasData.Should().BeTrue();
            }
            else
            {
                previewResult.Sections.OnPremise.Should().BeNull();
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
            Expression<Func<IHybridHostingType, bool>> hybridHostingType = h =>
                h.Summary == summary
                && h.Link == link
                && h.HostingModel == hosting
                && h.RequiresHSCN == requiresHscn;

            Expression<Func<IHosting, bool>> hostingExpression = h => h.HybridHostingType == Mock.Of(hybridHostingType);
            Expression<Func<ISolution, bool>> solution = s => s.Hosting == Mock.Of(hostingExpression);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            if (hasData)
            {
                previewResult.Sections.HybridHostingType.Answers.Summary.Should().Be(summary);
                previewResult.Sections.HybridHostingType.Answers.Link.Should().Be(link);
                previewResult.Sections.HybridHostingType.Answers.HostingModel.Should().Be(hosting);
                previewResult.Sections.HybridHostingType.Answers.RequiresHSCN.Should().Be(requiresHscn);

                previewResult.Sections.HybridHostingType.Answers.HasData.Should().BeTrue();
            }
            else
            {
                previewResult.Sections.HybridHostingType.Should().BeNull();
            }
        }

        [Test]
        public async Task ShouldIncludeNativeDesktopDataIfClientApplicationTypesIncludeNativeDesktop()
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "native-desktop" }
                && c.NativeDesktopHardwareRequirements == "Hardware requirements";

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections.HardwareRequirementsSection
                .Answers.HardwareRequirements.Should().Be("Hardware requirements");
        }

        [Test]
        public async Task ShouldNotIncludeNativeDesktopDataIfClientApplicationTypesDoNotIncludeNativeDesktop()
        {
            Expression<Func<IClientApplication, bool>> clientApplication = c =>
                c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" }
                && c.NativeDesktopHardwareRequirements == "Hardware requirements";

            Expression<Func<ISolution, bool>> solution = s => s.ClientApplication == Mock.Of(clientApplication);

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            previewResult.Sections.ClientApplicationTypes.Should().BeNull();
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldGetCapabilitiesOnlyForSolution(bool hasCapability)
        {
            var capabilityData = GetClaimedCapabilityTestData().Take(2).ToArray();

            var capabilities = hasCapability ? capabilityData.Select(c => c.Capability) : Array.Empty<IClaimedCapability>();

            Expression<Func<ISolution, bool>> solution = s =>
                s.Id == SolutionId
                && s.Capabilities == capabilities;

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of(solution));

            previewResult.Id.Should().Be(SolutionId);
            if (hasCapability)
            {
                previewResult.Sections.Capabilities.Answers.CapabilitiesMet.Should().BeEquivalentTo(
                    capabilityData.Select(c => c.Section));
            }
            else
            {
                previewResult.Sections.Capabilities.Should().BeNull();
            }
        }

        private static IEnumerable<(IClaimedCapability Capability, ClaimedCapabilitySection Section)> GetClaimedCapabilityTestData()
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

                var claimedCapabilityMock = Mock.Of<IClaimedCapability>(cc =>
                    cc.Name == $"Capability {capabilityNumber}"
                    && cc.Version == $"Version {capabilityNumber}"
                    && cc.Description == $"Description {capabilityNumber}"
                    && cc.Link == $"http://Capability.Link/{capabilityNumber}"
                    && cc.ClaimedEpics == claimedEpics);

                data.Add((claimedCapabilityMock, new ClaimedCapabilitySection(claimedCapabilityMock)));
            }

            return data;
        }

        private async Task<SolutionResult> GetSolutionPreviewSectionAsync(ISolution solution)
        {
            mockMediator.Setup(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await solutionsController.Preview(SolutionId)).Result as ObjectResult;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()));

            return result.Value as SolutionResult;
        }
    }
}
