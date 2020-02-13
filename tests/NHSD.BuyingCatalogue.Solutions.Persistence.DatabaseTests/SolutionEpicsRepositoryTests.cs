using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests.Models;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests
{
    [TestFixture]
    public sealed class SolutionEpicsRepositoryTests
    {
        private const string Solution1Id = "Sln1";

        private const string SupplierId = "Sup 1";

        private const string StatusPassed = "Passed";

        private ISolutionEpicsRepository _solutionEpicsRepository;

        private static readonly List<CapabilityDetails> _capDetails = new List<CapabilityDetails>()
        {
            CreateCapability("Cap1", "Desc1", "Ref1", "1.0.0","http://cap1.link"),
            CreateCapability("Cap2", "Desc2", "Ref2","1.0.0","http://cap2.link")
        };

        private readonly List<EpicDetails> _epicDetails = new List<EpicDetails>()
        {
            CreateEpic("Epic1", "Name1", _capDetails[0].Id),
            CreateEpic("Epic2", "Name2", _capDetails[1].Id)
        };

        private static CapabilityDetails CreateCapability(string name, string desc, string reference, string version, string sourceUrl)
        {
            return new CapabilityDetails()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Desc = desc,
                Reference = reference,
                Version = version,
                SourceUrl = sourceUrl
            };
        }

        private static EpicDetails CreateEpic(string id, string name, Guid capId)
        {
            return new EpicDetails()
            {
                Id = id,
                Name = name,
                CapabilityId = capId,
                SourceUrl = "www.somelink.com",
                CompliancyLevelId = 1,
                Active = true
            };
        }

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
            _solutionEpicsRepository = testContext.SolutionEpicsRepository;
        }

        [Test]
        public async Task UpdateSolutionWithOneEpicAsync()
        {
            await InsertCapabilityAsync(_capDetails[0]).ConfigureAwait(false);
            await InsertSolutionCapabilityAsync(Solution1Id, _capDetails[0].Id).ConfigureAwait(false);
            await InsertEpicAsync(_epicDetails[0]).ConfigureAwait(false);

            var expectedEpic = new List<IClaimedEpicResult>
            {
                Mock.Of<IClaimedEpicResult>(e => e.EpicId == _epicDetails[0].Id && e.StatusName == StatusPassed)
            };

            await _solutionEpicsRepository
                .UpdateSolutionEpicAsync(Solution1Id,
                    Mock.Of<IUpdateClaimedRequest>(c => c.ClaimedEpics == expectedEpic), new CancellationToken()).ConfigureAwait(false);

            var solutionEpics = (await SolutionEpicEntity.FetchForSolutionAsync(Solution1Id).ConfigureAwait(false)).ToList();
            solutionEpics.Count().Should().Be(1);
            solutionEpics[0].Should().Be(_epicDetails[0].Id);
        }

        [Test]
        public async Task UpdateSolutionWithMultipleEpicsAsync()
        {
            await InsertCapabilityAsync(_capDetails[0]).ConfigureAwait(false);
            await InsertCapabilityAsync(_capDetails[1]).ConfigureAwait(false);
            await InsertSolutionCapabilityAsync(Solution1Id, _capDetails[0].Id).ConfigureAwait(false);
            await InsertSolutionCapabilityAsync(Solution1Id, _capDetails[1].Id).ConfigureAwait(false);

            await InsertEpicAsync(_epicDetails[0]).ConfigureAwait(false);
            await InsertEpicAsync(_epicDetails[1]).ConfigureAwait(false);

            var expectedEpic = new List<IClaimedEpicResult>
            {
                Mock.Of<IClaimedEpicResult>(e => e.EpicId == _epicDetails[0].Id && e.StatusName == StatusPassed),
                Mock.Of<IClaimedEpicResult>(e => e.EpicId == _epicDetails[1].Id && e.StatusName == StatusPassed)
            };

            await _solutionEpicsRepository
                .UpdateSolutionEpicAsync(Solution1Id,
                    Mock.Of<IUpdateClaimedRequest>(c => c.ClaimedEpics == expectedEpic), new CancellationToken()).ConfigureAwait(false);

            var solutionEpics = (await SolutionEpicEntity.FetchForSolutionAsync(Solution1Id).ConfigureAwait(false)).ToList();
            solutionEpics.Count().Should().Be(2);
            solutionEpics.Should().BeEquivalentTo(_epicDetails.Select(ed=>ed.Id),
                options => options.WithoutStrictOrdering());
            
        }

        [Test]
        public void ShouldThrowIfEpicRequestIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _solutionEpicsRepository.UpdateSolutionEpicAsync(Solution1Id, null, new CancellationToken()));
        }

        private async Task InsertCapabilityAsync(CapabilityDetails capability)
        {
            await CapabilityEntityBuilder.Create()
                .WithId(capability.Id)
                .WithName(capability.Name)
                .WithDescription(capability.Desc)
                .WithCapabilityRef(capability.Reference)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);
        }

        private async Task InsertSolutionCapabilityAsync(string solutionId, Guid capId)
        {
            await SolutionCapabilityEntityBuilder.Create()
                .WithCapabilityId(capId)
                .WithSolutionId(solutionId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);
        }

        private async Task InsertEpicAsync(EpicDetails epic)
        {
            await EpicEntityBuilder.Create()
                .WithId(epic.Id)
                .WithName(epic.Name)
                .WithCapabilityId(epic.CapabilityId)
                .WithSourceUrl(epic.SourceUrl)
                .WithCompliancyLevelId(epic.CompliancyLevelId)
                .WithActive(epic.Active)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);
        }
    }
}
