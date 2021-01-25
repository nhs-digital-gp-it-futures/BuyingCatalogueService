using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Epics;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateClaimedEpics;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    internal sealed class EpicsControllerTests : ControllerBase
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private EpicsController controller;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            controller = new EpicsController(mockMediator.Object);
        }

        [Test]
        public async Task ShouldUpdateValidationValidAsync()
        {
            HashSet<ClaimedEpicViewModel> claimedEpics = new HashSet<ClaimedEpicViewModel>
            {
                new()
                {
                    EpicId = "Epic1",
                    StatusName = "Passed",
                },
            };

            var viewModel = new UpdateEpicsViewModel { ClaimedEpics = claimedEpics };
            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.IsValid).Returns(true);

            mockMediator
                .Setup(m => m.Send(It.IsAny<UpdateClaimedEpicsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = await controller.UpdateAsync(SolutionId, viewModel) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateClaimedEpicsCommand>(e => e.SolutionId == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            HashSet<ClaimedEpicViewModel> claimedEpics = new HashSet<ClaimedEpicViewModel>
            {
                new()
                {
                    EpicId = "Test",
                    StatusName = "Unknown",
                },
            };

            var viewModel = new UpdateEpicsViewModel { ClaimedEpics = claimedEpics };
            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.ToDictionary()).Returns(new Dictionary<string, string>
            {
                { "epics", "epicsInvalid" },
            });

            validationModel.Setup(s => s.IsValid).Returns(false);

            mockMediator
                .Setup(m => m.Send(It.IsAny<UpdateClaimedEpicsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = await controller.UpdateAsync(SolutionId, viewModel) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var resultValue = result.Value as Dictionary<string, string>;

            Assert.NotNull(resultValue);
            resultValue.Count.Should().Be(1);
            resultValue["epics"].Should().Be("epicsInvalid");

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateClaimedEpicsCommand>(e => e.SolutionId == SolutionId),
                It.IsAny<CancellationToken>()));
        }
    }
}
