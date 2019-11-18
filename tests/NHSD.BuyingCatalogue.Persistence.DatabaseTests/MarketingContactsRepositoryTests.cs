using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Persistence.Repositories;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    [TestFixture]
    class MarketingContactsRepositoryTests
    {
        private readonly Guid _org1Id = Guid.NewGuid();
        private readonly string _supplierId = "Sup 1";
        private readonly string _solutionId1 = "Sol 1";
        private readonly string _solutionId2 = "Sol 2";
        private IMarketingContactRepository _repository;

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync();

            await OrganisationEntityBuilder.Create()
                .WithId(_org1Id)
                .Build()
                .InsertAsync();

            await SupplierEntityBuilder.Create()
                .WithId(_supplierId)
                .WithOrganisation(_org1Id)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(_solutionId1)
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(a => a["ConnectionStrings:BuyingCatalogue"]).Returns(ConnectionStrings.ServiceConnectionString());

            _repository = new MarketingContactRepository(new DbConnectionFactory(configuration.Object));
        }

        [Test]
        public async Task SingleContactShouldReturnAllDetails()
        {
            var expected = await InsertContact(_solutionId1);

            var result = (await _repository.BySolutionIdAsync(_solutionId1, new CancellationToken())).ToList();
            result.Count().Should().Be(1);
            AssertEquivalent(expected, result.First());
        }

        [Test]
        public async Task MultipleContactsShouldReturnAll()
        {
            var expected1 = await InsertContact(_solutionId1);
            var expected2 = await InsertContact(_solutionId1);

            var result = (await _repository.BySolutionIdAsync(_solutionId1, new CancellationToken())).ToList();
            result.Count().Should().Be(2);

            AssertEquivalent(expected1, result.First());
            AssertEquivalent(expected2, result.Last());
        }

        [Test]
        public async Task OneContactPerSolutionShouldOnlyReturnRelevantContact()
        {
            await SolutionEntityBuilder.Create()
                .WithId(_solutionId2)
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            var expected1 = await InsertContact(_solutionId1);
            var expected2 = await InsertContact(_solutionId2);

            var result = (await _repository.BySolutionIdAsync(_solutionId1, new CancellationToken())).ToList();
            result.Count().Should().Be(1);
            AssertEquivalent(expected1, result.First());
            result = (await _repository.BySolutionIdAsync(_solutionId2, new CancellationToken())).ToList();
            result.Count().Should().Be(1);
            AssertEquivalent(expected2, result.First());
        }

        [Test]
        public async Task NoContactsShouldReturnNothing()
        {
            var result = (await _repository.BySolutionIdAsync(_solutionId1, new CancellationToken())).ToList();
            result.Count.Should().Be(0);
        }

        [Test]
        public async Task RequestNonExistentSolutionIdShouldReturnNothing()
        {
            var result = (await _repository.BySolutionIdAsync("IAmNotAnId", new CancellationToken())).ToList();
            result.Count.Should().Be(0);
        }

        private async Task<MarketingContactEntity> InsertContact(string solutionId)
        {
            var expected1 = MarketingContactEntityBuilder.Create()
                .WithSolutionId(solutionId)
                .WithFirstName($"{solutionId}FirstName")
                .WithLastName($"{solutionId}LastName")
                .WithDepartment($"{solutionId}Dept")
                .WithEmail($"{solutionId}Email")
                .WithPhoneNumber($"{solutionId}Number")
                .Build();

            await expected1.InsertAsync();

            return expected1;
        }

        private void AssertEquivalent(MarketingContactEntity expected, IMarketingContactResult actual)
        {
            actual.Should().BeEquivalentTo(expected, config => config
                .Excluding(x => x.LastUpdatedBy)
                .Excluding(x => x.LastUpdated));
        }
    }
}
