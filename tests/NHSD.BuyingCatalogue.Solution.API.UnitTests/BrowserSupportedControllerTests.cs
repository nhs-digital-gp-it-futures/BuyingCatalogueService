using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Solutions;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowsersSupported;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solution.API.UnitTests
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
            _mockMediator.Setup(m => m
                    .Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Mock.Of<ISolution>(s =>
                        s.ClientApplication == Mock.Of<IClientApplication>(c =>
                            c.BrowsersSupported == new HashSet<string>{ "Chrome", "Edge" } &&
                            c.MobileResponsive == true)));

            var result = (await _browserSupportedController.GetBrowsersSupportedAsync(SolutionId)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var browsersSupported = (result.Value as GetBrowsersSupportedResult);

            browsersSupported.BrowsersSupported.Should().BeEquivalentTo(new string[] { "Chrome", "Edge" });
            browsersSupported.MobileResponsive.Should().Be("Yes");

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldGetEmptyBrowsersSupported()
        {
            _mockMediator.Setup(m => m
                    .Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Mock.Of<ISolution>(s =>
                        s.ClientApplication == Mock.Of<IClientApplication>(c =>
                            c.MobileResponsive == true)));

            var result = (await _browserSupportedController.GetBrowsersSupportedAsync(SolutionId)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var browsersSupported = (result.Value as GetBrowsersSupportedResult);

            browsersSupported.BrowsersSupported.Should().BeEmpty();
            browsersSupported.MobileResponsive.Should().Be("Yes");

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCase(null, null)]
        [TestCase(true, "Yes")]
        [TestCase(false, "No")]
        public async Task ShouldGetMobileResponsive(bool? mobileResponsive, string expectedMobileReponsive)
        {
            _mockMediator.Setup(m => m
                    .Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Mock.Of<ISolution>(s =>
                        s.ClientApplication == Mock.Of<IClientApplication>(c =>
                            c.MobileResponsive == mobileResponsive)));

            var result = (await _browserSupportedController.GetBrowsersSupportedAsync(SolutionId)) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var browsersSupported = (result.Value as GetBrowsersSupportedResult);
            browsersSupported.MobileResponsive.Should().Be(expectedMobileReponsive);

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldGetClientApplicationIsNull()
        {
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<ISolution>(s =>
                    s.ClientApplication == null));

            var result = (await _browserSupportedController.GetBrowsersSupportedAsync(SolutionId)) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var browsersSupported = (result.Value as GetBrowsersSupportedResult);
            browsersSupported.MobileResponsive.Should().BeNull();
            browsersSupported.BrowsersSupported.Count().Should().Be(0);
        }


        [Test]
        public async Task ShouldReturnNotFound()
        {
            var result = (await _browserSupportedController.GetBrowsersSupportedAsync(SolutionId)) as NotFoundResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var browsersSupportedUpdateViewModel = new UpdateSolutionBrowsersSupportedViewModel();

            var validationModel = new UpdateSolutionBrowserSupportedValidationResult();

            _mockMediator.Setup(m =>
                    m.Send(
                        It.Is<UpdateSolutionBrowsersSupportedCommand>(q =>
                            q.SolutionId == SolutionId && q.UpdateSolutionBrowsersSupportedViewModel ==
                            browsersSupportedUpdateViewModel), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel);

            var result =
                (await _browserSupportedController.UpdateBrowsersSupportedAsync(SolutionId,
                    browsersSupportedUpdateViewModel)) as NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateSolutionBrowsersSupportedCommand>(q =>
                        q.SolutionId == SolutionId && q.UpdateSolutionBrowsersSupportedViewModel ==
                        browsersSupportedUpdateViewModel), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var browsersSupportedUpdateViewModel = new UpdateSolutionBrowsersSupportedViewModel();

            var validationModel = new UpdateSolutionBrowserSupportedValidationResult()
            {
                Required = { "browsers-supported", "mobile-responsive"}
            };

            _mockMediator.Setup(m =>
                    m.Send(
                        It.Is<UpdateSolutionBrowsersSupportedCommand>(q =>
                            q.SolutionId == SolutionId && q.UpdateSolutionBrowsersSupportedViewModel ==
                            browsersSupportedUpdateViewModel), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel);

            var result =
                (await _browserSupportedController.UpdateBrowsersSupportedAsync(SolutionId,
                    browsersSupportedUpdateViewModel)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result.Value as UpdateSolutionBrowserSupportedResult).Required.Should().BeEquivalentTo(new [] { "browsers-supported", "mobile-responsive" });

            _mockMediator.Verify(m => m.Send(It.Is<UpdateSolutionBrowsersSupportedCommand>(q => q.SolutionId == SolutionId && q.UpdateSolutionBrowsersSupportedViewModel == browsersSupportedUpdateViewModel), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
