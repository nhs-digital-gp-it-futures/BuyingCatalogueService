using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    internal sealed class SolutionEpicRepositoryTests
    {
        private const string Solution1Id = "Sln1";

        private const string SupplierId = "Sup 1";

        private const string StatusPassed = "Passed";
        private const string InvalidStatus = "Unknown";

        private static readonly List<CapabilityEntity> CapDetails = new()
        {
            CapabilityEntityBuilder.Create().WithId(Guid.NewGuid()).WithCapabilityRef("Ref1").Build(),
            CapabilityEntityBuilder.Create().WithId(Guid.NewGuid()).WithCapabilityRef("Ref2").Build(),
        };

        private readonly List<EpicEntity> epicDetails = new()
        {
            EpicEntityBuilder.Create().WithId("Epic1").WithCapabilityId(CapDetails[0].Id).Build(),
            EpicEntityBuilder.Create().WithId("Epic2").WithCapabilityId(CapDetails[1].Id).Build(),
        };

        private ISolutionEpicRepository solutionEpicRepository;
        private IEpicRepository epicRepository;
        private ISolutionEpicStatusRepository solutionEpicStatusRepository;

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
            solutionEpicRepository = testContext.SolutionEpicRepository;
            epicRepository = testContext.EpicRepository;
            solutionEpicStatusRepository = testContext.SolutionEpicStatusRepository;
        }

        [Test]
        public async Task UpdateSolutionWithOneEpicAsync()
        {
            await InsertCapabilityAsync(CapDetails[0]);
            await InsertEpicAsync(epicDetails[0]);

            var expectedClaimedEpic = new List<IClaimedEpicResult>
            {
                Mock.Of<IClaimedEpicResult>(e => e.EpicId == epicDetails[0].Id && e.StatusName == StatusPassed),
            };

            // ReSharper disable once PossibleUnintendedReferenceComparison
            Expression<Func<IUpdateClaimedEpicListRequest, bool>> request = r =>
                r.ClaimedEpics == expectedClaimedEpic;

            await solutionEpicRepository.UpdateSolutionEpicAsync(
                Solution1Id,
                Mock.Of(request),
                CancellationToken.None);

            var solutionEpics = (await SolutionEpicEntity.FetchAllEpicIdsForSolutionAsync(Solution1Id)).ToList();
            solutionEpics.Count.Should().Be(1);
            solutionEpics[0].Should().Be(epicDetails[0].Id);
        }

        [Test]
        public async Task UpdateSolutionWithMultipleEpicsAsync()
        {
            await InsertCapabilityAsync(CapDetails[0]);
            await InsertCapabilityAsync(CapDetails[1]);

            await InsertEpicAsync(epicDetails[0]);
            await InsertEpicAsync(epicDetails[1]);

            var expectedEpic = new List<IClaimedEpicResult>
            {
                Mock.Of<IClaimedEpicResult>(e => e.EpicId == epicDetails[0].Id && e.StatusName == StatusPassed),
                Mock.Of<IClaimedEpicResult>(e => e.EpicId == epicDetails[1].Id && e.StatusName == StatusPassed),
            };

            // ReSharper disable once PossibleUnintendedReferenceComparison
            Expression<Func<IUpdateClaimedEpicListRequest, bool>> request = r =>
                r.ClaimedEpics == expectedEpic;

            await solutionEpicRepository.UpdateSolutionEpicAsync(
                Solution1Id,
                Mock.Of(request),
                CancellationToken.None);

            var solutionEpics = (await SolutionEpicEntity.FetchAllEpicIdsForSolutionAsync(Solution1Id)).ToList();
            solutionEpics.Count.Should().Be(2);
            solutionEpics.Should().BeEquivalentTo(
                epicDetails.Select(ed => ed.Id),
                options => options.WithoutStrictOrdering());
        }

        [Test]
        public void ShouldThrowIfEpicRequestIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => solutionEpicRepository.UpdateSolutionEpicAsync(
                Solution1Id,
                null,
                CancellationToken.None));
        }

        [Test]
        public async Task ValidateIfNoEpicsExistAndStatusValidationThenCountIsZero()
        {
            var epics = new List<IClaimedEpicResult>
            {
                Mock.Of<IClaimedEpicResult>(e => e.EpicId == epicDetails[0].Id && e.StatusName == StatusPassed),
            };

            var epicIdCount = await epicRepository.CountMatchingEpicIdsAsync(
                epics.Select(r => r.EpicId),
                It.IsAny<CancellationToken>());

            var epicStatusNameCount = await solutionEpicStatusRepository.CountMatchingEpicStatusAsync(
                epics.Select(r => r.StatusName),
                It.IsAny<CancellationToken>());

            epicIdCount.Should().Be(0);
            epicStatusNameCount.Should().Be(1);
        }

        [Test]
        public async Task ValidateIfEpicsExistButStatusDoesNotExist()
        {
            await InsertCapabilityAsync(CapDetails[0]);
            await InsertEpicAsync(epicDetails[0]);

            var epics = new List<IClaimedEpicResult>
            {
                Mock.Of<IClaimedEpicResult>(e => e.EpicId == epicDetails[0].Id && e.StatusName == InvalidStatus),
            };

            var epicIdCount = await epicRepository.CountMatchingEpicIdsAsync(
                epics.Select(r => r.EpicId),
                It.IsAny<CancellationToken>());

            var epicStatusNameCount = await solutionEpicStatusRepository.CountMatchingEpicStatusAsync(
                epics.Select(r => r.StatusName),
                It.IsAny<CancellationToken>());

            epicIdCount.Should().Be(1);
            epicStatusNameCount.Should().Be(0);
        }

        private static async Task InsertCapabilityAsync(EntityBase capabilityEntity)
        {
            await capabilityEntity.InsertAsync();
        }

        private static async Task InsertEpicAsync(EntityBase epicEntity)
        {
            await epicEntity.InsertAsync();
        }
    }
}
