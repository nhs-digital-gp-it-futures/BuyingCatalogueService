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
    internal sealed class CapabilityRepositoryTests
    {
        private ICapabilityRepository capabilityRepository;

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync();

            TestContext testContext = new TestContext();
            capabilityRepository = testContext.CapabilityRepository;
        }

        [Test]
        public async Task ShouldReadCapabilitiesNoCapabilities()
        {
            var capabilities = await capabilityRepository.ListAsync(CancellationToken.None);

            capabilities.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldReadGPITFuturesCapabilitiesNoFrameworks()
        {
            var capabilityEntities = new List<CapabilityEntity>
            {
                CapabilityEntityBuilder.Create().WithName("Cap1").WithCategoryId(1).Build(),
                CapabilityEntityBuilder.Create().WithName("Cap2").WithCategoryId(1).Build(),
            };

            foreach (var capabilityEntity in capabilityEntities)
            {
                await capabilityEntity.InsertAsync();
            }

            foreach (var entity in new List<CapabilityEntity>()
            {
                CapabilityEntityBuilder.Create().WithName("Cap3").WithCategoryId(2).Build(),
                CapabilityEntityBuilder.Create().WithName("Cap4").WithCategoryId(3).Build(),
            })
            {
                await entity.InsertAsync();
            }

            var capabilities = await capabilityRepository.ListAsync(CancellationToken.None);

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
                CapabilityEntityBuilder.Create().WithName("Cap1").WithCategoryId(1).Build(),
                CapabilityEntityBuilder.Create().WithName("Cap2").WithCategoryId(1).Build(),
                CapabilityEntityBuilder.Create().WithName("Cap3").WithCategoryId(1).Build(),
            };

            foreach (var capabilityEntity in capabilityEntities)
            {
                await capabilityEntity.InsertAsync();
            }

            await FrameworkCapabilitiesEntityBuilder.Create()
                .WithCapabilityId(capabilityEntities.First(c => c.Name == "Cap1").Id)
                .WithIsFoundation(false)
                .Build()
                .InsertAsync();

            await FrameworkCapabilitiesEntityBuilder.Create()
                .WithCapabilityId(capabilityEntities.First(c => c.Name == "Cap2").Id)
                .WithIsFoundation(true)
                .Build()
                .InsertAsync();

            var capabilities = await capabilityRepository.ListAsync(CancellationToken.None);

            capabilities.Should().BeEquivalentTo(capabilityEntities.Select(ce => new
            {
                CapabilityReference = ce.CapabilityRef,
                ce.Version,
                ce.Name,
                IsFoundation = ce.Name == "Cap2",
            }));
        }

        [Test]
        public async Task ShouldReadGPITFuturesCapabilitiesWithFrameworksCorrectlyOrderedByName()
        {
            var capabilityEntities = new List<CapabilityEntity>
            {
                CapabilityEntityBuilder.Create().WithName("Bravo").WithCategoryId(1).Build(),
                CapabilityEntityBuilder.Create().WithName("Alpha").WithCategoryId(1).Build(),
                CapabilityEntityBuilder.Create().WithName("Delta").WithCategoryId(1).Build(),
                CapabilityEntityBuilder.Create().WithName("Charlie").WithCategoryId(1).Build(),
            };

            foreach (var capabilityEntity in capabilityEntities)
            {
                await capabilityEntity.InsertAsync();
            }

            foreach (var entity in new List<CapabilityEntity>
            {
                CapabilityEntityBuilder.Create().WithName("Echo").WithCategoryId(3).Build(),
                CapabilityEntityBuilder.Create().WithName("FoxTrot").WithCategoryId(0).Build(),
                CapabilityEntityBuilder.Create().WithName("Golf").WithCategoryId(2).Build(),
            })
            {
                await entity.InsertAsync();
            }

            await FrameworkCapabilitiesEntityBuilder.Create()
                .WithCapabilityId(capabilityEntities.First(c => c.Name == "Charlie").Id)
                .WithIsFoundation(true)
                .Build()
                .InsertAsync();

            await FrameworkCapabilitiesEntityBuilder.Create()
                .WithCapabilityId(capabilityEntities.First(c => c.Name == "Delta").Id)
                .WithIsFoundation(true)
                .Build()
                .InsertAsync();

            var capabilities = (await capabilityRepository.ListAsync(CancellationToken.None)).ToList();

            Assert.That(capabilities[0].Name, Is.EqualTo("Alpha"));
            Assert.That(capabilities[1].Name, Is.EqualTo("Bravo"));
            Assert.That(capabilities[2].Name, Is.EqualTo("Charlie"));
            Assert.That(capabilities[3].Name, Is.EqualTo("Delta"));
        }
    }
}
