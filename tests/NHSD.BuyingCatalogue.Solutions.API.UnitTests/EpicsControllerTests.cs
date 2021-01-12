using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Epics;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateClaimedEpics;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    public sealed class EpicsControllerTests : ControllerBase
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> _mockMediator;
        private EpicsController _controller;
        
        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new EpicsController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldUpdateValidationValidAsync()
        {
            HashSet<ClaimedEpicViewModel> claimedEpics = new HashSet<ClaimedEpicViewModel>()
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

            _mockMediator
                .Setup(m => m.Send(It.IsAny<UpdateClaimedEpicsCommand>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationModel.Object);

            var result = (await _controller.UpdateAsync(SolutionId, viewModel).ConfigureAwait(false)) as NoContentResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            _mockMediator.Verify(
                m => m.Send(It.Is<UpdateClaimedEpicsCommand>(e => e.SolutionId == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            HashSet<ClaimedEpicViewModel> claimedEpics = new HashSet<ClaimedEpicViewModel>()
            {
                new()
                {
                    EpicId = "Test",
                    StatusName = "Unknown",
                },
            };

            var viewModel = new UpdateEpicsViewModel { ClaimedEpics = claimedEpics };
            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.ToDictionary()).Returns(new Dictionary<string, string> { { "epics", "epicsInvalid" } });
            validationModel.Setup(s => s.IsValid).Returns(false);

            _mockMediator
                .Setup(m => m.Send(It.IsAny<UpdateClaimedEpicsCommand>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationModel.Object);

            var result = (await _controller.UpdateAsync(SolutionId, viewModel).ConfigureAwait(false)) as BadRequestObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            var resultValue = result.Value as Dictionary<string, string>;
            resultValue.Count.Should().Be(1);
            resultValue["epics"].Should().Be("epicsInvalid");

            _mockMediator.Verify(
                m => m.Send(It.Is<UpdateClaimedEpicsCommand>(e => e.SolutionId == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
