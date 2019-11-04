using System.Collections.Generic;
using System.Linq;
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
using NHSD.BuyingCatalogue.Application.Solutions.Domain;
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
            var a = dashboardResult.Sections.Should().HaveCount(3);

            var solutionDescriptionSection = dashboardResult.Sections.First(s => s.Id == "solution-description");
            solutionDescriptionSection.Requirement.Should().Be("Mandatory");

            var featuresSection = dashboardResult.Sections.First(s => s.Id == "features");
            featuresSection.Requirement.Should().Be("Optional");

            var clientApplicationTypesSection = dashboardResult.Sections.First(s => s.Id == "client-application-types");
            clientApplicationTypesSection.Requirement.Should().Be("Mandatory");
        }
        

        [TestCase("solution-description", "INCOMPLETE")]
        [TestCase("features", "INCOMPLETE")]
        [TestCase("client-application-types", "INCOMPLETE")]
        public async Task ShouldGetDashboardCalculateCompleteNull(string id, string result)
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(new Solution());
            dashboardResult.Sections.First(s => s.Id == id).Status.Should().Be(result);
        }

        [TestCase(null, "Desc", "", "INCOMPLETE")]
        [TestCase(null, "", "Link", "INCOMPLETE")]
        [TestCase(null, "Desc", "Link", "INCOMPLETE")]
        [TestCase("Summary", null, null, "COMPLETE")]
        [TestCase("Summary", "Desc", "", "COMPLETE")]
        [TestCase("Summary", "Desc", "Link", "COMPLETE")]

        public async Task ShouldGetDashboardCalculateSolutionDescription(string summary, string description, string link, string result)
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(new Solution(){Summary = summary, Description = description, AboutUrl = link});
            dashboardResult.Sections.First(s => s.Id == "solution-description").Status.Should().Be(result);
        }

        [TestCase(false, "INCOMPLETE")]
        [TestCase(true, "COMPLETE")]
        public async Task ShouldGetDashboardCalculateCompleteFeatures(bool isFeatures, string result)
        {
            var features = isFeatures ? new[] { "Feature1", "Feature2" } : new string[0];
            var dashboardResult = await GetSolutionDashboardSectionAsync(new Solution() { Features = features });
            dashboardResult.Sections.First(s => s.Id == "features").Status.Should().Be(result);
        }

        [TestCase("browser-based", "INCOMPLETE")]
        [TestCase("native-mobile", "INCOMPLETE")]
        [TestCase("native-desktop", "INCOMPLETE")]
        public async Task ShouldGetDashboardCalculateClientApplicationTypesNull(string applicationType, string result)
        {
            var dashboardResult = await GetSolutionDashboardSectionAsync(new Solution()
            {
                ClientApplication = new ClientApplication()
                {
                    ClientApplicationTypes = new HashSet<string>()
                    {
                        applicationType
                    }
                }
            });
            dashboardResult.Sections.First(s => s.Id == "client-application-types").Sections
                .First(x => x.Id == applicationType).Status.Should().Be(result);
        }

        [TestCase(false, null, "INCOMPLETE")]
        [TestCase(false, false, "INCOMPLETE")]
        [TestCase(false, true, "INCOMPLETE")]
        [TestCase(true, false, "COMPLETE")]
        [TestCase(true, null, "COMPLETE")]
        [TestCase(true, true, "COMPLETE")]
        public async Task ShouldGetDashboardCalculateClientApplicationTypeBrowserBased(bool someBrowsersSupported, bool? mobileResponsive, string result)
        {
            var browsersSupported = someBrowsersSupported ? new HashSet<string>() {"Edge", "Chrome"} : new HashSet<string>();

            var dashboardResult = await GetSolutionDashboardSectionAsync(new Solution()
            {
                ClientApplication = new ClientApplication()
                {
                    ClientApplicationTypes = new HashSet<string>()
                    {
                        "browser-based"
                    },
                    BrowsersSupported = browsersSupported,
                    MobileResponsive = mobileResponsive
                }
            });
            dashboardResult.Sections.First(s => s.Id == "client-application-types").Sections
                .First(x => x.Id == "browser-based").Status.Should().Be(result);
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
