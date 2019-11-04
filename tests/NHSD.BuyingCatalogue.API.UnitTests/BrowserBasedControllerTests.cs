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
using NHSD.BuyingCatalogue.Application.Solutions.Domain;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.UnitTests
{
    [TestFixture]
    public sealed class BrowserBasedControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private BrowserBasedController _browserBasedController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _browserBasedController = new BrowserBasedController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            var result = (await _browserBasedController.GetBrowserBasedAsync(SolutionId)) as NotFoundResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task ShouldGetBrowserBasedStaticData()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(new Solution());

            browserBasedResult.Should().NotBeNull();
            browserBasedResult.BrowserBasedDashboardSections.Should().NotBeNull();

            var browsersSupportedSection = browserBasedResult.BrowserBasedDashboardSections.BrowsersSupportedSection;
            browsersSupportedSection.Requirement.Should().Be("Mandatory");
            browsersSupportedSection.Status.Should().Be("INCOMPLETE");

            var plugInsSection = browserBasedResult.BrowserBasedDashboardSections.PluginsOrExtensionsSection;
            plugInsSection.Requirement.Should().Be("Mandatory");
            plugInsSection.Status.Should().Be("INCOMPLETE");

            var connectivitySection = browserBasedResult.BrowserBasedDashboardSections.ConnectivityAndResolutionSection;
            connectivitySection.Requirement.Should().Be("Mandatory");
            connectivitySection.Status.Should().Be("INCOMPLETE");

            var hardwareSection = browserBasedResult.BrowserBasedDashboardSections.HardwareRequirementsSection;
            hardwareSection.Requirement.Should().Be("Optional");
            hardwareSection.Status.Should().Be("INCOMPLETE");

            var additionalSection = browserBasedResult.BrowserBasedDashboardSections.AdditionalInformationSection;
            additionalSection.Requirement.Should().Be("Optional");
            additionalSection.Status.Should().Be("INCOMPLETE");
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteNullClientApplication()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(new Solution());
            browserBasedResult.BrowserBasedDashboardSections.BrowsersSupportedSection.Status.Should().Be("INCOMPLETE");
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteNullBrowsersSupported()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(new Solution { ClientApplication = new ClientApplication()});
            browserBasedResult.BrowserBasedDashboardSections.BrowsersSupportedSection.Status.Should().Be("INCOMPLETE");
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteEmptyBrowsersSupported()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(new Solution
            {
                ClientApplication = new ClientApplication { BrowsersSupported = new HashSet<string>()}
            });
            browserBasedResult.BrowserBasedDashboardSections.BrowsersSupportedSection.Status.Should().Be("INCOMPLETE");
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteNullMobileResponsive()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(new Solution
            {
                ClientApplication = new ClientApplication { BrowsersSupported = new HashSet<string> { "A" } }
            });
            browserBasedResult.BrowserBasedDashboardSections.BrowsersSupportedSection.Status.Should().Be("INCOMPLETE");
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteBrowsersSupportedNullAndMobileResponsive()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(new Solution
            {
                ClientApplication = new ClientApplication
                {
                    MobileResponsive = false
                }
            });
            browserBasedResult.BrowserBasedDashboardSections.BrowsersSupportedSection.Status.Should().Be("INCOMPLETE");
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteBrowsersSupportedAndMobileResponsive()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(new Solution
            {
                ClientApplication = new ClientApplication
                {
                    BrowsersSupported = new HashSet<string> { "A" },
                    MobileResponsive = false
                }
            });
            browserBasedResult.BrowserBasedDashboardSections.BrowsersSupportedSection.Status.Should().Be("COMPLETE");
        }

        private async Task<BrowserBasedResult> GetBrowserBasedSectionAsync(Solution solution)
        {
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await _browserBasedController.GetBrowserBasedAsync(SolutionId)) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);

            return result.Value as BrowserBasedResult;
        }
    }
}
