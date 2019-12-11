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

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _mockMediator.Setup(x => x.Send(It.Is<UpdateSolutionConnectivityAndResolutionCommand>(command =>
                    command.Id == _solutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _validationResult);
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
    }
}
