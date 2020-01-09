using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionNativeDesktopThirdParty;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.NativeDesktop
{
    [TestFixture]
    public sealed class NativeDesktopThirdPartyControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private NativeDesktopThirdPartyController _nativeDesktopThirdPartyController;
        private readonly string _solutionId = "Sln1";
        private Mock<ISimpleResult> _simpleResultMock;
        private Dictionary<string, string> _resultDictionary;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _nativeDesktopThirdPartyController = new NativeDesktopThirdPartyController(_mediatorMock.Object);
            _simpleResultMock = new Mock<ISimpleResult>();
            _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
            _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
            _resultDictionary = new Dictionary<string, string>();
            _mediatorMock.Setup(x =>
                    x.Send(It.IsAny<UpdateSolutionNativeDesktopThirdPartyCommand>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _simpleResultMock.Object);
        }

        [Test]
        public async Task UpdateValidThirdPartyDetails()
        {
            var request = new UpdateNativeDesktopThirdPartyViewModel
            {
                ThirdPartyComponents = "New Component",
                DeviceCapabilities = "New Capability"
            };

            var result =
                (await _nativeDesktopThirdPartyController.UpdatedThirdParty(_solutionId, request)
                    .ConfigureAwait(false)) as NoContentResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mediatorMock.Verify(
                x => x.Send(
                    It.Is<UpdateSolutionNativeDesktopThirdPartyCommand>(c =>
                        c.SolutionId == _solutionId && c.Data.ThirdPartyComponents == "New Component" &&
                        c.Data.DeviceCapabilities == "New Capability"),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateInvalidReturnsBadRequestWithValidationDetails()
        {
            _resultDictionary.Add("third-party-components", "maxLength");
            _resultDictionary.Add("device-capabilities", "maxLength");
            var request = new UpdateNativeDesktopThirdPartyViewModel();

            var result =
                (await _nativeDesktopThirdPartyController.UpdatedThirdParty(_solutionId, request)
                    .ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var validationResult = result.Value as Dictionary<string, string>;
            validationResult.Count.Should().Be(2);
            validationResult["third-party-components"].Should().Be("maxLength");
            validationResult["device-capabilities"].Should().Be("maxLength");
            
            _mediatorMock.Verify(x => x.Send(
                    It.Is<UpdateSolutionNativeDesktopThirdPartyCommand>(c =>
                        c.Data.ThirdPartyComponents == request.ThirdPartyComponents &&
                        c.Data.DeviceCapabilities == request.DeviceCapabilities &&
                        c.SolutionId == _solutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
