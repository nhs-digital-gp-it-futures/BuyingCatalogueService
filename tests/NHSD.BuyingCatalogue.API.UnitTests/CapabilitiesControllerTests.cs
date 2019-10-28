using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.API.Controllers;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.UnitTests
{
    [TestFixture]
	public sealed class CapabilitiesControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private CapabilitiesController _capabilitiesController;

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _capabilitiesController = new CapabilitiesController(_mockMediator.Object);
        }
        
        [Test]
        public async Task ShouldListCapabilities()
        {
            var expected = new ListCapabilitiesResult(new CapabilityViewModel[0]);

            _mockMediator.Setup(m => m.Send(It.IsAny<ListCapabilitiesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected);

            var result = (await _capabilitiesController.ListAsync()).Result as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().Be(expected);
        }
    }
}
