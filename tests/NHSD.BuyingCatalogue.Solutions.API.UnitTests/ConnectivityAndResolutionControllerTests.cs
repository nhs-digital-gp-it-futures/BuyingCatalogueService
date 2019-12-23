using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionConnectivityAndResolution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class ConnectivityAndResolutionControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private ConnectivityAndResolutionController _controller;
        private const string SolutionId = "Sln1";
        private UpdateSolutionConnectivityAndResolutionViewModel _viewModel;
        private RequiredResult _validationResult;
        private IClientApplication _clientApplication;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _mockMediator.Setup(x => x.Send(It.Is<UpdateSolutionConnectivityAndResolutionCommand>(command =>
                    command.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _validationResult);
            _clientApplication = Mock.Of<IClientApplication>(c => c.MinimumConnectionSpeed == "1 PPH" && c.MinimumDesktopResolution == "1x1");
            _mockMediator.Setup(x => x.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _clientApplication);
            _controller = new ConnectivityAndResolutionController(_mockMediator.Object);
            _validationResult = new RequiredResult();
            _viewModel = new UpdateSolutionConnectivityAndResolutionViewModel { MinimumConnectionSpeed = "1 PPH (Pigeon Per Hour)", MinimumDesktopResolution = "1x1" };
        }

        [Test]
        public async Task ValidResultForCommandReturnsNoContent()
        {
            var result = await _controller.UpdateConnectivityAndResolutionAsync(SolutionId, _viewModel).ConfigureAwait(false) as NoContentResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(x => x.Send(It.Is<UpdateSolutionConnectivityAndResolutionCommand>(command => command.Id == SolutionId && command.ViewModel == _viewModel), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task InvalidRequiredResultForCommandReturnsValidationDetails()
        {
            _validationResult.Required.Add("Hello");
            var result = await _controller.UpdateConnectivityAndResolutionAsync(SolutionId, _viewModel).ConfigureAwait(false) as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            var validationResult = result?.Value as UpdateFormRequiredResult;
            validationResult?.Required.Should().BeEquivalentTo(_validationResult.Required);
        }

        [Test]
        public async Task GetValidSolutionReturnsDetails()
        {
            var result = await _controller.GetConnectivityAndResolution(SolutionId).ConfigureAwait(false) as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var getResult = result?.Value as GetSolutionConnectivityAndResolutionResult;
            getResult?.MinimumConnectionSpeed.Should().Be(_clientApplication.MinimumConnectionSpeed);
            getResult?.MinimumDesktopResolution.Should().Be(_clientApplication.MinimumDesktopResolution);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task GetInvalidSolutionThrowsNotFound()
        {
            var result = await _controller.GetConnectivityAndResolution("unknownId").ConfigureAwait(false) as NotFoundResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == "unknownId"),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
