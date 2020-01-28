using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
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
    public sealed class MarketingContactsRepositoryTests
    {
        private readonly string _supplierId = "Sup 1";
        private readonly string _solutionId1 = "Sln1";
        private readonly string _solutionId2 = "Sln2";

        private IMarketingContactRepository _marketingContactRepository;

        private TestContext _testContext;

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync().ConfigureAwait(false);

            await SupplierEntityBuilder.Create()
                .WithId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SolutionEntityBuilder.Create()
                .WithId(_solutionId1)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            _testContext = new TestContext();

            _marketingContactRepository = _testContext.MarketingContactRepository;
        }

        [Test]
        public void NullConnectorThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new MarketingContactRepository(null));
        }

        [Test]
        public async Task SingleContactShouldReturnAllDetails()
        {
            var expected = await InsertContact(_solutionId1).ConfigureAwait(false);

            var result = (await _marketingContactRepository.BySolutionIdAsync(_solutionId1, new CancellationToken())
                .ConfigureAwait(false)).ToList();
            result.Count().Should().Be(1);
            AssertEquivalent(expected, result.First());
        }

        [Test]
        public async Task MultipleContactsShouldReturnAll()
        {
            var expected1 = await InsertContact(_solutionId1).ConfigureAwait(false);
            var expected2 = await InsertContact(_solutionId1).ConfigureAwait(false);

            var result = (await _marketingContactRepository.BySolutionIdAsync(_solutionId1, new CancellationToken())
                .ConfigureAwait(false)).ToList();
            result.Count().Should().Be(2);

            AssertEquivalent(expected1, result.First());
            AssertEquivalent(expected2, result.Last());
        }

        [Test]
        public async Task OneContactPerSolutionShouldOnlyReturnRelevantContact()
        {
            await SolutionEntityBuilder.Create()
                .WithId(_solutionId2)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            var expected1 = await InsertContact(_solutionId1).ConfigureAwait(false);
            var expected2 = await InsertContact(_solutionId2).ConfigureAwait(false);

            var result = (await _marketingContactRepository.BySolutionIdAsync(_solutionId1, new CancellationToken())
                .ConfigureAwait(false)).ToList();
            result.Count().Should().Be(1);
            AssertEquivalent(expected1, result.First());
            result = (await _marketingContactRepository.BySolutionIdAsync(_solutionId2, new CancellationToken())
                .ConfigureAwait(false)).ToList();
            result.Count().Should().Be(1);
            AssertEquivalent(expected2, result.First());
        }

        [Test]
        public async Task NoContactsShouldReturnNothing()
        {
            var result = (await _marketingContactRepository.BySolutionIdAsync(_solutionId1, new CancellationToken())
                .ConfigureAwait(false)).ToList();
            result.Count.Should().Be(0);
        }

        [Test]
        public async Task RequestNonExistentSolutionIdShouldReturnNothing()
        {
            var result = (await _marketingContactRepository.BySolutionIdAsync("IAmNotAnId", new CancellationToken())
                .ConfigureAwait(false)).ToList();
            result.Count.Should().Be(0);
        }

        [Test]
        public async Task ShouldReturnCorrectId()
        {
            await MarketingContactEntityBuilder.Create()
                .WithSolutionId(_solutionId1)
                .WithFirstName("FirstName1")
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await MarketingContactEntityBuilder.Create()
                .WithSolutionId(_solutionId1)
                .WithFirstName("FirstName2")
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            var result = (await _marketingContactRepository.BySolutionIdAsync(_solutionId1, new CancellationToken())
                .ConfigureAwait(false)).ToList();

            var id = (await _testContext.DbConnector.QueryAsync<int>("SELECT [Id] FROM MarketingContact Where [FirstName] = 'FirstName2'", new CancellationToken()).ConfigureAwait(false)).Single();

            result.Single(m => m.FirstName == "FirstName2").Id.Should().Be(id);
        }

        [Test]
        public async Task InsertingContactsReplacesCurrent()
        {
            await InsertContact(_solutionId1).ConfigureAwait(false);
            await InsertContact(_solutionId1).ConfigureAwait(false);

            var expectedContacts = new List<IContact> { Mock.Of<IContact>(c => c.FirstName == "BillyBob") };
            await _marketingContactRepository.ReplaceContactsForSolution(_solutionId1, expectedContacts, new CancellationToken())
                .ConfigureAwait(false);

            var newContacts = (await _marketingContactRepository.BySolutionIdAsync(_solutionId1, new CancellationToken())
                .ConfigureAwait(false)).ToList();
            newContacts.Should().BeEquivalentTo(expectedContacts, config => config.Excluding(e => e.Name));

            newContacts.ForEach(async x => (await x.LastUpdated.SecondsFromNow().ConfigureAwait(false)).Should().BeLessOrEqualTo(5));
        }

        [Test]
        public async Task InsertingZeroContactsRemovesCurrent()
        {
            await InsertContact(_solutionId1).ConfigureAwait(false);
            await InsertContact(_solutionId1).ConfigureAwait(false);
            var expectedContacts = new List<IContact>();

            await _marketingContactRepository.ReplaceContactsForSolution(_solutionId1, expectedContacts, new CancellationToken())
                .ConfigureAwait(false);
            var newContacts = (await _marketingContactRepository.BySolutionIdAsync(_solutionId1, new CancellationToken())
                .ConfigureAwait(false)).ToList();
            newContacts.Count.Should().Be(0);
        }

        [Test]
        public async Task InsertingBadDataThrowsAndDoesNotUpdate()
        {
            var expectedContacts = new List<MarketingContactEntity> { await InsertContact(_solutionId1).ConfigureAwait(false), await InsertContact(_solutionId1).ConfigureAwait(false) };
            var badData = new List<IContact> { Mock.Of<IContact>(c => c.FirstName == "IAmLongerThan35CharactersAndSoIShouldFail") };

            Assert.ThrowsAsync<SqlException>(() => _marketingContactRepository.ReplaceContactsForSolution(_solutionId1, badData, new CancellationToken()));
            var newContacts = (await _marketingContactRepository.BySolutionIdAsync(_solutionId1, new CancellationToken())
                .ConfigureAwait(false)).ToList();

            newContacts.Should().BeEquivalentTo(expectedContacts, config => config.Excluding(c => c.LastUpdated).Excluding(c => c.LastUpdatedBy));
        }

        private async Task<MarketingContactEntity> InsertContact(string solutionId)
        {
            var expected1 = MarketingContactEntityBuilder.Create()
                .WithSolutionId(solutionId)
                .WithFirstName(Guid.NewGuid().ToString().Substring(0, 25))
                .WithLastName(Guid.NewGuid().ToString().Substring(0, 25))
                .WithDepartment(Guid.NewGuid().ToString().Substring(0, 25))
                .WithEmail(Guid.NewGuid().ToString().Substring(0, 25))
                .WithPhoneNumber(Guid.NewGuid().ToString().Substring(0, 25))
                .Build();

            await expected1.InsertAsync().ConfigureAwait(false);

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
