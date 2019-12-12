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
        public async Task ShouldReturnNotFound()
        {
            var result = (await _solutionsController.Preview(SolutionId).ConfigureAwait(false)).Result as NotFoundResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCase(null, null, null)]
        [TestCase("Sln2", null, null)]
        [TestCase(null, "name", null)]
        [TestCase(null, null, "organization")]
        [TestCase("Sln2", null, "organization")]
        [TestCase(null, "name", "organization")]
        [TestCase("Sln2", "name", "organization")]
        public async Task ShouldReturGetValues(string id, string name, string organization)
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
        [Test]
        public void NullSolutionShouldThrowNullExceptionSolutionDescriptionPreviewAnswers()
        {
            Assert.Throws<ArgumentNullException>(() => new SolutionDescriptionSectionAnswers(null));
        }

        [Test]
        public void NullSolutionShouldThrowNullExceptionPreviewSections()
        {
            Assert.Throws<ArgumentNullException>(() => new SolutionResult(null));
        }

        [Test]
        public void NullSolutionShouldThrowNullExceptionPreviewResult()
        {
            Assert.Throws<ArgumentNullException>(() => new SolutionResult(null));
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
