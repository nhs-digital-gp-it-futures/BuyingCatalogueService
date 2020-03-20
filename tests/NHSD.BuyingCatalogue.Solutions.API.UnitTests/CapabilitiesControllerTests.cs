using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateCapabilities;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class CapabilitiesControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private CapabilitiesController _controller;
        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new CapabilitiesController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldUpdateValidationValidAsync()
        {
            HashSet<string> newCapabilitiesReferences = new HashSet<string>() { "C1", "C2" };

            var viewModel = new UpdateCapabilitiesViewModel { NewCapabilitiesReferences = newCapabilitiesReferences };
            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.IsValid).Returns(true);

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateCapabilitiesCommand>(q =>
                        q.SolutionId == SolutionId && q.NewCapabilitiesReferences == newCapabilitiesReferences),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationModel.Object);

            var result = (await _controller.Update(SolutionId, viewModel).ConfigureAwait(false)) as NoContentResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateCapabilitiesCommand>(q =>
                        q.SolutionId == SolutionId && q.NewCapabilitiesReferences == newCapabilitiesReferences),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            HashSet<string> newCapabilitiesReferences = new HashSet<string>() { "C1", "C2" };
            var viewModel = new UpdateCapabilitiesViewModel { NewCapabilitiesReferences = newCapabilitiesReferences };
            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.ToDictionary()).Returns(new Dictionary<string, string> { { "capabilities", "capabilityInvalid" } });
            validationModel.Setup(s => s.IsValid).Returns(false);

            _mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateCapabilitiesCommand>(q =>
                        q.SolutionId == SolutionId && q.NewCapabilitiesReferences == newCapabilitiesReferences),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationModel.Object);

            var result =
                (await _controller.Update(SolutionId, viewModel).ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var resultValue = result.Value as Dictionary<string, string>;
            resultValue.Count.Should().Be(1);
            resultValue["capabilities"].Should().Be("capabilityInvalid");

            _mockMediator.Verify(
                m => m.Send(
                    It.Is<UpdateCapabilitiesCommand>(q =>
                        q.SolutionId == SolutionId && q.NewCapabilitiesReferences == newCapabilitiesReferences),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
