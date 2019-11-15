using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Persistence.Repositories;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    [TestFixture]
    public class SolutionCapabilityRepositoryTests
    {
        private Mock<IConfiguration> _configuration;

        private SolutionCapabilityRepository _solutionCapabilityRepository;

        private readonly Guid _org1Id = Guid.NewGuid();

        private const string Solution1Id = "Sln1";
        private const string Solution2Id = "Sln2";

        private const string SupplierId = "Sup 1";

        private readonly Guid _cap1Id = Guid.NewGuid();
        private readonly Guid _cap2Id = Guid.NewGuid();
        private readonly Guid _cap3Id = Guid.NewGuid();

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync();

            await OrganisationEntityBuilder.Create()
                .WithId(_org1Id)
                .Build()
                .InsertAsync();

            await SupplierEntityBuilder.Create()
                .WithOrganisation(_org1Id)
                .WithId(SupplierId)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(Solution1Id)
                .WithOrganisationId(_org1Id)
                .WithSupplierId(SupplierId)
                .Build()
                .InsertAsync();

            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(a => a["ConnectionStrings:BuyingCatalogue"])
                .Returns(ConnectionStrings.ServiceConnectionString());

            _solutionCapabilityRepository = new SolutionCapabilityRepository(new DbConnectionFactory(_configuration.Object));
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
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithCapabilityId(_cap1Id)
                .WithSolutionId(Solution1Id)
                .Build()
                .InsertAsync();

            var solutionCapabilityRequest =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None);

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
                .InsertAsync();

            await CapabilityEntityBuilder.Create()
                .WithId(_cap2Id)
                .WithName(cap2Name)
                .WithDescription(cap2Desc)
                .Build()
                .InsertAsync();

            await CapabilityEntityBuilder.Create()
                .WithId(_cap3Id)
                .WithName(cap3Name)
                .WithDescription(cap3Desc)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithCapabilityId(_cap1Id)
                .WithSolutionId(Solution1Id)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithCapabilityId(_cap2Id)
                .WithSolutionId(Solution1Id)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithCapabilityId(_cap3Id)
                .WithSolutionId(Solution1Id)
                .Build()
                .InsertAsync();

            var solutionCapabilityResponse =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None);

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
                .WithOrganisationId(_org1Id)
                .WithSupplierId(SupplierId)
                .Build()
                .InsertAsync();

            await CapabilityEntityBuilder.Create()
                .WithId(_cap1Id)
                .WithName(cap1Name)
                .WithDescription(cap1Desc)
                .Build()
                .InsertAsync();

            await CapabilityEntityBuilder.Create()
                .WithId(_cap2Id)
                .WithName(cap2Name)
                .WithDescription(cap2Desc)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithCapabilityId(_cap1Id)
                .WithSolutionId(Solution1Id)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithCapabilityId(_cap2Id)
                .WithSolutionId(Solution2Id)
                .Build()
                .InsertAsync();

            var solutionCapabilityResponseSolution1 =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None);

            var solutionCapabilityResponseSolution2 =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution2Id, CancellationToken.None);


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
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution1Id, CancellationToken.None);

            var solutionCapability = solutionCapabilityResponse.Should().BeEmpty();

        }

        [Test]
        public async Task NoSolutions()
        {
            var solutionCapabilityResponse =
                await _solutionCapabilityRepository.ListSolutionCapabilities(Solution2Id, CancellationToken.None);

            var solutionCapability = solutionCapabilityResponse.Should().BeEmpty();
        }
    }
}
