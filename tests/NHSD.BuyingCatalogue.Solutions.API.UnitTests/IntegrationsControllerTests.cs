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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateIntegrations;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class IntegrationsControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private IntegrationsController _controller;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new IntegrationsController(_mockMediator.Object);
        }

        [TestCase("Some integrations url")]
        [TestCase(null)]
        public async Task ShouldGetIntegrations(string url)
        {
            _mockMediator.Setup(m => m.Send(It.Is<GetIntegrationsBySolutionIdQuery>(r => r.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IIntegrations>(m => m.Url == url));

            var result = await _controller.Get(SolutionId).ConfigureAwait(false);
            result.Should().BeOfType<OkObjectResult>();
            result.Should().NotBeNull();

            var objectResult = result as OkObjectResult;
            objectResult.Value.Should().BeOfType<IntegrationsResult>();

            var integrationsResult = objectResult.Value as IntegrationsResult;
            integrationsResult.Should().NotBeNull();
            integrationsResult.Url.Should().Be(url);

            _mockMediator.Verify(m => m.Send(It.Is<GetIntegrationsBySolutionIdQuery>(r => r.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
            _mockMediator.VerifyNoOtherCalls();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var expected = "an integrations url";
            var viewModel = new UpdateIntegrationsViewModel { Url = expected };
            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.IsValid).Returns(true);

            _mockMediator.Setup(m => m.Send(It.Is<UpdateIntegrationsCommand>(q => q.SolutionId == SolutionId && q.Url == expected), It.IsAny<CancellationToken>())).ReturnsAsync(validationModel.Object);

            var result =
                (await _controller.Update(SolutionId, viewModel).ConfigureAwait(false)) as
                NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(m => m.Send(It.Is<UpdateIntegrationsCommand>(q => q.SolutionId == SolutionId && q.Url == expected), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var expected = "an integrations url";
            var viewModel = new UpdateIntegrationsViewModel { Url = expected };
            var validationModel = new Mock<ISimpleResult>();
            validationModel.Setup(s => s.ToDictionary()).Returns(new Dictionary<string, string> { { "link", "maxLength" } });
            validationModel.Setup(s => s.IsValid).Returns(false);

            _mockMediator.Setup(m => m.Send(It.Is<UpdateIntegrationsCommand>(q => q.SolutionId == SolutionId), It.IsAny<CancellationToken>())).ReturnsAsync(validationModel.Object);

            var result =
                (await _controller.Update(SolutionId, viewModel).ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var resultValue = result.Value as Dictionary<string, string>;
            resultValue.Count.Should().Be(1);
            resultValue["link"].Should().Be("maxLength");

            _mockMediator.Verify(m => m.Send(It.Is<UpdateIntegrationsCommand>(q => q.SolutionId == SolutionId && q.Url == expected), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
