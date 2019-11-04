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
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.UnitTests
{
    [TestFixture]
    public sealed class BrowserSupportedControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private BrowsersSupportedController _browserSupportedController;

        private const string SolutionId = "Sln1";
        
        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _browserSupportedController = new BrowsersSupportedController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldGetBrowsersSupported()
        {
            var solution = new Solution
            {
                ClientApplication = new ClientApplication
                {
                    BrowsersSupported = new HashSet<string>(new string[] { "Chrome", "Edge" }),
                    MobileResponsive = true
                }
            };

            _mockMediator.Setup(m => m
                    .Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await _browserSupportedController.GetBrowsersSupportedAsync(SolutionId)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var browsersSupported = (result.Value as GetBrowsersSupportedResult);

            browsersSupported.BrowsersSupported.Should().BeEquivalentTo(new string[] { "Chrome", "Edge" });
            browsersSupported.MobileResponsive.Should().Be("yes");

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldGetEmptyBrowsersSupported()
        {
            var solution = new Solution
            {
                ClientApplication =  new ClientApplication
                {
                    MobileResponsive = true
                }
            };

            _mockMediator.Setup(m => m
                .Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await _browserSupportedController.GetBrowsersSupportedAsync(SolutionId)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var browsersSupported = (result.Value as GetBrowsersSupportedResult);

            browsersSupported.BrowsersSupported.Should().BeEmpty();
            browsersSupported.MobileResponsive.Should().Be("yes");

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCase(null, null)]
        [TestCase(true, "yes")]
        [TestCase(false, "no")]
        public async Task ShouldGetMobileResponsive(bool? mobileResponsive, string expectedMobileReponsive)
        {
            var solution = new Solution
            {
                ClientApplication = new ClientApplication
                {
                    MobileResponsive = mobileResponsive
                }
            };

            _mockMediator.Setup(m => m
                    .Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await _browserSupportedController.GetBrowsersSupportedAsync(SolutionId)) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var browsersSupported = (result.Value as GetBrowsersSupportedResult);
            browsersSupported.MobileResponsive.Should().Be(expectedMobileReponsive);

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            var result = (await _browserSupportedController.GetBrowsersSupportedAsync(SolutionId)) as NotFoundResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdate()
        {
            var browsersSupportedUpdateViewModel = new UpdateSolutionBrowsersSupportedViewModel();
            var result =
                (await _browserSupportedController.UpdateBrowsersSupportedAsync(SolutionId,
                    browsersSupportedUpdateViewModel)) as NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(m => m.Send(It.Is<UpdateSolutionBrowsersSupportedCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionBrowsersSupportedViewModel == browsersSupportedUpdateViewModel), It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}