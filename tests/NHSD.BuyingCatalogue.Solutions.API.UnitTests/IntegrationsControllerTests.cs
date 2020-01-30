using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
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

        [Test]
        public async Task ShouldGetIntegrations()
        {
            var url = "Some integrations url";
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
    }
}
