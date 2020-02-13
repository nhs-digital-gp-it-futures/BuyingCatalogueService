using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests
{
    [TestFixture]
    public sealed class SolutionEpicRepositoryTests
    {
        private const string Solution1Id = "Sln1";

        private const string SupplierId = "Sup 1";

        private const string StatusPassed = "Passed";

        private ISolutionEpicRepository _solutionEpicRepository;

        private static readonly List<CapabilityEntity> _capDetails = new List<CapabilityEntity>
        {
            CapabilityEntityBuilder.Create().WithId(Guid.NewGuid()).WithCapabilityRef("Ref1").Build(),
            CapabilityEntityBuilder.Create().WithId(Guid.NewGuid()).WithCapabilityRef("Ref2").Build(),
        };

        private readonly List<EpicEntity> _epicDetails = new List<EpicEntity>()
        {
            EpicEntityBuilder.Create().WithId("Epic1").WithCapabilityId(_capDetails[0].Id).Build(),
            EpicEntityBuilder.Create().WithId("Epic2").WithCapabilityId(_capDetails[1].Id).Build()
        };

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync().ConfigureAwait(false);

            await SupplierEntityBuilder.Create()
                .WithId(SupplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SolutionEntityBuilder.Create()
                .WithId(Solution1Id)
                .WithSupplierId(SupplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            TestContext testContext = new TestContext();
            _solutionEpicRepository = testContext.SolutionEpicRepository;
        }

        [Test]
        public async Task UpdateSolutionWithOneEpicAsync()
        {
            await InsertCapabilityAsync(_capDetails[0]).ConfigureAwait(false);
            await InsertEpicAsync(_epicDetails[0]).ConfigureAwait(false);

            var expectedClaimedEpic = new List<IClaimedEpicResult>
            {
                Mock.Of<IClaimedEpicResult>(e => e.EpicId == _epicDetails[0].Id && e.StatusName == StatusPassed)
            };

            await _solutionEpicRepository
                .UpdateSolutionEpicAsync(Solution1Id,
                    Mock.Of<IUpdateClaimedEpicListRequest>(c => c.ClaimedEpics == expectedClaimedEpic), new CancellationToken()).ConfigureAwait(false);

            var solutionEpics = (await SolutionEpicEntity.FetchAllEpicIdsForSolutionAsync(Solution1Id).ConfigureAwait(false)).ToList();
            solutionEpics.Count().Should().Be(1);
            solutionEpics[0].Should().Be(_epicDetails[0].Id);
        }

        [Test]
        public async Task UpdateSolutionWithMultipleEpicsAsync()
        {
            await InsertCapabilityAsync(_capDetails[0]).ConfigureAwait(false);
            await InsertCapabilityAsync(_capDetails[1]).ConfigureAwait(false);

            await InsertEpicAsync(_epicDetails[0]).ConfigureAwait(false);
            await InsertEpicAsync(_epicDetails[1]).ConfigureAwait(false);

            var expectedEpic = new List<IClaimedEpicResult>
            {
                Mock.Of<IClaimedEpicResult>(e => e.EpicId == _epicDetails[0].Id && e.StatusName == StatusPassed),
                Mock.Of<IClaimedEpicResult>(e => e.EpicId == _epicDetails[1].Id && e.StatusName == StatusPassed)
            };

            await _solutionEpicRepository
                .UpdateSolutionEpicAsync(Solution1Id,
                    Mock.Of<IUpdateClaimedEpicListRequest>(c => c.ClaimedEpics == expectedEpic), new CancellationToken()).ConfigureAwait(false);

            var solutionEpics = (await SolutionEpicEntity.FetchAllEpicIdsForSolutionAsync(Solution1Id).ConfigureAwait(false)).ToList();
            solutionEpics.Count().Should().Be(2);
            solutionEpics.Should().BeEquivalentTo(_epicDetails.Select(ed=>ed.Id),
                options => options.WithoutStrictOrdering());
            
        }

        [Test]
        public void ShouldThrowIfEpicRequestIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _solutionEpicRepository.UpdateSolutionEpicAsync(Solution1Id, null, new CancellationToken()));
        }

        private async Task InsertCapabilityAsync(CapabilityEntity capabilityEntity)
        {
            await capabilityEntity.InsertAsync().ConfigureAwait(false);
        }

        private async Task InsertEpicAsync(EpicEntity epicEntity)
        {
            await epicEntity.InsertAsync().ConfigureAwait(false);
        }
    }
}
