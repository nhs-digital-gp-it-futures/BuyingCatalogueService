using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using NHSD.BuyingCatalogue.API.Controllers;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;

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
