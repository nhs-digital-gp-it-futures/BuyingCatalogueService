using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    internal sealed class CapabilitiesControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private CapabilitiesController controller;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            controller = new CapabilitiesController(mockMediator.Object);
        }

        [Test]
        public async Task ShouldUpdateValidationValidAsync()
        {
            HashSet<string> newCapabilitiesReferences = new HashSet<string> { "C1", "C2" };

            var viewModel = new UpdateCapabilitiesViewModel { NewCapabilitiesReferences = newCapabilitiesReferences };
            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.IsValid).Returns(true);

            Expression<Func<UpdateCapabilitiesCommand, bool>> match = c =>
                c.SolutionId == SolutionId
                && c.NewCapabilitiesReferences == newCapabilitiesReferences;

            mockMediator
                .Setup(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = await controller.Update(SolutionId, viewModel) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mockMediator.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            HashSet<string> newCapabilitiesReferences = new HashSet<string> { "C1", "C2" };
            var model = new UpdateCapabilitiesViewModel { NewCapabilitiesReferences = newCapabilitiesReferences };
            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.ToDictionary()).Returns(new Dictionary<string, string>
            {
                { "capabilities", "capabilityInvalid" },
            });

            validationModel.Setup(s => s.IsValid).Returns(false);

            Expression<Func<UpdateCapabilitiesCommand, bool>> match = c =>
                c.SolutionId == SolutionId
                && c.NewCapabilitiesReferences == newCapabilitiesReferences;

            mockMediator
                .Setup(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = await controller.Update(SolutionId, model) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var resultValue = result.Value as Dictionary<string, string>;

            Assert.NotNull(resultValue);
            resultValue.Count.Should().Be(1);
            resultValue["capabilities"].Should().Be("capabilityInvalid");

            mockMediator.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }
    }
}
