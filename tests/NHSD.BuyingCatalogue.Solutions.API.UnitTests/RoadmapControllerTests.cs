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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadmap;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class RoadmapControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private RoadmapController _controller;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new RoadmapController(_mockMediator.Object);
        }

        [TestCase("Some roadmap summary")]
        [TestCase(null)]
        public async Task ShouldGetRoadMap(string summary)
        {
            _mockMediator.Setup(m => m.Send(It.Is<GetRoadMapBySolutionIdQuery>(r => r.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IRoadMap>(m => m.Summary == summary));

            var result = await _controller.Get(SolutionId).ConfigureAwait(false);
            result.Should().BeOfType<OkObjectResult>();
            result.Should().NotBeNull();

            var objectResult = result as OkObjectResult;
            objectResult.Value.Should().BeOfType<RoadMapResult>();

            var roadMapResult = objectResult.Value as RoadMapResult;
            roadMapResult.Should().NotBeNull();
            roadMapResult.Summary.Should().Be(summary);

            _mockMediator.Verify(m => m.Send(It.Is<GetRoadMapBySolutionIdQuery>(r => r.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
            _mockMediator.VerifyNoOtherCalls();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var expected = "a description";
            var viewModel = new UpdateRoadmapViewModel { Summary = expected };
            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.IsValid).Returns(true);

            _mockMediator.Setup(m => m.Send(It.Is<UpdateRoadmapCommand>(q => q.SolutionId == SolutionId && q.Summary == expected), It.IsAny<CancellationToken>())).ReturnsAsync(validationModel.Object);

            var result =
                (await _controller.Update(SolutionId, viewModel).ConfigureAwait(false)) as
                    NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(m => m.Send(It.Is<UpdateRoadmapCommand>(q => q.SolutionId == SolutionId && q.Summary == expected), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var expected = "a description";
            var viewModel = new UpdateRoadmapViewModel {Summary = expected};
            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.ToDictionary()).Returns(new Dictionary<string, string> { { "description", "maxLength" } });
            validationModel.Setup(s => s.IsValid).Returns(false);

            _mockMediator.Setup(m => m.Send(It.Is<UpdateRoadmapCommand>(q => q.SolutionId == SolutionId), It.IsAny<CancellationToken>())).ReturnsAsync(validationModel.Object);

            var result =
                (await _controller.Update(SolutionId, viewModel).ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var resultValue = result.Value as Dictionary<string, string>;
            resultValue.Count.Should().Be(1);
            resultValue["description"].Should().Be("maxLength");

            _mockMediator.Verify(m => m.Send(It.Is<UpdateRoadmapCommand>(q => q.SolutionId == SolutionId && q.Summary == expected), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
