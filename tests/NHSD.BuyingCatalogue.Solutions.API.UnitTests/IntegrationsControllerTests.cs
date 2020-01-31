using System.Collections.Generic;
using System.Linq;
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
        private Mock<IMediator> _mediatorMock;
        private IntegrationsController _controller;
        private const string SolutionId = "Sln1";
        private Mock<ISimpleResult> _simpleResultMock;
        private Dictionary<string, string> _resultDictionary;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new IntegrationsController(_mediatorMock.Object);
            _simpleResultMock = new Mock<ISimpleResult>();
            _simpleResultMock.Setup(x => x.IsValid).Returns(() => !_resultDictionary.Any());
            _simpleResultMock.Setup(x => x.ToDictionary()).Returns(() => _resultDictionary);
            _resultDictionary = new Dictionary<string, string>();
            _mediatorMock.Setup(x =>
                    x.Send(It.IsAny<UpdateIntegrationsCommand>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _simpleResultMock.Object);
        }

        [TestCase("Some integrations url")]
        [TestCase(null)]
        public async Task ShouldGetIntegrations(string url)
        {
            _mediatorMock.Setup(m => m.Send(It.Is<GetIntegrationsBySolutionIdQuery>(r => r.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IIntegrations>(m => m.Url == url));

            var result = await _controller.Get(SolutionId).ConfigureAwait(false);
            result.Should().BeOfType<OkObjectResult>();
            result.Should().NotBeNull();

            var objectResult = result as OkObjectResult;
            objectResult.Value.Should().BeOfType<IntegrationsResult>();

            var integrationsResult = objectResult.Value as IntegrationsResult;
            integrationsResult.Should().NotBeNull();
            integrationsResult.Url.Should().Be(url);

            _mediatorMock.Verify(m => m.Send(It.Is<GetIntegrationsBySolutionIdQuery>(r => r.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Test]
        public async Task ShouldUpdateValidationValid()
        {
            var expected = "an integrations url";
            var viewModel = new UpdateIntegrationsViewModel { Url = expected };

            var result =
                (await _controller.Update(SolutionId, viewModel).ConfigureAwait(false)) as
                NoContentResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mediatorMock.Verify(m => m.Send(It.Is<UpdateIntegrationsCommand>(q => q.SolutionId == SolutionId && q.Url == expected), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateValidationInvalid()
        {
            var expected = "an integrations url";
            var viewModel = new UpdateIntegrationsViewModel { Url = expected };
            _resultDictionary.Add("link", "maxLength");

            var result =
                (await _controller.Update(SolutionId, viewModel).ConfigureAwait(false)) as BadRequestObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var resultValue = result.Value as Dictionary<string, string>;
            resultValue.Count.Should().Be(1);
            resultValue["link"].Should().Be("maxLength");

            _mediatorMock.Verify(m => m.Send(
                It.Is<UpdateIntegrationsCommand>(q => q.SolutionId == SolutionId && q.Url == expected),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
