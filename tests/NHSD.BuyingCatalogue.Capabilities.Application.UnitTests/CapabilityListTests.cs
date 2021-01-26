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
    internal sealed class CapabilityListTests
    {
        private TestContext context;

        [SetUp]
        public void SetUpFixture()
        {
            context = new TestContext();
        }

        [Test]
        public async Task ShouldReadAllCapabilities()
        {
            var capabilityData = GetCapabilities().ToList();

            context.MockCapabilityRepository
                .Setup(r => r.ListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(capabilityData);

            var expectedCapabilities = capabilityData.Select(t => new
            {
                t.CapabilityReference,
                t.Version,
                t.Name,
                t.IsFoundation,
            });

            var capabilities = await context.ListCapabilitiesHandler.Handle(
                new ListCapabilitiesQuery(),
                CancellationToken.None);

            capabilities.Should().BeEquivalentTo(expectedCapabilities);
        }

        [Test]
        public async Task ShouldReadAllCapabilitiesEmpty()
        {
            context.MockCapabilityRepository
                .Setup(r => r.ListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ICapabilityListResult>());

            var capabilities = await context.ListCapabilitiesHandler.Handle(
                new ListCapabilitiesQuery(),
                CancellationToken.None);

            capabilities.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldCallRepository()
        {
            var capabilityData = GetCapabilities();
            context.MockCapabilityRepository
                .Setup(r => r.ListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(capabilityData);

            await context.ListCapabilitiesHandler.Handle(new ListCapabilitiesQuery(), CancellationToken.None);
            context.MockCapabilityRepository.Verify(r => r.ListAsync(It.IsAny<CancellationToken>()));
        }

        private static IEnumerable<ICapabilityListResult> GetCapabilities()
        {
            return new List<ICapabilityListResult>
            {
                GetCapability("C5", "1.0.1", "Cap1", true),
                GetCapability("C10", "1.0.1", "Cap2", false),
                GetCapability("C11", "1.0.1", "Cap3", true),
            };
        }

        private static ICapabilityListResult GetCapability(
            string capabilityReference,
            string version,
            string name,
            bool isFoundation)
        {
            var capability = new Mock<ICapabilityListResult>();
            capability.Setup(c => c.CapabilityReference).Returns(capabilityReference);
            capability.Setup(c => c.Version).Returns(version);
            capability.Setup(c => c.Name).Returns(name);
            capability.Setup(c => c.IsFoundation).Returns(isFoundation);
            return capability.Object;
        }
    }
}
