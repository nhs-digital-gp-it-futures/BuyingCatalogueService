using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Capabilities.API.ViewModels;
using NHSD.BuyingCatalogue.Capabilities.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Capabilities.API.UnitTests
{
    [TestFixture]
    internal sealed class CapabilitiesControllerTests
    {
        private Mock<IMediator> mockMediator;

        private CapabilitiesController capabilitiesController;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            capabilitiesController = new CapabilitiesController(mockMediator.Object);
        }

        [Test]
        public async Task ShouldListCapabilities()
        {
            Expression<Func<ICapability, bool>> capability1 = c =>
                c.IsFoundation
                && c.CapabilityReference == "C5"
                && c.Version == "1.0.1"
                && c.Name == "Name1";

            Expression<Func<ICapability, bool>> capability2 = c =>
                c.IsFoundation == false
                && c.CapabilityReference == "C27"
                && c.Version == "1.0.1"
                && c.Name == "Name2";

            var capabilities = new List<ICapability>
            {
                Mock.Of(capability1),
                Mock.Of(capability2),
            };

            mockMediator
                .Setup(m => m.Send(It.IsAny<ListCapabilitiesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(capabilities);

            var result = (await capabilitiesController.ListAsync()).Result as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            result.Value.Should().BeOfType<ListCapabilitiesResult>();
            result.Value.As<ListCapabilitiesResult>().Capabilities.Should().BeEquivalentTo(capabilities);
        }
    }
}
