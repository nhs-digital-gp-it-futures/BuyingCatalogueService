using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionConnectivityAndResolution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.BrowserBased
{
    [TestFixture]
    public sealed class ConnectivityAndResolutionControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private ConnectivityAndResolutionController _controller;
        private const string SolutionId = "Sln1";
        private UpdateBrowserBasedConnectivityAndResolutionViewModel _viewModel;
        private Mock<ISimpleResult> _validationResult;
        private IClientApplication _clientApplication;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _mockMediator.Setup(x => x.Send(It.Is<UpdateSolutionConnectivityAndResolutionCommand>(command =>
                    command.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _validationResult.Object);
            _clientApplication = Mock.Of<IClientApplication>(c => c.MinimumConnectionSpeed == "1 PPH" && c.MinimumDesktopResolution == "1x1");
            _mockMediator.Setup(x => x.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _clientApplication);
            _controller = new ConnectivityAndResolutionController(_mockMediator.Object);
            _validationResult = new Mock<ISimpleResult>();
            _validationResult.Setup(s => s.IsValid).Returns(true);

            _viewModel = new UpdateBrowserBasedConnectivityAndResolutionViewModel { MinimumConnectionSpeed = "1 PPH (Pigeon Per Hour)", MinimumDesktopResolution = "1x1" };
        }

        [Test]
        public async Task ValidResultForCommandReturnsNoContent()
        {
            var result = await _controller.UpdateConnectivityAndResolutionAsync(SolutionId, _viewModel).ConfigureAwait(false) as NoContentResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(x => x.Send(It.Is<UpdateSolutionConnectivityAndResolutionCommand>(command =>
                command.Id == SolutionId &&
                command.Data.MinimumConnectionSpeed == _viewModel.MinimumConnectionSpeed &&
                command.Data.MinimumDesktopResolution == _viewModel.MinimumDesktopResolution
                ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task InvalidRequiredResultForCommandReturnsValidationDetails()
        {
            _validationResult.Setup(s => s.ToDictionary()).Returns(new Dictionary<string, string> {{"Hello", "required"}});
            _validationResult.Setup(s => s.IsValid).Returns(false);

            var result = await _controller.UpdateConnectivityAndResolutionAsync(SolutionId, _viewModel).ConfigureAwait(false) as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            var resultValue = result.Value as Dictionary<string, string>;
            resultValue.Count.Should().Be(1);
            resultValue["Hello"].Should().Be("required");
        }

        [Test]
        public async Task GetValidSolutionReturnsDetails()
        {
            var result = await _controller.GetConnectivityAndResolution(SolutionId).ConfigureAwait(false) as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var getResult = result.Value as GetConnectivityAndResolutionResult;
            getResult.MinimumConnectionSpeed.Should().Be(_clientApplication.MinimumConnectionSpeed);
            getResult.MinimumDesktopResolution.Should().Be(_clientApplication.MinimumDesktopResolution);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task GetInvalidSolutionReturnsEmpty()
        {
            var result = await _controller.GetConnectivityAndResolution("unknownId").ConfigureAwait(false) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as GetConnectivityAndResolutionResult).MinimumConnectionSpeed.Should().BeNull();
            (result.Value as GetConnectivityAndResolutionResult).MinimumDesktopResolution.Should().BeNull();
        }
    }
}
