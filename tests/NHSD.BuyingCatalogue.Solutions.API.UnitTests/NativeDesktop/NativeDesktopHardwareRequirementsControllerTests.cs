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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateNativeDesktopHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.NativeDesktop
{
    [TestFixture]
    public sealed class NativeDesktopHardwareRequirementsControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private NativeDesktopHardwareRequirementsController _controller;
        private readonly string _solutionId = "Sln1";
        private Mock<ISimpleResult> _simpleResultMock;
        private Dictionary<string, string> _resultDictionary;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new NativeDesktopHardwareRequirementsController(_mediatorMock.Object);
            _simpleResultMock = new Mock<ISimpleResult>();
            _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
            _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
            _resultDictionary = new Dictionary<string, string>();
            _mediatorMock.Setup(x =>
                    x.Send(It.IsAny<UpdateNativeDesktopHardwareRequirementsCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _simpleResultMock.Object);
        }

        [TestCase("New Hardware")]
        [TestCase("       ")]
        [TestCase("")]
        [TestCase(null)]
        public async Task PopulatedHardwareDetailsShouldReturnHardwareDetails(string hardwareRequirements)
        {
            _mediatorMock.Setup(x => x.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == _solutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c =>
                    c.NativeDesktopHardwareRequirements == hardwareRequirements));

            var result = await _controller.GetHardwareRequirements(_solutionId).ConfigureAwait(false) as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<GetNativeDesktopHardwareRequirementsResult>();
            var hardwareResult = result.Value as GetNativeDesktopHardwareRequirementsResult;
            hardwareResult.HardwareRequirements.Should().Be(hardwareRequirements);
        }

        [Test]
        public async Task NullClientApplicationShouldReturnNull()
        {
            _mediatorMock.Setup(x => x.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == _solutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as IClientApplication);

            var result = (await _controller.GetHardwareRequirements(_solutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<GetNativeDesktopHardwareRequirementsResult>();
            var hardwareResult = result.Value as GetNativeDesktopHardwareRequirementsResult;
            hardwareResult.HardwareRequirements.Should().BeNull();
        }

        [Test]
        public async Task UpdateValidUpdatesRequirements()
        {
            var request = new UpdateNativeDesktopHardwareRequirementsViewModel { HardwareRequirements = "New Hardware Requirements" };
            var result = (await _controller.UpdateHardwareRequirements(_solutionId, request).ConfigureAwait(false))
                as NoContentResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mediatorMock.Verify(x => x.Send(
                It.Is<UpdateNativeDesktopHardwareRequirementsCommand>(c =>
                    c.HardwareRequirements == "New Hardware Requirements" &&
                    c.SolutionId == _solutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task UpdateInvalidReturnsBadRequestWithValidationDetails()
        {
            _resultDictionary.Add("hardware-requirements", "maxLength");
            var request = new UpdateNativeDesktopHardwareRequirementsViewModel { HardwareRequirements = "New Hardware Requirements" };
            var result = (await _controller.UpdateHardwareRequirements(_solutionId, request).ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var validationResult = result.Value as Dictionary<string, string>;
            validationResult.Count.Should().Be(1);
            validationResult["hardware-requirements"].Should().Be("maxLength");

            _mediatorMock.Verify(x => x.Send(
                    It.Is<UpdateNativeDesktopHardwareRequirementsCommand>(c =>
                        c.HardwareRequirements == "New Hardware Requirements" &&
                        c.SolutionId == _solutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
