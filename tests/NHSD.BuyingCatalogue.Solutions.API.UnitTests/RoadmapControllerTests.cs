using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadMap;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class RoadMapControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mockMediator;
        private RoadMapController controller;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            controller = new RoadMapController(mockMediator.Object);
        }

        [TestCase("Some road map summary")]
        [TestCase(null)]
        public async Task ShouldGetRoadMap(string summary)
        {
            mockMediator
                .Setup(m => m.Send(It.Is<GetRoadMapBySolutionIdQuery>(r => r.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IRoadMap>(m => m.Summary == summary));

            var result = await controller.Get(SolutionId);

            result.Should().BeOfType<OkObjectResult>();
            result.Should().NotBeNull();

            var objectResult = result as OkObjectResult;

            Assert.NotNull(objectResult);
            objectResult.Value.Should().BeOfType<RoadMapResult>();

            var roadMapResult = objectResult.Value as RoadMapResult;
            Assert.NotNull(roadMapResult);
            roadMapResult.Summary.Should().Be(summary);

            mockMediator.Verify(m => m.Send(
                It.Is<GetRoadMapBySolutionIdQuery>(r => r.Id == SolutionId),
                It.IsAny<CancellationToken>()));

            mockMediator.VerifyNoOtherCalls();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            const string expected = "a description";
            var model = new UpdateRoadMapViewModel { Summary = expected };
            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.IsValid).Returns(true);

            mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateRoadMapCommand>(c => c.SolutionId == SolutionId && c.Summary == expected),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = await controller.Update(SolutionId, model) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateRoadMapCommand>(c => c.SolutionId == SolutionId && c.Summary == expected),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            const string expected = "a description";
            var model = new UpdateRoadMapViewModel { Summary = expected };
            var validationModel = new Mock<ISimpleResult>();
            validationModel
                .Setup(s => s.ToDictionary())
                .Returns(new Dictionary<string, string> { { "description", "maxLength" } });

            validationModel.Setup(s => s.IsValid).Returns(false);

            mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateRoadMapCommand>(c => c.SolutionId == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationModel.Object);

            var result = await controller.Update(SolutionId, model) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var resultValue = result.Value as Dictionary<string, string>;

            Assert.NotNull(resultValue);
            resultValue.Count.Should().Be(1);
            resultValue["description"].Should().Be("maxLength");

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateRoadMapCommand>(c => c.SolutionId == SolutionId && c.Summary == expected),
                It.IsAny<CancellationToken>()));
        }
    }
}
