using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
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

        private List<(Guid id, string name, string desc, string reference)> capDetails;

        private ISolutionCapabilityRepository _solutionCapabilityRepository;

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
            capDetails = new List<(Guid id, string name, string desc, string reference)>
            {
                (Guid.NewGuid(), "Cap1", "Desc1", "Ref1"),
                (Guid.NewGuid(), "Cap2", "Desc2", "Ref2"),
                (Guid.NewGuid(), "Cap3", "Desc3", "Ref3")
            };
        }

        [Test]
        public async Task ShouldHaveOneCapabilityAsync()
        {
            await InsertCapabilityAsync(capDetails[0]).ConfigureAwait(false);

            await InsertSolutionCapabilityAsync(capDetails[0].id, Solution1Id).ConfigureAwait(false);

            var solutionCapabilityRequest =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None)
                    .ConfigureAwait(false);

            var solutionCapability = solutionCapabilityRequest.Should().ContainSingle().Subject;
            solutionCapability.CapabilityId.Should().Be(capDetails[0].id);
            solutionCapability.CapabilityName.Should().Be(capDetails[0].name);
            solutionCapability.CapabilityDescription.Should().Be(capDetails[0].desc);
        }

        [Test]
        public async Task ShouldHaveMultipleCapabilitiesAsync()
        {
            foreach (var capability in capDetails)
            {
                 await InsertCapabilityAsync(capability).ConfigureAwait(false);
                 await InsertSolutionCapabilityAsync(capability.id, Solution1Id).ConfigureAwait(false);
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

            await InsertCapabilityAsync(capDetails[0]).ConfigureAwait(false);
            await InsertCapabilityAsync(capDetails[1]).ConfigureAwait(false);

            await InsertSolutionCapabilityAsync(capDetails[0].id, Solution1Id).ConfigureAwait(false);
            await InsertSolutionCapabilityAsync(capDetails[1].id, Solution2Id).ConfigureAwait(false);


            var solutionCapabilityResponseSolution1 =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None)
                    .ConfigureAwait(false);

            var solutionCapabilityResponseSolution2 =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution2Id, CancellationToken.None)
                    .ConfigureAwait(false);


            var solutionCapability1 = solutionCapabilityResponseSolution1.Should().ContainSingle().Subject;
            solutionCapability1.CapabilityId.Should().Be(capDetails[0].id);
            solutionCapability1.CapabilityName.Should().Be(capDetails[0].name);
            solutionCapability1.CapabilityDescription.Should().Be(capDetails[0].desc);

            var solutionCapability2 = solutionCapabilityResponseSolution2.Should().ContainSingle().Subject;
            solutionCapability2.CapabilityId.Should().Be(capDetails[1].id);
            solutionCapability2.CapabilityName.Should().Be(capDetails[1].name);
            solutionCapability2.CapabilityDescription.Should().Be(capDetails[1].desc);
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
            await InsertCapabilityAsync(capDetails[0]).ConfigureAwait(false);
            await InsertSolutionCapabilityAsync(capDetails[0].id, Solution1Id).ConfigureAwait(false);

            await InsertCapabilityAsync(capDetails[1]).ConfigureAwait(false);

            IEnumerable<string> capabilityReferences = new List<string>(){capDetails[1].reference};

            await _solutionCapabilityRepository
                .UpdateCapabilitiesAsync(
                    Mock.Of<IUpdateCapabilityRequest>(c =>
                        c.SolutionId == Solution1Id && c.NewCapabilitiesReference == capabilityReferences),
                    new CancellationToken()).ConfigureAwait(false);

            var solutionCapabilities =
                (await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None)
                    .ConfigureAwait(false)).ToList();

            solutionCapabilities.Count().Should().Be(1);

            solutionCapabilities[0].CapabilityId.Should().Be(capDetails[1].id);
            solutionCapabilities[0].CapabilityName.Should().Be(capDetails[1].name);
            solutionCapabilities[0].CapabilityDescription.Should().Be(capDetails[1].desc);
        }

        [Test]
        public async Task UpdateSolutionWithMultipleCapabilitiesAsync()
        {
            await InsertCapabilityAsync(capDetails[0]).ConfigureAwait(false);
            await InsertSolutionCapabilityAsync(capDetails[0].id, Solution1Id).ConfigureAwait(false);

            await InsertCapabilityAsync(capDetails[1]).ConfigureAwait(false);
            await InsertCapabilityAsync(capDetails[2]).ConfigureAwait(false);

            IEnumerable<string> capabilityReferences = new List<string>() { capDetails[1].reference, capDetails[2].reference };

            await _solutionCapabilityRepository
                .UpdateCapabilitiesAsync(
                    Mock.Of<IUpdateCapabilityRequest>(c =>
                        c.SolutionId == Solution1Id && c.NewCapabilitiesReference == capabilityReferences),
                    new CancellationToken()).ConfigureAwait(false);

            var solutionCapabilities =
                (await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None)
                    .ConfigureAwait(false)).ToList();

            solutionCapabilities.Count().Should().Be(2);

            solutionCapabilities[0].CapabilityId.Should().Be((capDetails[1].id));
            solutionCapabilities[0].CapabilityName.Should().Be((capDetails[1].name));
            solutionCapabilities[0].CapabilityDescription.Should().Be((capDetails[1].desc));

            solutionCapabilities[1].CapabilityId.Should().Be((capDetails[2].id));
            solutionCapabilities[1].CapabilityName.Should().Be((capDetails[2].name));
            solutionCapabilities[1].CapabilityDescription.Should().Be((capDetails[2].desc));
        }

        [Test]
        public void ShouldThrowIfCapabilityRequestIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _solutionCapabilityRepository.UpdateCapabilitiesAsync(null, new CancellationToken()));
        }

        private async Task InsertCapabilityAsync((Guid id, string name, string desc, string reference) capability)
        {
            await CapabilityEntityBuilder.Create()
                .WithId(capability.id)
                .WithName(capability.name)
                .WithDescription(capability.desc)
                .WithCapabilityRef(capability.reference)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);
        }

        private async Task InsertSolutionCapabilityAsync(Guid capId, string solutionId)
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
