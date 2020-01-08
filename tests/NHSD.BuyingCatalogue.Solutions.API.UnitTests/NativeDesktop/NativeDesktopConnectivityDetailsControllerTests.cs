using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionConnectivityDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.NativeDesktop
{
    [TestFixture]
    public sealed class NativeDesktopConnectivityDetailsControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private NativeDesktopConnectivityDetailsController _nativeDesktopConnectivityDetailsController;
        private readonly string _solutionId = "Sln1";
        private Mock<ISimpleResult> _simpleResultMock;
        private Dictionary<string, string> _resultDictionary;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _nativeDesktopConnectivityDetailsController = new NativeDesktopConnectivityDetailsController(_mediatorMock.Object);
            _simpleResultMock = new Mock<ISimpleResult>();
            _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
            _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
            _resultDictionary = new Dictionary<string, string>();
            _mediatorMock.Setup(x =>
                    x.Send(It.IsAny<UpdateSolutionNativeDesktopConnectivityDetailsCommand>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _simpleResultMock.Object);
        }

        [Test]
        public async Task UpdateValidConnectivityDetails()
        {
            var request =
                new UpdateNativeDesktopConnectivityDetailsViewModel {NativeDesktopMinimumConnectionSpeed = "6Mbps"};
            var result =
                (await _nativeDesktopConnectivityDetailsController.UpdatedConnectivity(_solutionId, request)
                    .ConfigureAwait(false)) as NoContentResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mediatorMock.Verify(
                x => x.Send(
                    It.Is<UpdateSolutionNativeDesktopConnectivityDetailsCommand>(c =>
                        c.SolutionId == _solutionId && c.NativeDesktopMinimumConnectionSpeed == "6Mbps"),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateInvalidReturnsBadRequestWithValidationDetails()
        {
            _resultDictionary.Add("minimum-connection-speed", "required");
            var request =
                new UpdateNativeDesktopConnectivityDetailsViewModel {NativeDesktopMinimumConnectionSpeed = null};
            var result =
                (await _nativeDesktopConnectivityDetailsController.UpdatedConnectivity(_solutionId, request)
                    .ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var validationResult = result.Value as Dictionary<string, string>;
            validationResult.Count.Should().Be(1);
            validationResult["minimum-connection-speed"].Should().Be("required");

            _mediatorMock.Verify(x => x.Send(
                    It.Is<UpdateSolutionNativeDesktopConnectivityDetailsCommand>(c =>
                        c.NativeDesktopMinimumConnectionSpeed == null &&
                        c.SolutionId == _solutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
