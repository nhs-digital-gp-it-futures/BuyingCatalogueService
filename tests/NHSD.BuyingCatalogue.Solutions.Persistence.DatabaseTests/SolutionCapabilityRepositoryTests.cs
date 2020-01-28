using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
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

        private readonly Guid _cap1Id = Guid.NewGuid();
        private readonly Guid _cap2Id = Guid.NewGuid();
        private readonly Guid _cap3Id = Guid.NewGuid();

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
        }

        [Test]
        public async Task ShouldHaveOneCapability()
        {
            const string cap1Name = "Cap1";
            const string cap1Desc = "Desc1";

            await CapabilityEntityBuilder.Create()
                .WithId(_cap1Id)
                .WithName(cap1Name)
                .WithDescription(cap1Desc)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SolutionCapabilityEntityBuilder.Create()
                .WithCapabilityId(_cap1Id)
                .WithSolutionId(Solution1Id)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            var solutionCapabilityRequest =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None)
                    .ConfigureAwait(false);

            var solutionCapability = solutionCapabilityRequest.Should().ContainSingle().Subject;
            solutionCapability.CapabilityId.Should().Be(_cap1Id);
            solutionCapability.CapabilityName.Should().Be(cap1Name);
            solutionCapability.CapabilityDescription.Should().Be(cap1Desc);
        }

        [Test]
        public async Task ShouldHaveMultipleCapabilities()
        {
            const string cap1Name = "Cap1";
            const string cap1Desc = "Desc1";

            const string cap2Name = "Cap2";
            const string cap2Desc = "Desc2";

            const string cap3Name = "Cap3";
            const string cap3Desc = "Desc3";

            await CapabilityEntityBuilder.Create()
                .WithId(_cap1Id)
                .WithName(cap1Name)
                .WithDescription(cap1Desc)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await CapabilityEntityBuilder.Create()
                .WithId(_cap2Id)
                .WithName(cap2Name)
                .WithDescription(cap2Desc)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await CapabilityEntityBuilder.Create()
                .WithId(_cap3Id)
                .WithName(cap3Name)
                .WithDescription(cap3Desc)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SolutionCapabilityEntityBuilder.Create()
                .WithCapabilityId(_cap1Id)
                .WithSolutionId(Solution1Id)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SolutionCapabilityEntityBuilder.Create()
                .WithCapabilityId(_cap2Id)
                .WithSolutionId(Solution1Id)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SolutionCapabilityEntityBuilder.Create()
                .WithCapabilityId(_cap3Id)
                .WithSolutionId(Solution1Id)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            var solutionCapabilityResponse =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None)
                    .ConfigureAwait(false);

            solutionCapabilityResponse.Count().Should().Be(3);
        }

        [Test]
        public async Task HasMultipleSolutions()
        {
            const string cap1Name = "Cap1";
            const string cap1Desc = "Desc1";

            const string cap2Name = "Cap2";
            const string cap2Desc = "Desc2";

            await SolutionEntityBuilder.Create()
                .WithId(Solution2Id)
                .WithSupplierId(SupplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await CapabilityEntityBuilder.Create()
                .WithId(_cap1Id)
                .WithName(cap1Name)
                .WithDescription(cap1Desc)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await CapabilityEntityBuilder.Create()
                .WithId(_cap2Id)
                .WithName(cap2Name)
                .WithDescription(cap2Desc)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SolutionCapabilityEntityBuilder.Create()
                .WithCapabilityId(_cap1Id)
                .WithSolutionId(Solution1Id)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SolutionCapabilityEntityBuilder.Create()
                .WithCapabilityId(_cap2Id)
                .WithSolutionId(Solution2Id)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            var solutionCapabilityResponseSolution1 =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None)
                    .ConfigureAwait(false);

            var solutionCapabilityResponseSolution2 =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution2Id, CancellationToken.None)
                    .ConfigureAwait(false);


            var solutionCapability1 = solutionCapabilityResponseSolution1.Should().ContainSingle().Subject;
            solutionCapability1.CapabilityId.Should().Be(_cap1Id);
            solutionCapability1.CapabilityName.Should().Be(cap1Name);
            solutionCapability1.CapabilityDescription.Should().Be(cap1Desc);

            var solutionCapability2 = solutionCapabilityResponseSolution2.Should().ContainSingle().Subject;
            solutionCapability2.CapabilityId.Should().Be(_cap2Id);
            solutionCapability2.CapabilityName.Should().Be(cap2Name);
            solutionCapability2.CapabilityDescription.Should().Be(cap2Desc);
        }

        [Test]
        public async Task NoCapabilities()
        {
            var solutionCapabilityResponse =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None)
                    .ConfigureAwait(false);

            solutionCapabilityResponse.Should().BeEmpty();
        }

        [Test]
        public async Task NoSolutions()
        {
            var solutionCapabilityResponse =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution2Id, CancellationToken.None)
                    .ConfigureAwait(false);

            solutionCapabilityResponse.Should().BeEmpty();
        }
    }
}
