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
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class SolutionsControllerGetPreviewTests
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
        public async Task ShouldReturnEmpty()
        {
            var result = (await _solutionsController.Preview(SolutionId).ConfigureAwait(false)).Result as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            (result.Value as SolutionResult).Id.Should().BeNull();
        }

        [TestCase(null, null, null)]
        [TestCase("Sln2", null, null)]
        [TestCase(null, "name", null)]
        [TestCase(null, null, "organization")]
        [TestCase("Sln2", null, "organization")]
        [TestCase(null, "name", "organization")]
        [TestCase("Sln2", "name", "organization")]
        public async Task ShouldReturnGetValues(string id, string name, string organization)
        {
            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                s.Id == id &&
                s.Name == name &&
                s.OrganisationName == organization)).ConfigureAwait(false);

            previewResult.Id.Should().Be(id);
            previewResult.Name.Should().Be(name);
            previewResult.OrganisationName.Should().Be(organization);
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
            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                s.Summary == summary &&
                s.Description == description &&
                s.AboutUrl == link)).ConfigureAwait(false);

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

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s => s.Features == features)).ConfigureAwait(false);
            previewResult.Sections.Features.Should().BeNull();
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public async Task ShouldGetPreviewCalculateFeatures(bool isFeature, bool hasData)
        {
            var features = isFeature ? new List<string> { "Feature1", "Feature2" } : new List<string>();

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s => s.Features == features)).ConfigureAwait(false);

            if (hasData)
            {
                previewResult.Sections.Features.Answers.HasData.Should().BeTrue();
                previewResult.Sections.Features.Answers.Listing.Should()
                    .BeEquivalentTo(new List<string> { "Feature1", "Feature2" });

            }
            else
            {
                previewResult.Sections.Features.Should().BeNull();
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

            var previewResult = await GetSolutionPreviewSectionAsync(
                Mock.Of<ISolution>(s =>
                    s.ClientApplication == Mock.Of<IClientApplication>(c =>
                        c.ClientApplicationTypes == clientApplicationTypes &&
                        c.BrowsersSupported == browsersSupported &&
                        c.MobileResponsive == mobileResponsive))).ConfigureAwait(false);

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
            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" } &&
                    c.BrowsersSupported == new HashSet<string> { "Chrome", "Edge" } &&
                    c.MobileResponsive == true))).ConfigureAwait(false);

            previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers.SupportedBrowsers
                .Should().BeEquivalentTo(new HashSet<string> { "Chrome", "Edge" });
            previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers.MobileResponsive
                .Should().Be("Yes");
        }

        [Test]
        public async Task ShouldIncludeBrowserBasedDataIfClientApplicationTypesIncludePluginInformation()
        {
            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" }
                    && c.Plugins == Mock.Of<IPlugins>(p => p.Required == true && p.AdditionalInformation == "Plugin additional information")))).ConfigureAwait(false);

            previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.PluginOrExtensionsSection.Answers.Required
                .Should().Be("Yes");
            previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.PluginOrExtensionsSection.Answers.AdditionalInformation
                .Should().Be("Plugin additional information");
        }

        [Test]
        public async Task IfBrowserBasedThenHardwareRequirementsCanBeSet()
        {
            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" }
                    && c.HardwareRequirements == "New Hardware"))).ConfigureAwait(false);

            previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections
                .BrowserHardwareRequirementsSection.Answers.HardwareRequirements.Should().Be("New Hardware");
        }

        [Test]
        public async Task IfBrowserBasedThenAdditionalInformationCanBeSet()
        {
            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> {"browser-based", "native-mobile"} &&
                    c.AdditionalInformation == "Some Additional Info"))).ConfigureAwait(false);

            previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections
                .BrowserAdditionalInformationSection.Answers.AdditionalInformation.Should().Be("Some Additional Info");
        }

        [Test]
        public async Task ShouldNotIncludeBrowserBasedDataIfClientApplicationTypesDoNotIncludeBrowserBased()
        {
            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "native-desktop", "native-mobile" } &&
                    c.BrowsersSupported == new HashSet<string> { "Chrome", "Edge" } &&
                    c.MobileResponsive == true))).ConfigureAwait(false);

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
            var publicResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                s.PublishedStatus == PublishedStatus.Published &&
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "browser-based" } &&
                    c.MinimumConnectionSpeed == connectivity &&
                    c.MinimumDesktopResolution == resolution))).ConfigureAwait(false);
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
            var publicResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                    s.ClientApplication == Mock.Of<IClientApplication>(
                        c =>
                            c.ClientApplicationTypes == new HashSet<string> { "browser-based" }
                            && c.MobileFirstDesign == mobileFirst)))
                .ConfigureAwait(false);

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

            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                    s.ClientApplication ==
                    Mock.Of<IClientApplication>(c =>
                        c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" } &&
                        c.MobileOperatingSystems == Mock.Of<IMobileOperatingSystems>(m =>
                                                         m.OperatingSystems == operatingSystem &&
                                                         m.OperatingSystemsDescription == description))))
                .ConfigureAwait(false);

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
        public async Task MobileConnectionDetailsIsSetCorrectly(bool hasConnectionType, bool hasDescription, bool hasMinimumConnectionSpeed, bool hasData)
        {
            var connectionType = hasConnectionType ? new HashSet<string> { "3G", "4G" } : null;
            var description = hasDescription ? "I am a description" : null;
            var minimumConnectionSpeed = hasMinimumConnectionSpeed ? "1GBps" : null;
            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                    s.ClientApplication ==
                    Mock.Of<IClientApplication>(c =>
                        c.ClientApplicationTypes == new HashSet<string> { "native-mobile" } &&
                        c.MobileConnectionDetails == Mock.Of<IMobileConnectionDetails>(m =>
                            m.ConnectionType == connectionType &&
                            m.Description == description &&
                            m.MinimumConnectionSpeed == minimumConnectionSpeed))))
                .ConfigureAwait(false);

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
        public async Task MobileMemoryAndStorageIsSetCorrectly(string minMemoryManagement, string description,
            bool hasData)
        {
            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> {"native-mobile"} &&
                    c.MobileMemoryAndStorage == Mock.Of<IMobileMemoryAndStorage>(
                        m => m.MinimumMemoryRequirement == minMemoryManagement &&
                             m.Description == description)))).ConfigureAwait(false);

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
            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "native-mobile" }
                    && c.NativeMobileHardwareRequirements == requirements))).ConfigureAwait(false);

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
            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                    s.ClientApplication ==
                    Mock.Of<IClientApplication>(c =>
                        c.ClientApplicationTypes == new HashSet<string> { "native-mobile" } &&
                        c.MobileThirdParty == Mock.Of<IMobileThirdParty>(m =>
                            m.ThirdPartyComponents == component && m.DeviceCapabilities == capability))))
                .ConfigureAwait(false);

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
            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "native-mobile" }
                    && c.NativeMobileAdditionalInformation == additionalInformation))).ConfigureAwait(false);

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
            solution.OrganisationName.Should().BeNull();
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
            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "native-desktop" } &&
                    c.NativeDesktopHardwareRequirements == requirements
                    ))).ConfigureAwait(false);

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

        [TestCase("3 Mbps", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("      ", false)]
        public async Task IfNativeDesktopThenNativeConnectivityDetailsIsSetCorrectly(string minimumConnectionSpeed, bool hasData)
        {
            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "native-desktop" } &&
                    c.NativeDesktopMinimumConnectionSpeed == minimumConnectionSpeed
                ))).ConfigureAwait(false);

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

        [Test]
        public async Task ShouldIncludeNativeDesktopDataIfClientApplicationTypesIncludeNativeDesktop()
        {
            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "native-desktop" } &&
                    c.NativeDesktopHardwareRequirements == "Hardware requirements"
                    ))).ConfigureAwait(false);

            previewResult.Sections.ClientApplicationTypes.Sections.NativeDesktop.Sections.HardwareRequirementsSection
                .Answers.HardwareRequirements.Should().Be("Hardware requirements");
        }

        [Test]
        public async Task ShouldNotIncludeNativeDesktopDataIfClientApplicationTypesDoNotIncludeNativeDesktop()
        {
            var previewResult = await GetSolutionPreviewSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" } &&
                    c.NativeDesktopHardwareRequirements == "Hardware requirements")
                )).ConfigureAwait(false);

            previewResult.Sections.ClientApplicationTypes.Should().BeNull();
        }

        private async Task<SolutionResult> GetSolutionPreviewSectionAsync(ISolution solution)
        {
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await _solutionsController.Preview(SolutionId).ConfigureAwait(false)).Result as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);

            return result.Value as SolutionResult;
        }
    }
}
