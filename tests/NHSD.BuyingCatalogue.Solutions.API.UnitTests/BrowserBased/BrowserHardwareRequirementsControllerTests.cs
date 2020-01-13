using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowserHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateNativeDesktopHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.BrowserBased
{
    [TestFixture]
    public sealed class BrowserHardwareRequirementsControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private BrowserHardwareRequirementsController _hardwareRequirementsController;
        private const string SolutionId = "Sln1";
        private Mock<ISimpleResult> _simpleResultMock;
        private Dictionary<string, string> _resultDictionary;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _hardwareRequirementsController = new BrowserHardwareRequirementsController(_mockMediator.Object);
            _simpleResultMock = new Mock<ISimpleResult>();
            _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
            _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
            _resultDictionary = new Dictionary<string, string>();
            _mockMediator.Setup(x =>
                    x.Send(It.IsAny<UpdateSolutionBrowserHardwareRequirementsCommand>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _simpleResultMock.Object);
        }

        [TestCase(null)]
        [TestCase("Hardware Info")]
        public async Task ShouldGetBrowserHardwareRequirement(string requirement)
        {
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c =>
                    c.HardwareRequirements == requirement));

            var result =
                (await _hardwareRequirementsController.GetHardwareRequirementsAsync(SolutionId)
                    .ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var browserHardwareRequirements = result.Value as GetBrowserHardwareRequirementsResult;

            browserHardwareRequirements.HardwareRequirements.Should().Be(requirement);
            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = (await _hardwareRequirementsController.GetHardwareRequirementsAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var browserHardwareRequirements = result.Value as GetBrowserHardwareRequirementsResult;
            browserHardwareRequirements.HardwareRequirements.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var request = new UpdateBrowserBasedHardwareRequirementViewModel { HardwareRequirements = "New Hardware Requirements" };
            var result = (await _hardwareRequirementsController.UpdateHardwareRequirementsAsync(SolutionId, request).ConfigureAwait(false))
                as NoContentResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(x => x.Send(
                    It.Is<UpdateSolutionBrowserHardwareRequirementsCommand>(c =>
                        c.HardwareRequirements == "New Hardware Requirements" &&
                        c.SolutionId == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            _resultDictionary.Add("hardware-requirements-description", "maxLength");
            var request = new UpdateBrowserBasedHardwareRequirementViewModel { HardwareRequirements = "New Hardware Requirements" };
            var result =
                (await _hardwareRequirementsController.UpdateHardwareRequirementsAsync(SolutionId, request)
                    .ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var validationResult = result.Value as Dictionary<string, string>;
            validationResult.Count.Should().Be(1);
            validationResult["hardware-requirements-description"].Should().Be("maxLength");

            _mockMediator.Verify(x => x.Send(
                    It.Is<UpdateSolutionBrowserHardwareRequirementsCommand>(c =>
                        c.HardwareRequirements == "New Hardware Requirements" &&
                        c.SolutionId == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
