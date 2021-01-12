using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Capabilities.API.ViewModels;
using NHSD.BuyingCatalogue.Capabilities.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Capabilities.API.UnitTests
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
            var capabilities = new List<ICapability>
            {
                Mock.Of<ICapability>(c => c.IsFoundation == true && c.CapabilityReference == "C5" && c.Version == "1.0.1" && c.Name == "Name1"),
                Mock.Of<ICapability>(c => c.IsFoundation == false && c.CapabilityReference == "C27" && c.Version == "1.0.1" && c.Name == "Name2"),
            };

            _mockMediator.Setup(m => m.Send(It.IsAny<ListCapabilitiesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(capabilities);

            var result = (await _capabilitiesController.ListAsync().ConfigureAwait(false)).Result as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as ListCapabilitiesResult).Capabilities.Should().BeEquivalentTo(capabilities);
        }
    }
}
