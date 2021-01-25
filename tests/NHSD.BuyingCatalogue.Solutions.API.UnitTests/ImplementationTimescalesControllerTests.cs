using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateImplementationTimescales;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class ImplementationTimescalesControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mediatorMock;
        private ImplementationTimescalesController controller;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mediatorMock = new Mock<IMediator>();
            controller = new ImplementationTimescalesController(mediatorMock.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(r => r.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(r => r.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateImplementationTimescalesCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
        }

        [TestCase("Some implementation timescales description")]
        [TestCase(null)]
        public async Task ShouldGetImplementationTimescales(string description)
        {
            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetImplementationTimescalesBySolutionIdQuery>(r => r.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IImplementationTimescales>(m => m.Description == description));

            var result = await controller.Get(SolutionId);
            result.Should().BeOfType<OkObjectResult>();
            result.Should().NotBeNull();

            var objectResult = result as OkObjectResult;

            Assert.NotNull(objectResult);
            objectResult.Value.Should().BeOfType<ImplementationTimescalesResult>();

            var implementationTimescalesResult = objectResult.Value as ImplementationTimescalesResult;

            Assert.NotNull(implementationTimescalesResult);
            implementationTimescalesResult.Description.Should().Be(description);

            mediatorMock.Verify(m => m.Send(
                It.Is<GetImplementationTimescalesBySolutionIdQuery>(r => r.Id == SolutionId),
                It.IsAny<CancellationToken>()));

            mediatorMock.VerifyNoOtherCalls();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            const string expected = "an implementation timescales description";
            var model = new UpdateImplementationTimescalesViewModel { Description = expected };

            var result = await controller.Update(SolutionId, model) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mediatorMock.Verify(m => m.Send(
                It.Is<UpdateImplementationTimescalesCommand>(c => c.SolutionId == SolutionId && c.Description == expected),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            const string expected = "an implementation timescales description";
            var viewModel = new UpdateImplementationTimescalesViewModel { Description = expected };
            resultDictionary.Add("description", "maxLength");

            var result = await controller.Update(SolutionId, viewModel) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var resultValue = result.Value as Dictionary<string, string>;

            Assert.NotNull(resultValue);
            resultValue.Count.Should().Be(1);
            resultValue["description"].Should().Be("maxLength");

            mediatorMock.Verify(m => m.Send(
                It.Is<UpdateImplementationTimescalesCommand>(q => q.SolutionId == SolutionId && q.Description == expected),
                It.IsAny<CancellationToken>()));
        }
    }
}
