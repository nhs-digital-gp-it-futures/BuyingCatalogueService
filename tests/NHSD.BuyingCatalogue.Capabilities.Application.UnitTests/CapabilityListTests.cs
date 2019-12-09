using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Capabilities.Contracts;
using NHSD.BuyingCatalogue.Capabilities.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Capabilities.Application.UnitTests
{
    [TestFixture]
    public class CapabilityListTests
    {
        private TestContext _context;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
        }

        [Test]
        public async Task ShouldReadAllCapabilities()
        {
            var capabilityData = GetCapabilities();
            _context.MockCapabilityRepository.Setup(r => r.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(capabilityData);

            var expectedCapabilities = capabilityData.Select(t => new
            {
                t.Id,
                t.Name,
                t.IsFoundation
            });

            var capabilities = await _context.ListCapabilitiesHandler.Handle(new ListCapabilitiesQuery(), new CancellationToken()).ConfigureAwait(false);
            capabilities.Should().BeEquivalentTo(expectedCapabilities);
        }

        [Test]
        public async Task ShouldReadAllCapabilitiesEmpty()
        {
            _context.MockCapabilityRepository.Setup(r => r.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ICapabilityListResult>());

            var capabilities = await _context.ListCapabilitiesHandler.Handle(new ListCapabilitiesQuery(), new CancellationToken()).ConfigureAwait(false);
            capabilities.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldCallRepository()
        {
            var capabilityData = GetCapabilities();
            _context.MockCapabilityRepository.Setup(r => r.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(capabilityData);

            var capabilities = await _context.ListCapabilitiesHandler.Handle(new ListCapabilitiesQuery(), new CancellationToken()).ConfigureAwait(false);
            _context.MockCapabilityRepository.Verify(r => r.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        private IEnumerable<ICapabilityListResult> GetCapabilities()
        {
            return new List<ICapabilityListResult>
            {
                GetCapability("Cap1", true),
                GetCapability("Cap2", false),
                GetCapability("Cap3", true),
            };
        }

        private static ICapabilityListResult GetCapability(string name, bool isFoundation)
        {
            var capability = new Mock<ICapabilityListResult>();
            capability.Setup(c => c.Id).Returns(Guid.NewGuid());
            capability.Setup(c => c.Name).Returns(name);
            capability.Setup(c => c.IsFoundation).Returns(isFoundation);
            return capability.Object;
        }
    }
}
