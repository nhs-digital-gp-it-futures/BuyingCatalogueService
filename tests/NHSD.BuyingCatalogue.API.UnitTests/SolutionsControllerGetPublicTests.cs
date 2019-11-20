using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.API.Controllers;
using NHSD.BuyingCatalogue.API.ViewModels.Public;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Contracts.Solutions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.UnitTests
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
        public async Task ShouldReturnNotFound()
        {
            var result = (await _solutionsController.Public(SolutionId1)).Result as NotFoundResult;

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
                     s.LastUpdated == _lastUpdated), SolutionId1);
            publicResult.Id.Should().Be(id);
            publicResult.Name.Should().Be(name);
            publicResult.OrganisationName.Should().Be(organisationName);
            publicResult.IsFoundation.Should().Be(isFoundation);
            publicResult.LastUpdated.Should().Be(_lastUpdated.ToString("dd-MMM-yyyy"));
        }

        [TestCase(null,null,null)]
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
                s.AboutUrl == link), SolutionId1);

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
            var feature = hasFeature ? new List<string>() {"feature1", "feature2"} : new List<string>();

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.Id == SolutionId1 &&
                s.Features == feature), SolutionId1);

            publicResult.Id.Should().Be(SolutionId1);

            if (hasFeature)
            {
                publicResult.Sections.Features.Answers.Listing.Should()
                    .BeEquivalentTo(new List<string>() {"feature1", "feature2"});
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
                s.ClientApplication == null), SolutionId1);

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
                ? new HashSet<string> {"browser-based", "native-mobile"}
                : new HashSet<string>();
            var browsersSupported = isBrowserSupported ? new HashSet<string> {"Chrome", "Edge"} : new HashSet<string>();

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == clientApplicationTypes &&
                    c.BrowsersSupported == browsersSupported &&
                    c.MobileResponsive == mobileResponsive)), SolutionId1);
            if (expectData)
            {
                publicResult.Sections.ClientApplicationTypes.Sections.HasData.Should().Be(true);

                if (mobileResponsive.HasValue)
                {
                    publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers.MobileResponsive
                        .Should()
                        .Be(mobileResponsive.GetValueOrDefault() ? "yes" : "no");
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
                        .SupportedBrowsers.Should().BeEmpty();
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
                    c.MobileResponsive == true)), SolutionId1);

            publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers.SupportedBrowsers
                .Should().BeEquivalentTo(new HashSet<string> { "Chrome", "Edge" });
            publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.BrowsersSupported.Answers.MobileResponsive
                .Should().Be("yes");
        }


        [Test]
        public async Task ShouldIncludeBrowserBasedDataIfClientApplicationTypesIncludePluginInformation()
        {
            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "browser-based", "native-mobile" }
                    && c.Plugins == new PluginsDto { Required = true, AdditionalInformation = "Plugin additional information" })), SolutionId1);

            publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.PluginOrExtensionsSection.Answers.Required
                .Should().Be("yes");
            publicResult.Sections.ClientApplicationTypes.Sections.BrowserBased.Sections.PluginOrExtensionsSection.Answers.AdditionalInformation
                .Should().Be("Plugin additional information");
        }

        [Test]
        public async Task ShouldNotIncludeBrowserBasedDataIfClientApplicationTypesDoNotIncludeBrowserBased()
        {
            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.ClientApplicationTypes == new HashSet<string> { "native-desktop", "native-mobile" } &&
                    c.BrowsersSupported == new HashSet<string> { "Chrome", "Edge" } &&
                    c.MobileResponsive == true)), SolutionId1);

            publicResult.Sections.ClientApplicationTypes.Should().BeNull();
        }

        [Test]
        public async Task CapabilitiesIsNullForSolution()
        {
            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.Id == SolutionId1 &&
                s.Capabilities == null), SolutionId1);

            publicResult.Id.Should().Be(SolutionId1);
            publicResult.Sections.Capabilities.Answers.CapabilitiesMet.Should().BeEquivalentTo(new List<string>());
        }


        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldGetCapabilitiesOnlyForSolution(bool hasCapability)
        {
            var capabilities = hasCapability ? new List<string>() {"cap1", "cap2"} : new List<string>(); 

            var publicResult = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.Id == SolutionId1 &&
                s.Capabilities == capabilities), SolutionId1);

            publicResult.Id.Should().Be(SolutionId1);
            publicResult.Sections.Capabilities.Answers.CapabilitiesMet.Should().ContainInOrder(hasCapability ? new List<string>{ "cap1", "cap2" } : new List<string>());
        }

        [Test]
        public async Task MultipleCapabilitiesForDifferentSolutions()
        {
            var publicResult1 = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.Id == SolutionId1 &&
                s.Capabilities == new List<string>() {"cap1", "cap2"}), SolutionId1);

            var publicResult2 = await GetSolutionPublicResultAsync(Mock.Of<ISolution>(s =>
                s.Id == SolutionId2 &&
                s.Capabilities == new List<string>() {"cap3", "cap4", "cap5"}), SolutionId2);

            publicResult1.Id.Should().Be(SolutionId1);
            publicResult1.Sections.Capabilities.Answers.CapabilitiesMet.Should()
                .ContainInOrder(new List<string>() {"cap1", "cap2"});

            publicResult2.Id.Should().Be(SolutionId2);
            publicResult2.Sections.Capabilities.Answers.CapabilitiesMet.Should()
                .ContainInOrder(new List<string>() {"cap3", "cap4", "cap5"});
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
                s.Contacts == contacts), SolutionId1);

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
        public void NullSolutionShouldThrowNullExceptionPublicResult()
        {
            Assert.Throws<ArgumentNullException>(() => new SolutionPublicResult(null));
        }

        [Test]
        public void NullSolutionShouldThrowNullExceptionPublicSections()
        {
            Assert.Throws<ArgumentNullException>(() => new PublicSections(null));
        }

        [Test]
        public void NullSolutionShouldThrowNullExceptionSolutionDescriptionPublicAnswers()
        {
            Assert.Throws<ArgumentNullException>(() => new SolutionDescriptionPublicSectionAnswers(null));
        }
        
        private async Task<SolutionPublicResult> GetSolutionPublicResultAsync(ISolution solution, string solutionId)
        {
            _mockMediator
                .Setup(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == solutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await _solutionsController.Public(solutionId)).Result as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == solutionId), It.IsAny<CancellationToken>()), Times.Once);

            return result.Value as SolutionPublicResult;
        }
    }
}
