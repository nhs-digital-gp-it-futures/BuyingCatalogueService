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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateIntegrations;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class IntegrationsControllerTests
    {
        private const string SolutionId = "Sln1";

        private Mock<IMediator> mediatorMock;
        private IntegrationsController controller;
        private Mock<ISimpleResult> simpleResultMock;
        private Dictionary<string, string> resultDictionary;

        [SetUp]
        public void Setup()
        {
            mediatorMock = new Mock<IMediator>();
            controller = new IntegrationsController(mediatorMock.Object);
            simpleResultMock = new Mock<ISimpleResult>();
            simpleResultMock.Setup(r => r.IsValid).Returns(() => !resultDictionary.Any());
            simpleResultMock.Setup(r => r.ToDictionary()).Returns(() => resultDictionary);
            resultDictionary = new Dictionary<string, string>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateIntegrationsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => simpleResultMock.Object);
        }

        [TestCase("Some integrations url")]
        [TestCase(null)]
        public async Task ShouldGetIntegrations(string url)
        {
            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetIntegrationsBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IIntegrations>(i => i.Url == url));

            var result = await controller.Get(SolutionId);
            result.Should().BeOfType<OkObjectResult>();
            result.Should().NotBeNull();

            var objectResult = result as OkObjectResult;

            Assert.NotNull(objectResult);
            objectResult.Value.Should().BeOfType<IntegrationsResult>();

            var integrationsResult = objectResult.Value as IntegrationsResult;
            Assert.NotNull(integrationsResult);
            integrationsResult.Url.Should().Be(url);

            mediatorMock.Verify(m => m.Send(
                It.Is<GetIntegrationsBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));

            mediatorMock.VerifyNoOtherCalls();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            const string expected = "an integrations url";
            var model = new UpdateIntegrationsViewModel { Url = expected };

            var result = await controller.Update(SolutionId, model) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mediatorMock.Verify(m => m.Send(
                It.Is<UpdateIntegrationsCommand>(c => c.SolutionId == SolutionId && c.Url == expected),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            const string expected = "an integrations url";
            var model = new UpdateIntegrationsViewModel { Url = expected };
            resultDictionary.Add("link", "maxLength");

            var result = await controller.Update(SolutionId, model) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var resultValue = result.Value as Dictionary<string, string>;

            Assert.NotNull(resultValue);
            resultValue.Count.Should().Be(1);
            resultValue["link"].Should().Be("maxLength");

            mediatorMock.Verify(m => m.Send(
                It.Is<UpdateIntegrationsCommand>(q => q.SolutionId == SolutionId && q.Url == expected),
                It.IsAny<CancellationToken>()));
        }
    }
}
