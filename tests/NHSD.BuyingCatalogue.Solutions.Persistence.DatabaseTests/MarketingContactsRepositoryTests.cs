using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.Repositories;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests
{
    [TestFixture]
    internal sealed class MarketingContactsRepositoryTests
    {
        private const string SupplierId = "Sup 1";
        private const string SolutionId1 = "Sln1";
        private const string SolutionId2 = "Sln2";

        private IMarketingContactRepository marketingContactRepository;

        private TestContext testContext;

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync();

            await SupplierEntityBuilder.Create()
                .WithId(SupplierId)
                .Build()
                .InsertAsync();

            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(SolutionId1)
                .WithName(SolutionId1)
                .WithPublishedStatusId((int)PublishedStatus.Published)
                .WithSupplierId(SupplierId)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(SolutionId1)
                .Build()
                .InsertAsync();

            testContext = new TestContext();

            marketingContactRepository = testContext.MarketingContactRepository;
        }

        [Test]
        public void NullConnectorThrows()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new MarketingContactRepository(null));
        }

        [Test]
        public async Task SingleContactShouldReturnAllDetails()
        {
            var expected = await InsertContact(SolutionId1);

            var result = (await marketingContactRepository.BySolutionIdAsync(SolutionId1, CancellationToken.None)).ToList();
            result.Count.Should().Be(1);
            AssertEquivalent(expected, result.First());
        }

        [Test]
        public async Task MultipleContactsShouldReturnAll()
        {
            var expected1 = await InsertContact(SolutionId1);
            var expected2 = await InsertContact(SolutionId1);

            var result = (await marketingContactRepository.BySolutionIdAsync(SolutionId1, CancellationToken.None)).ToList();
            result.Count.Should().Be(2);

            AssertEquivalent(expected1, result.First());
            AssertEquivalent(expected2, result.Last());
        }

        [Test]
        public async Task OneContactPerSolutionShouldOnlyReturnRelevantContact()
        {
            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(SolutionId2)
                .WithName(SolutionId2)
                .WithPublishedStatusId((int)PublishedStatus.Published)
                .WithSupplierId(SupplierId)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(SolutionId2)
                .Build()
                .InsertAsync();

            var expected1 = await InsertContact(SolutionId1);
            var expected2 = await InsertContact(SolutionId2);

            var result = (await marketingContactRepository.BySolutionIdAsync(SolutionId1, CancellationToken.None)).ToList();
            result.Count.Should().Be(1);
            AssertEquivalent(expected1, result.First());
            result = (await marketingContactRepository.BySolutionIdAsync(SolutionId2, CancellationToken.None)).ToList();
            result.Count.Should().Be(1);
            AssertEquivalent(expected2, result.First());
        }

        [Test]
        public async Task NoContactsShouldReturnNothing()
        {
            var result = (await marketingContactRepository.BySolutionIdAsync(SolutionId1, CancellationToken.None)).ToList();
            result.Count.Should().Be(0);
        }

        [Test]
        public async Task RequestNonExistentSolutionIdShouldReturnNothing()
        {
            var result = (await marketingContactRepository.BySolutionIdAsync("IAmNotAnId", CancellationToken.None)).ToList();
            result.Count.Should().Be(0);
        }

        [Test]
        public async Task ShouldReturnCorrectId()
        {
            await MarketingContactEntityBuilder.Create()
                .WithSolutionId(SolutionId1)
                .WithFirstName("FirstName1")
                .Build()
                .InsertAsync();

            await MarketingContactEntityBuilder.Create()
                .WithSolutionId(SolutionId1)
                .WithFirstName("FirstName2")
                .Build()
                .InsertAsync();

            var result = (await marketingContactRepository.BySolutionIdAsync(SolutionId1, CancellationToken.None)).ToList();

            var id = (await testContext.DbConnector.QueryAsync<int>(
                "SELECT Id FROM dbo.MarketingContact WHERE FirstName = 'FirstName2';", CancellationToken.None)).Single();

            result.Single(m => m.FirstName == "FirstName2").Id.Should().Be(id);
        }

        [Test]
        public async Task InsertingContactsReplacesCurrent()
        {
            await InsertContact(SolutionId1);
            await InsertContact(SolutionId1);

            var expectedContacts = new List<IContact> { Mock.Of<IContact>(c => c.FirstName == "BillyBob") };
            await marketingContactRepository.ReplaceContactsForSolution(
                SolutionId1,
                expectedContacts,
                CancellationToken.None);

            var newContacts = (await marketingContactRepository.BySolutionIdAsync(
                SolutionId1,
                CancellationToken.None)).ToList();

            newContacts.Should().BeEquivalentTo(expectedContacts, config => config.Excluding(e => e.Name));

            newContacts.ForEach(async r => (await r.LastUpdated.SecondsFromNow()).Should().BeLessOrEqualTo(5));
        }

        [Test]
        public async Task InsertingZeroContactsRemovesCurrent()
        {
            await InsertContact(SolutionId1);
            await InsertContact(SolutionId1);

            await marketingContactRepository.ReplaceContactsForSolution(
                SolutionId1,
                Array.Empty<IContact>(),
                CancellationToken.None);

            var newContacts = (await marketingContactRepository.BySolutionIdAsync(
                SolutionId1,
                CancellationToken.None)).ToList();

            newContacts.Count.Should().Be(0);
        }

        [Test]
        public async Task InsertingBadDataThrowsAndDoesNotUpdate()
        {
            var expectedContacts = new List<MarketingContactEntity>
            {
                await InsertContact(SolutionId1),
                await InsertContact(SolutionId1),
            };

            var badData = new List<IContact>
            {
                Mock.Of<IContact>(c => c.FirstName == "IAmLongerThan35CharactersAndSoIShouldFail"),
            };

            Assert.ThrowsAsync<SqlException>(() => marketingContactRepository.ReplaceContactsForSolution(
                SolutionId1,
                badData,
                CancellationToken.None));

            var newContacts = (await marketingContactRepository.BySolutionIdAsync(
                SolutionId1,
                CancellationToken.None)).ToList();

            static EquivalencyAssertionOptions<MarketingContactEntity> AssertionConfig(
                EquivalencyAssertionOptions<MarketingContactEntity> config)
            {
                return config.Excluding(c => c.LastUpdated).Excluding(c => c.LastUpdatedBy);
            }

            newContacts.Should().BeEquivalentTo(expectedContacts, AssertionConfig);
        }

        private static async Task<MarketingContactEntity> InsertContact(string solutionId)
        {
            var expected1 = MarketingContactEntityBuilder.Create()
                .WithSolutionId(solutionId)
                .WithFirstName(Guid.NewGuid().ToString().Substring(0, 25))
                .WithLastName(Guid.NewGuid().ToString().Substring(0, 25))
                .WithDepartment(Guid.NewGuid().ToString().Substring(0, 25))
                .WithEmail(Guid.NewGuid().ToString().Substring(0, 25))
                .WithPhoneNumber(Guid.NewGuid().ToString().Substring(0, 25))
                .Build();

            await expected1.InsertAsync();

            return expected1;
        }

        private static void AssertEquivalent(MarketingContactEntity expected, IMarketingContactResult actual)
        {
            static EquivalencyAssertionOptions<MarketingContactEntity> AssertionConfig(
                EquivalencyAssertionOptions<MarketingContactEntity> config)
            {
                return config.Excluding(c => c.LastUpdatedBy).Excluding(c => c.LastUpdated);
            }

            actual.Should().BeEquivalentTo(expected, AssertionConfig);
        }
    }
}
