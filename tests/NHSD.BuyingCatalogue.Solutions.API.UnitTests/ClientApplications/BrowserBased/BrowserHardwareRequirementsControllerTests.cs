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
using NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionBrowserHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.BrowserBased
{
    [TestFixture]
    internal sealed class BrowserHardwareRequirementsControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private BrowserHardwareRequirementsController hardwareRequirementsController;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            hardwareRequirementsController = new BrowserHardwareRequirementsController(mockMediator.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(x => x.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mockMediator
                .Setup(m => m.Send(
                    It.IsAny<UpdateSolutionBrowserHardwareRequirementsCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
        }

        [TestCase(null)]
        [TestCase("Hardware Info")]
        public async Task ShouldGetBrowserHardwareRequirement(string requirement)
        {
            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IClientApplication>(c => c.HardwareRequirements == requirement));

            var result = (await hardwareRequirementsController.GetHardwareRequirementsAsync(SolutionId)) as ObjectResult;

            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var browserHardwareRequirements = result.Value as GetBrowserHardwareRequirementsResult;

            browserHardwareRequirements.HardwareRequirements.Should().Be(requirement);
            mockMediator.Verify(m => m.Send(
                It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = (await hardwareRequirementsController.GetHardwareRequirementsAsync(SolutionId)) as ObjectResult;

            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            var browserHardwareRequirements = result.Value as GetBrowserHardwareRequirementsResult;
            browserHardwareRequirements.HardwareRequirements.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var request = new UpdateBrowserBasedHardwareRequirementViewModel
            {
                HardwareRequirements = "New Hardware Requirements",
            };

            var result = (await hardwareRequirementsController.UpdateHardwareRequirementsAsync(
                SolutionId,
                request)) as NoContentResult;

            Expression<Func<UpdateSolutionBrowserHardwareRequirementsCommand, bool>> match = c =>
                c.HardwareRequirements == "New Hardware Requirements"
                && c.SolutionId == SolutionId;

            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
            mockMediator.Verify(x => x.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            resultDictionary.Add("hardware-requirements-description", "maxLength");
            var request = new UpdateBrowserBasedHardwareRequirementViewModel
            {
                HardwareRequirements = "New Hardware Requirements",
            };

            var result = (await hardwareRequirementsController.UpdateHardwareRequirementsAsync(
                SolutionId,
                request)) as BadRequestObjectResult;

            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            var validationResult = result.Value as Dictionary<string, string>;
            validationResult.Count.Should().Be(1);
            validationResult["hardware-requirements-description"].Should().Be("maxLength");

            Expression<Func<UpdateSolutionBrowserHardwareRequirementsCommand, bool>> match = c =>
                c.HardwareRequirements == "New Hardware Requirements"
                && c.SolutionId == SolutionId;

            mockMediator.Verify(x => x.Send(It.Is(match), It.IsAny<CancellationToken>()));
        }
    }
}
