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
        private string _solutionId = "Sln1";
        private UpdateSolutionConnectivityAndResolutionViewModel _viewModel;
        private UpdateSolutionConnectivityAndResolutionValidationResult _validationResult;
        private ISolution _solution;
        private IClientApplication _clientApplication;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _mockMediator.Setup(x => x.Send(It.Is<UpdateSolutionConnectivityAndResolutionCommand>(command =>
                    command.Id == _solutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _validationResult);
            _clientApplication = Mock.Of<IClientApplication>(c => c.MinimumConnectionSpeed == "1 PPH" && c.MinimumDesktopResolution == "1x1");
            _solution = Mock.Of<ISolution>(s => s.ClientApplication == _clientApplication);
            _mockMediator.Setup(x => x.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == _solutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _solution);
            _controller = new ConnectivityAndResolutionController(_mockMediator.Object);
            _validationResult = new UpdateSolutionConnectivityAndResolutionValidationResult();
            _viewModel = new UpdateSolutionConnectivityAndResolutionViewModel { MinimumConnectionSpeed = "1 PPH (Pigeon Per Hour)", MinimumDesktopResolution = "1x1" };
        }

        [Test]
        public async Task ValidResultForCommandReturnsNoContent()
        {
            var result = await _controller.UpdateConnectivityAndResolutionAsync(_solutionId, _viewModel).ConfigureAwait(false) as NoContentResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(x => x.Send(It.Is<UpdateSolutionConnectivityAndResolutionCommand>(command => command.Id == _solutionId && command.ViewModel == _viewModel), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task InvalidRequiredResultForCommandReturnsValidationDetails()
        {
            _validationResult.Required.Add("Hello");
            var result = await _controller.UpdateConnectivityAndResolutionAsync(_solutionId, _viewModel).ConfigureAwait(false) as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            var validationResult = result.Value as UpdateSolutionConnectivityAndResolutionResult;
            validationResult.Required.Should().BeEquivalentTo(_validationResult.Required);
        }

        [Test]
        public async Task GetValidSolutionReturnsDetails()
        {
            var result = await _controller.GetConnectivityAndResolution(_solutionId).ConfigureAwait(false) as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var getResult = result.Value as GetSolutionConnectivityAndResolutionResult;
            getResult.MinimumConnectionSpeed.Should().Be(_clientApplication.MinimumConnectionSpeed);
            getResult.MinimumDesktopResolution.Should().Be(_clientApplication.MinimumDesktopResolution);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == _solutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task GetInvalidSolutionThrowsNotFound()
        {
            var result = await _controller.GetConnectivityAndResolution("unknownId").ConfigureAwait(false) as NotFoundResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == "unknownId"), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
