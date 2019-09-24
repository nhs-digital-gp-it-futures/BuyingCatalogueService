using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Persistence.Repositories;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    [TestFixture]
    public class CapabilityRepositoryTests
    {
        private Mock<IConfiguration> _configuration;

        private CapabilityRepository _capabilityRepository;

        [SetUp]
        public async Task Setup()
        {
            await Database.Clear();
            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(a => a["ConnectionStrings:BuyingCatalogue"]).Returns(Database.ConnectionString);

            _capabilityRepository = new CapabilityRepository(new DbConnectionFactory(_configuration.Object));
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

            capabilityEntities.ForEach(async c => await c.Insert());

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

            capabilityEntities.ForEach(async c => await c.Insert());

            await FrameworkCapabilitiesEntityBuilder.Create().WithCapabilityId(capabilityEntities.First(c => c.Name == "Cap1").Id).WithIsFoundation(false).Build().Insert();
            await FrameworkCapabilitiesEntityBuilder.Create().WithCapabilityId(capabilityEntities.First(c => c.Name == "Cap2").Id).WithIsFoundation(true).Build().Insert();

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

            capabilityEntities.ForEach(async c => await c.Insert());

            await FrameworkCapabilitiesEntityBuilder.Create().WithCapabilityId(capabilityEntities.First(c => c.Name == "Charlie").Id).WithIsFoundation(true).Build().Insert();
            await FrameworkCapabilitiesEntityBuilder.Create().WithCapabilityId(capabilityEntities.First(c => c.Name == "Delta").Id).WithIsFoundation(true).Build().Insert();

            var capabilities = (await _capabilityRepository.ListAsync(new CancellationToken())).ToList();

            Assert.That(capabilities[0].Name, Is.EqualTo("Charlie"));
            Assert.That(capabilities[1].Name, Is.EqualTo("Delta"));
            Assert.That(capabilities[2].Name, Is.EqualTo("Alpha"));
            Assert.That(capabilities[3].Name, Is.EqualTo("Bravo"));
        }
    }
}
