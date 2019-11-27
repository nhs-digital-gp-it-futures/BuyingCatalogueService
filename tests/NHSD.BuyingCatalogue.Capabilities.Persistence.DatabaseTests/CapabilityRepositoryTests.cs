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
            await Database.ClearAsync();

            TestContext testContext = new TestContext();
            _capabilityRepository = testContext.CapabilityRepository;
        }

        [Test]
        public async Task ShouldReadCapabilities_NoCapabilities()
        {
            var capabilities = await _capabilityRepository.ListAsync(new CancellationToken());

            capabilities.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldReadCapabilities_NoFrameworks()
        {
            var capabilityEntities = new List<CapabilityEntity>
            {
                CapabilityEntityBuilder.Create().WithName("Cap1").Build(),
                CapabilityEntityBuilder.Create().WithName("Cap2").Build()
            };

            foreach (var capabilityEntity in capabilityEntities)
            {
                await capabilityEntity.InsertAsync();
            }

            var capabilities = await _capabilityRepository.ListAsync(new CancellationToken());

            capabilities.Should().BeEquivalentTo(capabilityEntities.Select(ce => new
            {
                Id = ce.Id, Name = ce.Name, IsFoundation = false
            }));
        }

        [Test]
        public async Task ShouldReadCapabilities_WithFrameworks()
        {
            var capabilityEntities = new List<CapabilityEntity>
            {
                CapabilityEntityBuilder.Create().WithName("Cap1").Build(),
                CapabilityEntityBuilder.Create().WithName("Cap2").Build(),
                CapabilityEntityBuilder.Create().WithName("Cap3").Build()
            };

            foreach (var capabilityEntity in capabilityEntities)
            {
                await capabilityEntity.InsertAsync();
            }


            await FrameworkCapabilitiesEntityBuilder.Create().WithCapabilityId(capabilityEntities.First(c => c.Name == "Cap1").Id).WithIsFoundation(false).Build().InsertAsync();
            await FrameworkCapabilitiesEntityBuilder.Create().WithCapabilityId(capabilityEntities.First(c => c.Name == "Cap2").Id).WithIsFoundation(true).Build().InsertAsync();

            var capabilities = await _capabilityRepository.ListAsync(new CancellationToken());

            capabilities.Should().BeEquivalentTo(capabilityEntities.Select(ce => new
            {
                Id = ce.Id,
                Name = ce.Name,
                IsFoundation = (ce.Name == "Cap2")
            }));
        }

        [Test]
        public async Task ShouldReadCapabilities_WithFrameworks_CorrectlyOrderedByIsFoundationAndName()
        {
            var capabilityEntities = new List<CapabilityEntity>
            {
                CapabilityEntityBuilder.Create().WithName("Alpha").Build(),
                CapabilityEntityBuilder.Create().WithName("Bravo").Build(),
                CapabilityEntityBuilder.Create().WithName("Charlie").Build(),
                CapabilityEntityBuilder.Create().WithName("Delta").Build()
            };

            foreach (var capabilityEntity in capabilityEntities)
            {
                await capabilityEntity.InsertAsync();
            }

            await FrameworkCapabilitiesEntityBuilder.Create().WithCapabilityId(capabilityEntities.First(c => c.Name == "Charlie").Id).WithIsFoundation(true).Build().InsertAsync();
            await FrameworkCapabilitiesEntityBuilder.Create().WithCapabilityId(capabilityEntities.First(c => c.Name == "Delta").Id).WithIsFoundation(true).Build().InsertAsync();

            var capabilities = (await _capabilityRepository.ListAsync(new CancellationToken())).ToList();

            Assert.That(capabilities[0].Name, Is.EqualTo("Charlie"));
            Assert.That(capabilities[1].Name, Is.EqualTo("Delta"));
            Assert.That(capabilities[2].Name, Is.EqualTo("Alpha"));
            Assert.That(capabilities[3].Name, Is.EqualTo("Bravo"));
        }
    }
}
