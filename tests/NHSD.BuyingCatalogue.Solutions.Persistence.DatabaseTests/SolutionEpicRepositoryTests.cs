using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Contracts;
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
        private const string InvalidStatus = "Unknown";

        private ISolutionEpicRepository _solutionEpicRepository;
        private IEpicRepository _epicRepository;
        private ISolutionEpicStatusRepository _solutionEpicStatusRepository;

        private static readonly List<CapabilityEntity> _capDetails = new()
        {
            CapabilityEntityBuilder.Create().WithId(Guid.NewGuid()).WithCapabilityRef("Ref1").Build(),
            CapabilityEntityBuilder.Create().WithId(Guid.NewGuid()).WithCapabilityRef("Ref2").Build(),
        };

        private readonly List<EpicEntity> _epicDetails = new()
        {
            EpicEntityBuilder.Create().WithId("Epic1").WithCapabilityId(_capDetails[0].Id).Build(),
            EpicEntityBuilder.Create().WithId("Epic2").WithCapabilityId(_capDetails[1].Id).Build(),
        };

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
                .WithCatalogueItemId(Solution1Id)
                .WithName(Solution1Id)
                .WithPublishedStatusId((int)PublishedStatus.Published)
                .WithSupplierId(SupplierId)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(Solution1Id)
                .Build()
                .InsertAsync();

            TestContext testContext = new TestContext();
            _solutionEpicRepository = testContext.SolutionEpicRepository;
            _epicRepository = testContext.EpicRepository;
            _solutionEpicStatusRepository = testContext.SolutionEpicStatusRepository;
        }

        [Test]
        public async Task UpdateSolutionWithOneEpicAsync()
        {
            await InsertCapabilityAsync(_capDetails[0]);
            await InsertEpicAsync(_epicDetails[0]);

            var expectedClaimedEpic = new List<IClaimedEpicResult>
            {
                Mock.Of<IClaimedEpicResult>(e => e.EpicId == _epicDetails[0].Id && e.StatusName == StatusPassed),
            };

            await _solutionEpicRepository
                .UpdateSolutionEpicAsync(Solution1Id,
                    Mock.Of<IUpdateClaimedEpicListRequest>(c => c.ClaimedEpics == expectedClaimedEpic), new CancellationToken());

            var solutionEpics = (await SolutionEpicEntity.FetchAllEpicIdsForSolutionAsync(Solution1Id)).ToList();
            solutionEpics.Count.Should().Be(1);
            solutionEpics[0].Should().Be(_epicDetails[0].Id);
        }

        [Test]
        public async Task UpdateSolutionWithMultipleEpicsAsync()
        {
            await InsertCapabilityAsync(_capDetails[0]);
            await InsertCapabilityAsync(_capDetails[1]);

            await InsertEpicAsync(_epicDetails[0]);
            await InsertEpicAsync(_epicDetails[1]);

            var expectedEpic = new List<IClaimedEpicResult>
            {
                Mock.Of<IClaimedEpicResult>(e => e.EpicId == _epicDetails[0].Id && e.StatusName == StatusPassed),
                Mock.Of<IClaimedEpicResult>(e => e.EpicId == _epicDetails[1].Id && e.StatusName == StatusPassed),
            };

            await _solutionEpicRepository
                .UpdateSolutionEpicAsync(Solution1Id,
                    Mock.Of<IUpdateClaimedEpicListRequest>(c => c.ClaimedEpics == expectedEpic), new CancellationToken());

            var solutionEpics = (await SolutionEpicEntity.FetchAllEpicIdsForSolutionAsync(Solution1Id)).ToList();
            solutionEpics.Count.Should().Be(2);
            solutionEpics.Should().BeEquivalentTo(_epicDetails.Select(ed => ed.Id),
                options => options.WithoutStrictOrdering());
        }

        [Test]
        public void ShouldThrowIfEpicRequestIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _solutionEpicRepository.UpdateSolutionEpicAsync(Solution1Id, null, new CancellationToken()));
        }

        [Test]
        public async Task ValidateIfNoEpicsExistAndStatusValidationThenCountIsZero()
        {
            var epics = new List<IClaimedEpicResult>
            {
                Mock.Of<IClaimedEpicResult>(e => e.EpicId == _epicDetails[0].Id && e.StatusName == StatusPassed),
            };

            var epicIdCount = await _epicRepository.CountMatchingEpicIdsAsync(epics.Select(x => x.EpicId), It.IsAny<CancellationToken>());

            var epicStatusNameCount = await _solutionEpicStatusRepository.CountMatchingEpicStatusAsync(epics.Select(x => x.StatusName), It.IsAny<CancellationToken>());

            epicIdCount.Should().Be(0);
            epicStatusNameCount.Should().Be(1);
        }

        [Test]
        public async Task ValidateIfEpicsExistButStatusDoesNotExist()
        {
            await InsertCapabilityAsync(_capDetails[0]);
            await InsertEpicAsync(_epicDetails[0]);

            var epics = new List<IClaimedEpicResult>
            {
                Mock.Of<IClaimedEpicResult>(e => e.EpicId == _epicDetails[0].Id && e.StatusName == InvalidStatus),
            };

            var epicIdCount = await _epicRepository.CountMatchingEpicIdsAsync(epics.Select(x => x.EpicId), It.IsAny<CancellationToken>());

            var epicStatusNameCount = await _solutionEpicStatusRepository.CountMatchingEpicStatusAsync(epics.Select(x => x.StatusName), It.IsAny<CancellationToken>());

            epicIdCount.Should().Be(1);
            epicStatusNameCount.Should().Be(0);
        }

        private static async Task InsertCapabilityAsync(CapabilityEntity capabilityEntity)
        {
            await capabilityEntity.InsertAsync();
        }

        private static async Task InsertEpicAsync(EpicEntity epicEntity)
        {
            await epicEntity.InsertAsync();
        }
    }
}
