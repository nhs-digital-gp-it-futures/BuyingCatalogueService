using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Epics;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateEpics;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Epics;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    public sealed class EpicsControllerTests : ControllerBase
    {
        private Mock<IMediator> _mockMediator;
        private EpicsController _controller;
        private const string SolutionId = "Sln1";

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
                new ClaimedEpicViewModel()
                {
                    EpicId = "Epic1",
                    StatusName = "Passed"
                }
            };

            var updateEpicsViewModel = new UpdateEpicsViewModel {ClaimedEpics = claimedEpics};

            var epicsViewModel = new HashSet<IClaimedEpic>(updateEpicsViewModel.ClaimedEpics);

            var viewModel = new UpdateEpicsViewModel {ClaimedEpics = claimedEpics};
            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.IsValid).Returns(true);

            _mockMediator
                .Setup(m => m.Send(It.Is<UpdateEpicsCommand>(e => e.SolutionId == SolutionId),
                    It.IsAny<CancellationToken>())).ReturnsAsync(validationModel.Object);

            var result = (await _controller.Update(SolutionId, viewModel).ConfigureAwait(false)) as NoContentResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            _mockMediator.Verify(
                m => m.Send(It.Is<UpdateEpicsCommand>(e => e.SolutionId == SolutionId),
                    It.IsAny<CancellationToken>()));
        }
    }
}
