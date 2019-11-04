using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.API.Controllers;
using NHSD.BuyingCatalogue.API.ViewModels;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.UnitTests
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
            var result = (await _solutionsController.Dashboard(SolutionId)).Result as NotFoundResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(
                        m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCase(null, null)]
        [TestCase("Sln2", null)]
        [TestCase(null, "Bob")]
        [TestCase("Sln2", "Bob")]
        public async Task ShouldReturnNameId(string id, string name)
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(new Solution()
            {
                Id = id,
                Name = name
            });

            dashboardResult.Id.Should().Be(id);
            dashboardResult.Name.Should().Be(name);
        }

        [Test]
        public async Task ShouldReturnSolutionDashboardStaticData()
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(new Solution());
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
            var dashboardResult = await GetSolutionDashboardSectionAsync(new Solution());
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
            var dashboardResult = await GetSolutionDashboardSectionAsync(new Solution {Summary = summary, Description = description, AboutUrl = link});
            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.SolutionDescriptionSection.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.SolutionDescriptionSection.Status.Should().Be(result);
        }

        [TestCase(false, "INCOMPLETE")]
        [TestCase(true, "COMPLETE")]
        public async Task ShouldGetDashboardCalculateCompleteFeatures(bool isFeatures, string result)
        {
            var features = isFeatures ? new[] { "Feature1", "Feature2" } : new string[0];
            var dashboardResult = await GetSolutionDashboardSectionAsync(new Solution { Features = features });
            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.FeaturesSection.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.FeaturesSection.Status.Should().Be(result);
        }

        [Test]
        public async Task ShouldGetDashboardCalculateClientApplicationTypesBrowserBasedNull()
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(new Solution
            {
                ClientApplication = new ClientApplication
                {
                    ClientApplicationTypes = new HashSet<string>
                    {
                        "browser-based"
                    }
                }
            });

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section.Should().BeOfType<ClientApplicationTypesSubSections>();
            var clientApplicationTypesSubSections = (ClientApplicationTypesSubSections) dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section;
            clientApplicationTypesSubSections.BrowserBasedSection.Should().NotBeNull();
            clientApplicationTypesSubSections.BrowserBasedSection.Status.Should().Be("INCOMPLETE");
        }

        [Test]
        public async Task ShouldGetDashboardCalculateClientApplicationTypesNativeMobileNull()
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(new Solution
            {
                ClientApplication = new ClientApplication
                {
                    ClientApplicationTypes = new HashSet<string>
                    {
                        "native-mobile"
                    }
                }
            });

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section.Should().BeOfType<ClientApplicationTypesSubSections>();
            var clientApplicationTypesSubSections = (ClientApplicationTypesSubSections) dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section;
            clientApplicationTypesSubSections.NativeMobileSection.Should().NotBeNull();
            clientApplicationTypesSubSections.NativeMobileSection.Status.Should().Be("INCOMPLETE");
        }

        [Test]
        public async Task ShouldGetDashboardCalculateClientApplicationTypesNativeDesktopNull()
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(new Solution
            {
                ClientApplication = new ClientApplication
                {
                    ClientApplicationTypes = new HashSet<string>
                    {
                        "native-desktop"
                    }
                }
            });

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section.Should().BeOfType<ClientApplicationTypesSubSections>();
            var clientApplicationTypesSubSections = (ClientApplicationTypesSubSections) dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section;
            clientApplicationTypesSubSections.NativeDesktopSection.Should().NotBeNull();
            clientApplicationTypesSubSections.NativeDesktopSection.Status.Should().Be("INCOMPLETE");
        }

        [TestCase(false, null, "INCOMPLETE")]
        [TestCase(false, false, "INCOMPLETE")]
        [TestCase(false, true, "INCOMPLETE")]
        [TestCase(true, false, "COMPLETE")]
        [TestCase(true, null, "INCOMPLETE")]
        [TestCase(true, true, "COMPLETE")]
        public async Task ShouldGetDashboardCalculateClientApplicationTypeBrowserBased(bool someBrowsersSupported, bool? mobileResponsive, string result)
        {
            var browsersSupported = someBrowsersSupported ? new HashSet<string> {"Edge", "Chrome"} : new HashSet<string>();

            var dashboardResult = await GetSolutionDashboardSectionAsync(new Solution
            {
                ClientApplication = new ClientApplication
                {
                    ClientApplicationTypes = new HashSet<string>
                    {
                        "browser-based"
                    },
                    BrowsersSupported = browsersSupported,
                    MobileResponsive = mobileResponsive
                }
            });

            dashboardResult.SolutionDashboardSections.Should().NotBeNull();
            dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section.Should().BeOfType<ClientApplicationTypesSubSections>();
            var clientApplicationTypesSubSections = (ClientApplicationTypesSubSections) dashboardResult.SolutionDashboardSections.ClientApplicationTypesSection.Section;
            clientApplicationTypesSubSections.BrowserBasedSection.Should().NotBeNull();
            clientApplicationTypesSubSections.BrowserBasedSection.Status.Should().Be(result);
        }

        private async Task<SolutionDashboardResult> GetSolutionDashboardSectionAsync(Solution solution)
        {
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);


            var result = (await _solutionsController.Dashboard(SolutionId)).Result as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);

            return result.Value as SolutionDashboardResult;
        }
    }
}
