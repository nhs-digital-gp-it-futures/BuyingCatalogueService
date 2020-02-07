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
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests
{
    [TestFixture]
    public class SolutionCapabilityRepositoryTests
    {
        private const string Solution1Id = "Sln1";
        private const string Solution2Id = "Sln2";

        private const string SupplierId = "Sup 1";

        private readonly List<CapabilityDetails> _capDetails = new List<CapabilityDetails>()
        {
            CreateCapability("Cap1", "Desc1", "Ref1"),
            CreateCapability("Cap2", "Desc2", "Ref2"),
            CreateCapability("Cap3", "Desc3", "Ref3")
        };

        private ISolutionCapabilityRepository _solutionCapabilityRepository;

        private static CapabilityDetails CreateCapability(string name, string desc, string reference)
        {
            return new CapabilityDetails()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Desc = desc,
                Reference = reference
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
            _solutionCapabilityRepository = testContext.SolutionCapabilityRepository;
        }

        [Test]
        public async Task ShouldHaveOneCapabilityAsync()
        {
            await InsertCapabilityAsync(_capDetails[0]).ConfigureAwait(false);

            await InsertSolutionCapabilityAsync(Solution1Id, _capDetails[0].Id).ConfigureAwait(false);

            var solutionCapabilityRequest =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None)
                    .ConfigureAwait(false);

            var solutionCapability = solutionCapabilityRequest.Should().ContainSingle().Subject;
            solutionCapability.CapabilityId.Should().Be(_capDetails[0].Id);
            solutionCapability.CapabilityName.Should().Be(_capDetails[0].Name);
            solutionCapability.CapabilityDescription.Should().Be(_capDetails[0].Desc);
        }

        [Test]
        public async Task ShouldHaveMultipleCapabilitiesAsync()
        {
            foreach (var capability in _capDetails)
            {
                 await InsertCapabilityAsync(capability).ConfigureAwait(false);
                 await InsertSolutionCapabilityAsync(Solution1Id, capability.Id).ConfigureAwait(false);
            }

            var solutionCapabilityResponse =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None)
                    .ConfigureAwait(false);

            solutionCapabilityResponse.Count().Should().Be(3);
        }

        [Test]
        public async Task HasMultipleSolutionsAsync()
        {
            await SolutionEntityBuilder.Create()
                .WithId(Solution2Id)
                .WithSupplierId(SupplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await InsertCapabilityAsync(_capDetails[0]).ConfigureAwait(false);
            await InsertCapabilityAsync(_capDetails[1]).ConfigureAwait(false);

            await InsertSolutionCapabilityAsync(Solution1Id, _capDetails[0].Id).ConfigureAwait(false);
            await InsertSolutionCapabilityAsync(Solution2Id, _capDetails[1].Id).ConfigureAwait(false);


            var solutionCapabilityResponseSolution1 =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None)
                    .ConfigureAwait(false);

            var solutionCapabilityResponseSolution2 =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution2Id, CancellationToken.None)
                    .ConfigureAwait(false);


            var solutionCapability1 = solutionCapabilityResponseSolution1.Should().ContainSingle().Subject;
            solutionCapability1.CapabilityId.Should().Be(_capDetails[0].Id);
            solutionCapability1.CapabilityName.Should().Be(_capDetails[0].Name);
            solutionCapability1.CapabilityDescription.Should().Be(_capDetails[0].Desc);

            var solutionCapability2 = solutionCapabilityResponseSolution2.Should().ContainSingle().Subject;
            solutionCapability2.CapabilityId.Should().Be(_capDetails[1].Id);
            solutionCapability2.CapabilityName.Should().Be(_capDetails[1].Name);
            solutionCapability2.CapabilityDescription.Should().Be(_capDetails[1].Desc);
        }

        [Test]
        public async Task NoCapabilitiesAsync()
        {
            var solutionCapabilityResponse =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None)
                    .ConfigureAwait(false);

            solutionCapabilityResponse.Should().BeEmpty();
        }

        [Test]
        public async Task NoSolutionsAsync()
        {
            var solutionCapabilityResponse =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution2Id, CancellationToken.None)
                    .ConfigureAwait(false);

            solutionCapabilityResponse.Should().BeEmpty();
        }

        [Test]
        public async Task UpdateSolutionWithOneCapabilityAsync()
        {
            await InsertCapabilityAsync(_capDetails[0]).ConfigureAwait(false);
            await InsertSolutionCapabilityAsync(Solution1Id, _capDetails[0].Id).ConfigureAwait(false);

            await InsertCapabilityAsync(_capDetails[1]).ConfigureAwait(false);

            IEnumerable<string> capabilityReferences = new List<string>(){_capDetails[1].Reference };

            await _solutionCapabilityRepository
                .UpdateCapabilitiesAsync(
                    Mock.Of<IUpdateCapabilityRequest>(c =>
                        c.SolutionId == Solution1Id && c.NewCapabilitiesReference == capabilityReferences),
                    new CancellationToken()).ConfigureAwait(false);

            var solutionCapabilities =
                (await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None)
                    .ConfigureAwait(false)).ToList();

            solutionCapabilities.Count().Should().Be(1);

            solutionCapabilities[0].CapabilityId.Should().Be(_capDetails[1].Id);
            solutionCapabilities[0].CapabilityName.Should().Be(_capDetails[1].Name);
            solutionCapabilities[0].CapabilityDescription.Should().Be(_capDetails[1].Desc);
        }

        [Test]
        public async Task UpdateSolutionWithMultipleCapabilitiesAsync()
        {
            await InsertCapabilityAsync(_capDetails[0]).ConfigureAwait(false);
            await InsertSolutionCapabilityAsync(Solution1Id, _capDetails[0].Id).ConfigureAwait(false);

            await InsertCapabilityAsync(_capDetails[1]).ConfigureAwait(false);
            await InsertCapabilityAsync(_capDetails[2]).ConfigureAwait(false);

            IEnumerable<string> capabilityReferences = new List<string>() { _capDetails[1].Reference, _capDetails[2].Reference };

            await _solutionCapabilityRepository
                .UpdateCapabilitiesAsync(
                    Mock.Of<IUpdateCapabilityRequest>(c =>
                        c.SolutionId == Solution1Id && c.NewCapabilitiesReference == capabilityReferences),
                    new CancellationToken()).ConfigureAwait(false);

            var solutionCapabilities =
                (await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None)
                    .ConfigureAwait(false)).ToList();

            solutionCapabilities.Count().Should().Be(2);

            solutionCapabilities[0].CapabilityId.Should().Be((_capDetails[1].Id));
            solutionCapabilities[0].CapabilityName.Should().Be((_capDetails[1].Name));
            solutionCapabilities[0].CapabilityDescription.Should().Be((_capDetails[1].Desc));

            solutionCapabilities[1].CapabilityId.Should().Be((_capDetails[2].Id));
            solutionCapabilities[1].CapabilityName.Should().Be((_capDetails[2].Name));
            solutionCapabilities[1].CapabilityDescription.Should().Be((_capDetails[2].Desc));
        }

        [Test]
        public void ShouldThrowIfCapabilityRequestIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _solutionCapabilityRepository.UpdateCapabilitiesAsync(null, new CancellationToken()));
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
    }
}
