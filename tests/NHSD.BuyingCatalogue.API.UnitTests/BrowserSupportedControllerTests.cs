using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.API.Controllers;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetBrowsersSupported;
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
            var expected = new GetBrowsersSupportedResult(new ClientApplication());

            _mockMediator.Setup(m => m
                .Send(It.Is<GetBrowsersSupportedQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var result = (await _browserSupportedController.GetBrowsersSupportedAsync(SolutionId)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().Be(expected);

            _mockMediator.Verify(m => m.Send(It.Is<GetBrowsersSupportedQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            var result = (await _browserSupportedController.GetBrowsersSupportedAsync(SolutionId)) as NotFoundResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(m => m.Send(It.Is<GetBrowsersSupportedQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
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
