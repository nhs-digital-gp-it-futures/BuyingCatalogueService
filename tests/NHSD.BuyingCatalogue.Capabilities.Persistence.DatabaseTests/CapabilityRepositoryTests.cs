using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.Capabilities.Contracts.Persistence;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Capabilities.Persistence.DatabaseTests
{
    [TestFixture]
    public class CapabilityRepositoryTests
    {
        private ICapabilityRepository _capabilityRepository;

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync().ConfigureAwait(false);

            TestContext testContext = new TestContext();
            _capabilityRepository = testContext.CapabilityRepository;
        }

        [Test]
        public async Task ShouldReadCapabilitiesNoCapabilities()
        {
            var capabilities = await _capabilityRepository.ListAsync(new CancellationToken()).ConfigureAwait(false);

            capabilities.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldReadCapabilitiesNoFrameworks()
        {
            var capabilityEntities = new List<CapabilityEntity>
            {
                CapabilityEntityBuilder.Create().WithName("Cap1").Build(),
                CapabilityEntityBuilder.Create().WithName("Cap2").Build(),
            };

            foreach (var capabilityEntity in capabilityEntities)
            {
                await capabilityEntity.InsertAsync().ConfigureAwait(false);
            }

            var capabilities = await _capabilityRepository.ListAsync(new CancellationToken()).ConfigureAwait(false);

            capabilities.Should().BeEquivalentTo(capabilityEntities.Select(ce => new
            {
                CapabilityReference = ce.CapabilityRef,
                ce.Version,
                ce.Name,
                IsFoundation = false,
            }));
        }

        [Test]
        public async Task ShouldReadCapabilitiesWithFrameworks()
        {
            var capabilityEntities = new List<CapabilityEntity>
            {
                CapabilityEntityBuilder.Create().WithName("Cap1").Build(),
                CapabilityEntityBuilder.Create().WithName("Cap2").Build(),
                CapabilityEntityBuilder.Create().WithName("Cap3").Build(),
            };

            foreach (var capabilityEntity in capabilityEntities)
            {
                await capabilityEntity.InsertAsync().ConfigureAwait(false);
            }

            await FrameworkCapabilitiesEntityBuilder.Create().WithCapabilityId(capabilityEntities.First(c => c.Name == "Cap1").Id).WithIsFoundation(false).Build().InsertAsync().ConfigureAwait(false);
            await FrameworkCapabilitiesEntityBuilder.Create().WithCapabilityId(capabilityEntities.First(c => c.Name == "Cap2").Id).WithIsFoundation(true).Build().InsertAsync().ConfigureAwait(false);

            var capabilities = await _capabilityRepository.ListAsync(new CancellationToken()).ConfigureAwait(false);

            capabilities.Should().BeEquivalentTo(capabilityEntities.Select(ce => new
            {
                CapabilityReference = ce.CapabilityRef,
                ce.Version,
                ce.Name,
                IsFoundation = (ce.Name == "Cap2"),
            }));
        }

        [Test]
        public async Task ShouldReadCapabilitiesWithFrameworksCorrectlyOrderedByName()
        {
            var capabilityEntities = new List<CapabilityEntity>
            {
                CapabilityEntityBuilder.Create().WithName("Bravo").Build(),
                CapabilityEntityBuilder.Create().WithName("Alpha").Build(),
                CapabilityEntityBuilder.Create().WithName("Delta").Build(),
                CapabilityEntityBuilder.Create().WithName("Charlie").Build(),
            };

            foreach (var capabilityEntity in capabilityEntities)
            {
                await capabilityEntity.InsertAsync().ConfigureAwait(false);
            }

            await FrameworkCapabilitiesEntityBuilder.Create().WithCapabilityId(capabilityEntities.First(c => c.Name == "Charlie").Id).WithIsFoundation(true).Build().InsertAsync().ConfigureAwait(false);
            await FrameworkCapabilitiesEntityBuilder.Create().WithCapabilityId(capabilityEntities.First(c => c.Name == "Delta").Id).WithIsFoundation(true).Build().InsertAsync().ConfigureAwait(false);

            var capabilities = (await _capabilityRepository.ListAsync(new CancellationToken()).ConfigureAwait(false)).ToList();

            Assert.That(capabilities[0].Name, Is.EqualTo("Alpha"));
            Assert.That(capabilities[1].Name, Is.EqualTo("Bravo"));
            Assert.That(capabilities[2].Name, Is.EqualTo("Charlie"));
            Assert.That(capabilities[3].Name, Is.EqualTo("Delta"));
        }
    }
}
