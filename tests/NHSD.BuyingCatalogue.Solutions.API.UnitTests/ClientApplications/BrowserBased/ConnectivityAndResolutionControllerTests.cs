using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionConnectivityAndResolution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.BrowserBased
{
    [TestFixture]
    public sealed class ConnectivityAndResolutionControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private ConnectivityAndResolutionController _controller;
        private const string SolutionId = "Sln1";
        private Mock<ISimpleResult> _simpleResultMock;
        private Dictionary<string, string> _resultDictionary;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new ConnectivityAndResolutionController(_mockMediator.Object);
            _simpleResultMock = new Mock<ISimpleResult>();
            _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
            _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
            _resultDictionary = new Dictionary<string, string>();
            _mockMediator.Setup(x =>
                    x.Send(It.IsAny<UpdateSolutionConnectivityAndResolutionCommand>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _simpleResultMock.Object);
        }

        [Test]
        public async Task UpdateConnectivityResult()
        {
            var request = new UpdateBrowserBasedConnectivityAndResolutionViewModel();

            var result =
                (await _controller.UpdateConnectivityAndResolutionAsync(SolutionId, request)
                    .ConfigureAwait(false)) as NoContentResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(
                x => x.Send(
                    It.Is<UpdateSolutionConnectivityAndResolutionCommand>(c =>
                        c.Id == SolutionId &&
                        c.Data == request
                    ),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task InvalidRequiredResultForCommandReturnsValidationDetails()
        {
            _resultDictionary.Add("minimum-connection-speed", "required");
            var request = new UpdateBrowserBasedConnectivityAndResolutionViewModel();

            var result =
                (await _controller.UpdateConnectivityAndResolutionAsync(SolutionId, request)
                    .ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var validationResult = result.Value as Dictionary<string, string>;
            validationResult.Count.Should().Be(1);
            validationResult["minimum-connection-speed"].Should().Be("required");

            _mockMediator.Verify(x => x.Send(
                    It.Is<UpdateSolutionConnectivityAndResolutionCommand>(c =>
                        c.Data.MinimumConnectionSpeed == null &&
                        c.Data.MinimumDesktopResolution == null &&
                        c.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [TestCase(null, null)]
        [TestCase("Connection Speed", null)]
        [TestCase(null, "Resolution")]
        [TestCase("Connection Speed", "Resolution")]
        public async Task ShouldGetConnectivityAndResolution(string connection, string resolution)
        {
            _mockMediator.Setup(m => m
                    .Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c =>
                    c.MinimumConnectionSpeed == connection &&
                    c.MinimumDesktopResolution == resolution));

            var result = (await _controller.GetConnectivityAndResolution(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var connectivityResult = (result.Value as GetConnectivityAndResolutionResult);

            connectivityResult.MinimumConnectionSpeed.Should().Be(connection);
            connectivityResult.MinimumDesktopResolution.Should().Be(resolution);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
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
