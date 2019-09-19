using System.Data.Common;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Moq;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Persistence.Repositories;
using FluentAssertions;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NHSD.BuyingCatalogue.Testing.Data;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
	public class CapabilityRepositoryTests
    {
        private Mock<IConfiguration> _configuration;

        private CapabilityRepository _capabilityRepository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Database.Create();

            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(a => a["ConnectionStrings:BuyingCatalogue"]).Returns(Database.ConnectionString);

            _capabilityRepository = new CapabilityRepository(new DbConnectionFactory(_configuration.Object));
        }

        [SetUp]
		public void Setup()
		{
            Database.Clear();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Database.Drop();
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
                Id = ce.Id,
                Name = ce.Name,
                IsFoundation = false
            }));
        }

        //framework cap, is foundation

        //ordering , isfoundation, name
    }
}
