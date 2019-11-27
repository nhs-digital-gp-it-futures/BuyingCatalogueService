using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    [TestFixture]
    public sealed class MarketingContactsRepositoryTests
    {
        private readonly Guid _org1Id = Guid.NewGuid();
        private readonly string _supplierId = "Sup 1";
        private readonly string _solutionId1 = "Sln1";
        private readonly string _solutionId2 = "Sln2";

        private IMarketingContactRepository _marketingContactRepository;

        private TestContext _testContext;

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

            _testContext = new TestContext();
            _marketingContactRepository = _testContext.MarketingContactRepository;
        }

        [Test]
        public async Task SingleContactShouldReturnAllDetails()
        {
            var expected = await InsertContact(_solutionId1);

            var result = (await _marketingContactRepository.BySolutionIdAsync(_solutionId1, new CancellationToken())).ToList();
            result.Count().Should().Be(1);
            AssertEquivalent(expected, result.First());
        }

        [Test]
        public async Task MultipleContactsShouldReturnAll()
        {
            var expected1 = await InsertContact(_solutionId1);
            var expected2 = await InsertContact(_solutionId1);

            var result = (await _marketingContactRepository.BySolutionIdAsync(_solutionId1, new CancellationToken())).ToList();
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

            var result = (await _marketingContactRepository.BySolutionIdAsync(_solutionId1, new CancellationToken())).ToList();
            result.Count().Should().Be(1);
            AssertEquivalent(expected1, result.First());
            result = (await _marketingContactRepository.BySolutionIdAsync(_solutionId2, new CancellationToken())).ToList();
            result.Count().Should().Be(1);
            AssertEquivalent(expected2, result.First());
        }

        [Test]
        public async Task NoContactsShouldReturnNothing()
        {
            var result = (await _marketingContactRepository.BySolutionIdAsync(_solutionId1, new CancellationToken())).ToList();
            result.Count.Should().Be(0);
        }

        [Test]
        public async Task RequestNonExistentSolutionIdShouldReturnNothing()
        {
            var result = (await _marketingContactRepository.BySolutionIdAsync("IAmNotAnId", new CancellationToken())).ToList();
            result.Count.Should().Be(0);
        }

        [Test]
        public async Task ShouldReturnCorrectId()
        {
            await MarketingContactEntityBuilder.Create()
                .WithSolutionId(_solutionId1)
                .WithFirstName("FirstName1")
                .Build()
                .InsertAsync();

            await MarketingContactEntityBuilder.Create()
                .WithSolutionId(_solutionId1)
                .WithFirstName("FirstName2")
                .Build()
                .InsertAsync();

            var result = (await _marketingContactRepository.BySolutionIdAsync(_solutionId1, new CancellationToken())).ToList();

            var id = (await _testContext.DbConnector.QueryAsync<int>(new CancellationToken(),
                "SELECT [Id] FROM MarketingContact Where [FirstName] = 'FirstName2'")).Single();

            result.Single(m => m.FirstName == "FirstName2").Id.Should().Be(id);
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
