using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.API.Controllers;
using NHSD.BuyingCatalogue.API.ViewModels;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;
using NHSD.BuyingCatalogue.Contracts;
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
            var capabilities = new List<ICapability>
            {
                Mock.Of<ICapability>(c => c.IsFoundation == true && c.Id == Guid.NewGuid() && c.Name == "Name1"),
                Mock.Of<ICapability>(c => c.IsFoundation == false && c.Id == Guid.NewGuid() && c.Name == "Name2")
            };

            _mockMediator.Setup(m => m.Send(It.IsAny<ListCapabilitiesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(capabilities);

            var result = (await _capabilitiesController.ListAsync()).Result as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as ListCapabilitiesResult).Capabilities.Should().BeEquivalentTo(capabilities);
        }
    }
}
