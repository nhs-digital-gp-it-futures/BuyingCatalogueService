using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Persistence.Repositories;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    [TestFixture]
    public class SolutionRepositoryTests
    {
        private Mock<IConfiguration> _configuration;

        private SolutionRepository _solutionRepository;

        private readonly Guid _cap1Id = Guid.NewGuid();
        private readonly Guid _cap2Id = Guid.NewGuid();

        private readonly Guid _org1Id = Guid.NewGuid();
        private readonly string _orgName = "Org1";

        private readonly string _supplierId = "Sup 1";

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync();

            await OrganisationEntityBuilder.Create()
                .WithName(_orgName)
                .WithId(_org1Id)
                .Build()
                .InsertAsync();

            await SupplierEntityBuilder.Create()
                .WithId(_supplierId)
                .WithOrganisation(_org1Id)
                .Build()
                .InsertAsync();

            await CapabilityEntityBuilder.Create().WithName("Cap1").WithId(_cap1Id).WithDescription("Cap1Desc").Build().InsertAsync();
            await CapabilityEntityBuilder.Create().WithName("Cap2").WithId(_cap2Id).WithDescription("Cap2Desc").Build().InsertAsync();

            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(a => a["ConnectionStrings:BuyingCatalogue"]).Returns(ConnectionStrings.ServiceConnectionString());

            _solutionRepository = new SolutionRepository(new DbConnectionFactory(_configuration.Object));
        }

        [Test]
        public async Task ShouldListSingleSolution()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            var solutions = await _solutionRepository.ListAsync(false, new CancellationToken());
            solutions.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldListSingleSolutionWithSingleCapability()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithSummary("Sln1Summary")
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithCapabilityId(_cap1Id)
                .Build()
                .InsertAsync();

            var solutions = await _solutionRepository.ListAsync(false, new CancellationToken());

            var solution = solutions.Should().ContainSingle().Subject;
            solution.SolutionId.Should().Be("Sln1");
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.OrganisationId.Should().Be(_org1Id);
            solution.OrganisationName.Should().Be(_orgName);
            solution.CapabilityId.Should().Be(_cap1Id);
            solution.CapabilityName.Should().Be("Cap1");
            solution.CapabilityDescription.Should().Be("Cap1Desc");
            solution.IsFoundation.Should().BeFalse();
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldListSingleSolutionAsFoundation(bool isFoundation)
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithSummary("Sln1Summary")
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithCapabilityId(_cap1Id)
                .Build()
                .InsertAsync();

            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithFoundation(isFoundation)
                .Build().InsertAsync();

            var solutions = await _solutionRepository.ListAsync(false, new CancellationToken());

            var solution = solutions.Should().ContainSingle().Subject;
            solution.SolutionId.Should().Be("Sln1");
            solution.IsFoundation.Should().Be(isFoundation);
        }

        [Test]
        public async Task ShouldListSingleSolutionWithMultipleCapability()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithSummary("Sln1Summary")
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithCapabilityId(_cap1Id)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithCapabilityId(_cap2Id)
                .Build()
                .InsertAsync();

            var solutions = (await _solutionRepository.ListAsync(false, new CancellationToken())).ToList();
            solutions.Should().HaveCount(2);

            var solution = solutions.Should().ContainSingle(s => s.CapabilityId == _cap1Id).Subject;
            solution.SolutionId.Should().Be("Sln1");
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.OrganisationId.Should().Be(_org1Id);
            solution.OrganisationName.Should().Be(_orgName);
            solution.CapabilityId.Should().Be(_cap1Id);
            solution.CapabilityName.Should().Be("Cap1");
            solution.CapabilityDescription.Should().Be("Cap1Desc");

            solution = solutions.Should().ContainSingle(s => s.CapabilityId == _cap2Id).Subject;
            solution.SolutionId.Should().Be("Sln1");
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.OrganisationId.Should().Be(_org1Id);
            solution.OrganisationName.Should().Be(_orgName);
            solution.CapabilityId.Should().Be(_cap2Id);
            solution.CapabilityName.Should().Be("Cap2");
            solution.CapabilityDescription.Should().Be("Cap2Desc");
        }

        [Test]
        public async Task ShouldListMultipleSolutionsWithCapabilities()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithName("Solution2")
                .WithId("Sln2")
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithSummary("Sln1Summary")
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId("Sln2")
                .WithSummary("Sln2Summary")
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithCapabilityId(_cap1Id)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithCapabilityId(_cap2Id)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId("Sln2")
                .WithCapabilityId(_cap2Id)
                .Build()
                .InsertAsync();

            var solutions = (await _solutionRepository.ListAsync(false, new CancellationToken())).ToList();
            solutions.Should().HaveCount(3);

            var solution = solutions.Should().ContainSingle(s => s.SolutionId == "Sln1" && s.CapabilityId == _cap1Id).Subject;
            solution.SolutionId.Should().Be("Sln1");
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.OrganisationId.Should().Be(_org1Id);
            solution.OrganisationName.Should().Be(_orgName);
            solution.CapabilityId.Should().Be(_cap1Id);
            solution.CapabilityName.Should().Be("Cap1");
            solution.CapabilityDescription.Should().Be("Cap1Desc");

            solution = solutions.Should().ContainSingle(s => s.SolutionId == "Sln1" && s.CapabilityId == _cap2Id).Subject;
            solution.SolutionId.Should().Be("Sln1");
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.OrganisationId.Should().Be(_org1Id);
            solution.OrganisationName.Should().Be(_orgName);
            solution.CapabilityId.Should().Be(_cap2Id);
            solution.CapabilityName.Should().Be("Cap2");
            solution.CapabilityDescription.Should().Be("Cap2Desc");

            solution = solutions.Should().ContainSingle(s => s.SolutionId == "Sln2" && s.CapabilityId == _cap2Id).Subject;
            solution.SolutionId.Should().Be("Sln2");
            solution.SolutionName.Should().Be("Solution2");
            solution.SolutionSummary.Should().Be("Sln2Summary");
            solution.OrganisationId.Should().Be(_org1Id);
            solution.OrganisationName.Should().Be(_orgName);
            solution.CapabilityId.Should().Be(_cap2Id);
            solution.CapabilityName.Should().Be("Cap2");
            solution.CapabilityDescription.Should().Be("Cap2Desc");
        }

        [Test]
        public async Task ShouldFilterByFoundation()
        {
            await CreateSimpleSolutionWithOneCap("Sln1");
            await CreateSimpleSolutionWithOneCap("Sln2");
            await CreateSimpleSolutionWithOneCap("Sln3");
            await CreateSimpleSolutionWithOneCap("Sln4");

            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithFoundation(true)
                .Build()
                .InsertAsync();

            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId("Sln2")
                .WithFoundation(false)
                .Build()
                .InsertAsync();

            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId("Sln4")
                .WithFoundation(true)
                .Build()
                .InsertAsync();

            var solutions = await _solutionRepository.ListAsync(true, new CancellationToken());
            solutions.Should().HaveCount(2);
            var solution = solutions.Should().ContainSingle(s => s.SolutionId == "Sln1").Subject;
            solution.IsFoundation.Should().Be(true);
            solution = solutions.Should().ContainSingle(s => s.SolutionId == "Sln4").Subject;
            solution.IsFoundation.Should().Be(true);
        }

        private async Task CreateSimpleSolutionWithOneCap(string solutionId)
        {
            await SolutionEntityBuilder.Create()
                .WithName(solutionId)
                .WithId(solutionId)
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(solutionId)
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId(solutionId)
                .WithCapabilityId(_cap1Id)
                .Build()
                .InsertAsync();
        }

        [Test]
        public async Task ShouldGetById()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithSummary("Sln1Summary")
                .WithFullDescription("Sln1Description")
                .WithAboutUrl("AboutUrl")
                .WithFeatures("Features")
                .WithClientApplication("Browser-based")
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            var solution = await _solutionRepository.ByIdAsync("Sln1", new CancellationToken());
            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Solution1");
            solution.Summary.Should().Be("Sln1Summary");
            solution.Description.Should().Be("Sln1Description");
            solution.AboutUrl.Should().Be("AboutUrl");
            solution.Features.Should().Be("Features");
            solution.ClientApplication.Should().Be("Browser-based");
            solution.OrganisationName.Should().Be(_orgName);
            solution.IsFoundation.Should().BeFalse();
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldGetByIdFoundation(bool isFoundation)
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithFoundation(isFoundation)
                .Build()
                .InsertAsync();

            var solution = await _solutionRepository.ByIdAsync("Sln1", new CancellationToken());
            solution.Id.Should().Be("Sln1");
            solution.IsFoundation.Should().Be(isFoundation);
        }

        [Test]
        public async Task ShouldGetByIdNotPresent()
        {
            var solution = await _solutionRepository.ByIdAsync("Sln1", new CancellationToken());
            solution.Should().BeNull();
        }

        [Test]
        public async Task ShouldGetByIdSolutionDetailNotPresent()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            var solution = await _solutionRepository.ByIdAsync("Sln1", new CancellationToken());
            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Solution1");
            solution.Summary.Should().BeNull();
            solution.Description.Should().BeNull();
            solution.OrganisationName.Should().Be(_orgName);
            solution.AboutUrl.Should().BeNull();
            solution.Features.Should().BeNull();
            solution.ClientApplication.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateSupplierStatus()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .WithSupplierStatusId(1)
                .Build()
                .InsertAsync();

            var mockUpdateSolutionSupplierStatusRequest = new Mock<IUpdateSolutionSupplierStatusRequest>();
            mockUpdateSolutionSupplierStatusRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionSupplierStatusRequest.Setup(m => m.SupplierStatusId).Returns(2);

            await _solutionRepository.UpdateSupplierStatusAsync(mockUpdateSolutionSupplierStatusRequest.Object, new CancellationToken());

            var solution = await SolutionEntity.GetByIdAsync("Sln1");
            solution.Id.Should().Be("Sln1");
            solution.SupplierStatusId.Should().Be(2);
        }

        [Test]
        public void ShouldThrowOnUpdateSupplierStatusSolutionNotPresent()
        {
            var mockUpdateSolutionSupplierStatusRequest = new Mock<IUpdateSolutionSupplierStatusRequest>();
            mockUpdateSolutionSupplierStatusRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionSupplierStatusRequest.Setup(m => m.SupplierStatusId).Returns(2);

            Assert.ThrowsAsync<SqlException>(() =>  _solutionRepository.UpdateSupplierStatusAsync(mockUpdateSolutionSupplierStatusRequest.Object, new CancellationToken()));
        }
    }
}
