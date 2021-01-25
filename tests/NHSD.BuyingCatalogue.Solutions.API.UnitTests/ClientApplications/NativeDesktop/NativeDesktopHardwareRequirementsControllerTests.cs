using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.NativeDesktop
{
    [TestFixture]
    internal sealed class NativeDesktopHardwareRequirementsControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mediatorMock;
        private NativeDesktopHardwareRequirementsController controller;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mediatorMock = new Mock<IMediator>();
            controller = new NativeDesktopHardwareRequirementsController(mediatorMock.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(m => m.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(m => m.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mediatorMock
                .Setup(m => m.Send(
                    It.IsAny<UpdateNativeDesktopHardwareRequirementsCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
        }

        [TestCase("New Hardware")]
        [TestCase("       ")]
        [TestCase("")]
        [TestCase(null)]
        public async Task PopulatedHardwareDetailsShouldReturnHardwareDetails(string hardwareRequirements)
        {
            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c => c.NativeDesktopHardwareRequirements == hardwareRequirements));

            var result = await controller.GetHardwareRequirements(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<GetNativeDesktopHardwareRequirementsResult>();

            var hardwareResult = result.Value as GetNativeDesktopHardwareRequirementsResult;

            Assert.NotNull(hardwareResult);
            hardwareResult.HardwareRequirements.Should().Be(hardwareRequirements);
        }

        [Test]
        public async Task NullClientApplicationShouldReturnNull()
        {
            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as IClientApplication);

            var result = await controller.GetHardwareRequirements(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<GetNativeDesktopHardwareRequirementsResult>();

            var hardwareResult = result.Value as GetNativeDesktopHardwareRequirementsResult;

            Assert.NotNull(hardwareResult);
            hardwareResult.HardwareRequirements.Should().BeNull();
        }

        [Test]
        public async Task UpdateValidUpdatesRequirements()
        {
            var request = new UpdateNativeDesktopHardwareRequirementsViewModel
            {
                HardwareRequirements = "New Hardware Requirements",
            };

            var result = await controller.UpdateHardwareRequirements(SolutionId, request) as NoContentResult;

            Expression<Func<UpdateNativeDesktopHardwareRequirementsCommand, bool>> match = c =>
                c.HardwareRequirements == "New Hardware Requirements"
                && c.SolutionId == SolutionId;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mediatorMock.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task UpdateInvalidReturnsBadRequestWithValidationDetails()
        {
            resultDictionary.Add("hardware-requirements", "maxLength");
            var request = new UpdateNativeDesktopHardwareRequirementsViewModel
            {
                HardwareRequirements = "New Hardware Requirements",
            };

            var result = await controller.UpdateHardwareRequirements(SolutionId, request) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var validationResult = result.Value as Dictionary<string, string>;

            Assert.NotNull(validationResult);
            validationResult.Count.Should().Be(1);
            validationResult["hardware-requirements"].Should().Be("maxLength");

            Expression<Func<UpdateNativeDesktopHardwareRequirementsCommand, bool>> match =
                c => c.HardwareRequirements == "New Hardware Requirements"
                    && c.SolutionId == SolutionId;

            mediatorMock.Verify(m => m.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }
    }
}
