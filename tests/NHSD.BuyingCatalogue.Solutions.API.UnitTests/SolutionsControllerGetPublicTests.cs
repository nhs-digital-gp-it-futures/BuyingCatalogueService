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
    public sealed class SolutionsControllerGetPublicTests
    {
        private Mock<IMediator> _mockMediator;

        private SolutionsController _solutionsController;

        private const string SolutionId1 = "Sln1";
        private const string SolutionId2 = "Sln2";

        private readonly DateTime _lastUpdated = DateTime.Today;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _solutionsController = new SolutionsController(_mockMediator.Object);
        }

        [Test]
        public async Task NoSolutionShouldReturnNotFound()
        {
            var result = (await _solutionsController.Public(SolutionId1).ConfigureAwait(false)).Result as NotFoundResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId1), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UnpublishedSolutionShouldReturnNotFound()
        {
            var solution = Mock.Of<ISolution>(
                s => s.Id == SolutionId1 &&
                     s.PublishedStatus == PublishedStatus.Draft);

            _mockMediator
                .Setup(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await _solutionsController.Public(SolutionId1).ConfigureAwait(false)).Result as NotFoundResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId1), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCase(null, null, null, false)]
        [TestCase("Sln2", null, null, false)]
        [TestCase(null, "Bob", "Org1", false)]
        [TestCase("Sln1", "Bob", "Org1", false)]
        [TestCase("Sln1", null, "Org1", false)]
        [TestCase("Sln1", "Bob", "Org1", false)]
        [TestCase("Sln1", "Bob", "Org1", true)]
        public async Task ShouldReturnGetValues(string id, string name, string organisationName, bool isFoundation)
        {
            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(
                s => s.Id == id &&
                     s.Name == name &&
                     s.OrganisationName == organisationName &&
                     s.IsFoundation == isFoundation &&
                     s.LastUpdated == _lastUpdated &&
                     s.PublishedStatus == PublishedStatus.Published), SolutionId1).ConfigureAwait(false);
            publicResult.Id.Should().Be(id);
            publicResult.Name.Should().Be(name);
            publicResult.OrganisationName.Should().Be(organisationName);
            publicResult.IsFoundation.Should().Be(isFoundation);
            publicResult.LastUpdated.Should().Be(_lastUpdated);
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
            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.Id == SolutionId1 &&
                s.Summary == summary &&
                s.Description == description &&
                s.AboutUrl == link &&
                s.PublishedStatus == PublishedStatus.Published), SolutionId1).ConfigureAwait(false);

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
            var feature = hasFeature ? new List<string>() { "feature1", "feature2" } : new List<string>();

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.Id == SolutionId1 &&
                s.Features == feature &&
                s.PublishedStatus == PublishedStatus.Published), SolutionId1).ConfigureAwait(false);

            publicResult.Id.Should().Be(SolutionId1);

            if (hasFeature)
            {
                publicResult.Sections.Features.Answers.Listing.Should()
                    .BeEquivalentTo(new List<string>() { "feature1", "feature2" });
            }
            else
            {
                publicResult.Sections.Features.Should().BeNull();
            }
        }

        [Test]
        public async Task ShouldCheckForNullClientApplicationTypes()
        {
            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == null &&
                s.PublishedStatus == PublishedStatus.Published), SolutionId1).ConfigureAwait(false);

            publicResult.Sections.ClientApplicationTypes.Should().BeNull();
        }


        [TestCase(false, false, null, false)]
        [TestCase(true, false, null, false)]
        [TestCase(true, true, null, true)]
        [TestCase(true, false, false, true)]
        [TestCase(true, false, true, true)]
        [TestCase(true, true, false, true)]
        [TestCase(true, true, true, true)]

        public async Task ShouldGetPublicCalculateClientApplication(bool isClientApplication, bool isBrowserSupported,
            bool? mobileResponsive, bool expectData)
        {
            var clientApplicationTypes = isClientApplication
                ? new HashSet<string> { "browser-based", "native-mobile" }
                : new HashSet<string>();
            var browsersSupported = isBrowserSupported ? new HashSet<string> { "Chrome", "Edge" } : new HashSet<string>();

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == clientApplicationTypes &&
                    c.BrowsersSupported == browsersSupported &&
                    c.MobileResponsive == mobileResponsive) &&
                s.PublishedStatus == PublishedStatus.Published), SolutionId1).ConfigureAwait(false);
            if (expectData)
            {
                publicResult.Sections.ClientApplicationTypes.Sections.HasData.Should().Be(true);

                if (mobileResponsive.HasValue)
                {
                    publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers.MobileResponsive
                        .Should()
                        .Be(mobileResponsive.GetValueOrDefault() ? "Yes" : "No");
                }
                else
                {
                    publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers
                        .MobileResponsive.Should().BeNull();
                }

                if (isBrowserSupported)
                {
                    publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers.SupportedBrowsers
                        .Should().BeEquivalentTo(isClientApplication
                            ? new HashSet<string> { "Chrome", "Edge" }
                            : new HashSet<string>());
                }
                else
                {
                    publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers
                        .SupportedBrowsers.Should().BeNull();
                }
            }
            else
            {
                publicResult.Sections.ClientApplicationTypes.Should().BeNull();
            }
        }


        [Test]
        public async Task ShouldIncludeBrowserBasedDataIfClientApplicationTypesIncludeBrowserBased()
        {
            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" } &&
                    c.BrowsersSupported == new HashSet<string> { "Chrome", "Edge" } &&
                    c.MobileResponsive == true) &&
                s.PublishedStatus == PublishedStatus.Published), SolutionId1).ConfigureAwait(false);

            publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers.SupportedBrowsers
                .Should().BeEquivalentTo(new HashSet<string> { "Chrome", "Edge" });
            publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers.MobileResponsive
                .Should().Be("Yes");
        }

        [Test]
        public async Task ShouldIncludeBrowserBasedDataIfClientApplicationTypesIncludePluginInformation()
        {
            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" }
                    && c.Plugins == Mock.Of<IPlugins>(p => p.Required == true && p.AdditionalInformation == "Plugin additional information")) &&
                s.PublishedStatus == PublishedStatus.Published), SolutionId1).ConfigureAwait(false);

            publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.PluginOrExtensionsSection.Answers.Required
                .Should().Be("Yes");
            publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.PluginOrExtensionsSection.Answers.AdditionalInformation
                .Should().Be("Plugin additional information");
        }

        [Test]
        public async Task IfBrowserBasedThenHardwareRequirementsCanBeSet()
        {
            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" }
                    && c.HardwareRequirements == "New Hardware") &&
                s.PublishedStatus == PublishedStatus.Published), SolutionId1).ConfigureAwait(false);

            publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections
                .BrowserHardwareRequirementsSection.Answers.HardwareRequirements.Should().Be("New Hardware");
        }

        [Test]
        public async Task IfBrowserBasedThenAdditionalInformationCanBeSet()
        {
            var previewResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.PublishedStatus == PublishedStatus.Published &&
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" } &&
                    c.AdditionalInformation == "Some Additional Info")), SolutionId1).ConfigureAwait(false);

            previewResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections
                .BrowserAdditionalInformationSection.Answers.AdditionalInformation.Should().Be("Some Additional Info");
        }

        [Test]
        public async Task ShouldNotIncludeBrowserBasedDataIfClientApplicationTypesDoNotIncludeBrowserBased()
        {
            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "native-desktop", "native-mobile" } &&
                    c.BrowsersSupported == new HashSet<string> { "Chrome", "Edge" } &&
                    c.MobileResponsive == true) &&
                s.PublishedStatus == PublishedStatus.Published), SolutionId1).ConfigureAwait(false);

            publicResult.Sections.ClientApplicationTypes.Should().BeNull();
        }

        [Test]
        public async Task CapabilitiesIsNullForSolution()
        {
            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.Id == SolutionId1 &&
                s.Capabilities == null &&
                s.PublishedStatus == PublishedStatus.Published), SolutionId1).ConfigureAwait(false);

            publicResult.Id.Should().Be(SolutionId1);
            publicResult.Sections.Capabilities.Answers.CapabilitiesMet.Should().BeEquivalentTo(new List<string>());
        }


        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldGetCapabilitiesOnlyForSolution(bool hasCapability)
        {
            var capabilities = hasCapability ? new List<string>() { "cap1", "cap2" } : new List<string>();

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.Id == SolutionId1 &&
                s.Capabilities == capabilities &&
                s.PublishedStatus == PublishedStatus.Published), SolutionId1).ConfigureAwait(false);

            publicResult.Id.Should().Be(SolutionId1);
            publicResult.Sections.Capabilities.Answers.CapabilitiesMet.Should().ContainInOrder(hasCapability ? new List<string> { "cap1", "cap2" } : new List<string>());
        }

        [Test]
        public async Task MultipleCapabilitiesForDifferentSolutions()
        {
            var publicResult1 = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.Id == SolutionId1 &&
                s.Capabilities == new List<string>() { "cap1", "cap2" } &&
                s.PublishedStatus == PublishedStatus.Published), SolutionId1).ConfigureAwait(false);

            var publicResult2 = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.Id == SolutionId2 &&
                s.Capabilities == new List<string>() { "cap3", "cap4", "cap5" } &&
                s.PublishedStatus == PublishedStatus.Published), SolutionId2).ConfigureAwait(false);

            publicResult1.Id.Should().Be(SolutionId1);
            publicResult1.Sections.Capabilities.Answers.CapabilitiesMet.Should()
                .ContainInOrder(new List<string>() { "cap1", "cap2" });

            publicResult2.Id.Should().Be(SolutionId2);
            publicResult2.Sections.Capabilities.Answers.CapabilitiesMet.Should()
                .ContainInOrder(new List<string>() { "cap3", "cap4", "cap5" });
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
            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.PublishedStatus == PublishedStatus.Published &&
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "browser-based" } &&
                    c.MinimumConnectionSpeed == connectivity &&
                    c.MinimumDesktopResolution == resolution)), SolutionId1).ConfigureAwait(false);
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

        [TestCase(false, null, false)]
        [TestCase(false, "Desc", false)]
        [TestCase(true, null, true)]
        [TestCase(true, "Desc", true)]
        public async Task MobileOperatingSystemsIsSetCorrectly(bool isOperatingSystem, string description, bool hasData)
        {
            var operatingSystem = isOperatingSystem ? new HashSet<string> { "IOS", "Windows" } : new HashSet<string>();

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                    s.PublishedStatus == PublishedStatus.Published &&
                    s.ClientApplication ==
                    Mock.Of<IClientApplication>(c =>
                        c.ClientApplicationTypes == new HashSet<string> {"native-mobile"} &&
                        c.MobileOperatingSystems == Mock.Of<IMobileOperatingSystems>(m =>
                            m.OperatingSystems == operatingSystem &&
                            m.OperatingSystemsDescription == description))), SolutionId1)
                .ConfigureAwait(false);

            var operatingSystemResult = publicResult?.Sections?.ClientApplicationTypes?.Sections?.NativeMobile
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
            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                    s.Id == SolutionId1 &&
                    s.PublishedStatus == PublishedStatus.Published &&
                    s.ClientApplication ==
                    Mock.Of<IClientApplication>(c =>
                        c.ClientApplicationTypes == new HashSet<string> { "native-mobile" } &&
                        c.MobileConnectionDetails == Mock.Of<IMobileConnectionDetails>(m =>
                            m.ConnectionType == connectionType &&
                            m.Description == description &&
                            m.MinimumConnectionSpeed == minimumConnectionSpeed))), SolutionId1)
                .ConfigureAwait(false);

            var mobileConnectionDetailsResult = publicResult?.Sections?.ClientApplicationTypes?.Sections?.NativeMobile
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
            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.PublishedStatus == PublishedStatus.Published &&
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "native-mobile" } &&
                    c.MobileMemoryAndStorage == Mock.Of<IMobileMemoryAndStorage>(
                        m => m.MinimumMemoryRequirement == minMemoryManagement &&
                             m.Description == description))), SolutionId1).ConfigureAwait(false);

            var mobileMemoryStorageResult = publicResult?.Sections?.ClientApplicationTypes?.Sections?.NativeMobile
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

        [TestCase(null, null)]
        [TestCase(false, "No")]
        [TestCase(true, "Yes")]
        public async Task BrowserMobileFirstIsSetCorrectly(bool? mobileFirst, string result)
        {
            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.Id == SolutionId1 &&
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "browser-based" } &&
                    c.MobileFirstDesign == mobileFirst) &&
                s.PublishedStatus == PublishedStatus.Published), SolutionId1).ConfigureAwait(false);

            var mobileFirstSection = publicResult?.Sections?.ClientApplicationTypes?.Sections?.BrowserBased?.Sections
                ?.BrowserMobileFirstSection;

            if (result == null)
            {
                mobileFirstSection.Should().BeNull();
                return;
            }

            mobileFirstSection.Answers.MobileFirstDesign.Should().Be(result);
        }


        [Test]
        public async Task ShouldGetContacts()
        {
            var contacts = new List<IContact>
            {
                Mock.Of<IContact>(m => m.Name == "name1" && m.Department == "dep1" && m.Email == "test@gmail.com" && m.PhoneNumber == "01234567890"),
                Mock.Of<IContact>(m => m.Name == "name2" && m.Department == "dep2" && m.Email == "test2@gmail.com" && m.PhoneNumber == "12345678901")
            };

            var contact = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.Id == SolutionId1 &&
                s.Contacts == contacts &&
                s.PublishedStatus == PublishedStatus.Published), SolutionId1).ConfigureAwait(false);

            contact.Id.Should().Be(SolutionId1);

            contact.Sections.ContactDetails.Answers.Contact1.ContactName.Should().BeEquivalentTo(contacts[0].Name);
            contact.Sections.ContactDetails.Answers.Contact1.DepartmentName.Should().BeEquivalentTo(contacts[0].Department);
            contact.Sections.ContactDetails.Answers.Contact1.EmailAddress.Should().BeEquivalentTo(contacts[0].Email);
            contact.Sections.ContactDetails.Answers.Contact1.PhoneNumber.Should().BeEquivalentTo(contacts[0].PhoneNumber);

            contact.Sections.ContactDetails.Answers.Contact2.ContactName.Should().BeEquivalentTo(contacts[1].Name);
            contact.Sections.ContactDetails.Answers.Contact2.DepartmentName.Should().BeEquivalentTo(contacts[1].Department);
            contact.Sections.ContactDetails.Answers.Contact2.EmailAddress.Should().BeEquivalentTo(contacts[1].Email);
            contact.Sections.ContactDetails.Answers.Contact2.PhoneNumber.Should().BeEquivalentTo(contacts[1].PhoneNumber);
        }

        [Test]
        public async Task EmptyContactShouldReturnNoData()
        {
            var contacts = new List<IContact>
            {
                Mock.Of<IContact>(m =>
                    m.Name == null && m.Department == "" && m.Email == "" && m.PhoneNumber == "   "),
                Mock.Of<IContact>(m => m.Name == "" && m.Department == "" && m.Email == "" && m.PhoneNumber == "")
            };

            var contact = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.Id == SolutionId1 &&
                s.Contacts == contacts &&
                s.PublishedStatus == PublishedStatus.Published), SolutionId1).ConfigureAwait(false);

            contact.Id.Should().Be(SolutionId1);
            contact.Sections.ContactDetails.Should().BeNull();
        }

        [Test]
        public async Task SingleContactShouldReturnNullForEmptyData()
        {
            var contacts = new List<IContact>
            {
                Mock.Of<IContact>(m =>
                    m.Name == "Hello" && m.Department == "" && m.Email == "" && m.PhoneNumber == "   "),
                Mock.Of<IContact>(m => m.Name == "" && m.Department == "" && m.Email == "" && m.PhoneNumber == "")
            };

            var contact = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.Id == SolutionId1 &&
                s.Contacts == contacts &&
                s.PublishedStatus == PublishedStatus.Published), SolutionId1).ConfigureAwait(false);

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
        public void NullSolutionShouldThrowNullExceptionPublicResult()
        {
            var solution = new SolutionResult(null);
            solution.Id.Should().BeNull();
            solution.Name.Should().BeNull();
            solution.OrganisationName.Should().BeNull();
            solution.LastUpdated.Should().BeNull();
            solution.IsFoundation.Should().BeNull();

            solution.Sections.Should().BeNull();
        }


        [Test]
        public void NullSolutionShouldThrowNullExceptionSolutionDescriptionPublicAnswers()
        {
            Assert.Throws<ArgumentNullException>(() => new SolutionDescriptionSectionAnswers(null));
        }

        private async Task<SolutionResult> GetSolutionPublicResultAsync(ISolution solution, string solutionId)
        {
            _mockMediator
                .Setup(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == solutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await _solutionsController.Public(solutionId).ConfigureAwait(false)).Result as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == solutionId), It.IsAny<CancellationToken>()), Times.Once);

            return result.Value as SolutionResult;
        }
    }
}
