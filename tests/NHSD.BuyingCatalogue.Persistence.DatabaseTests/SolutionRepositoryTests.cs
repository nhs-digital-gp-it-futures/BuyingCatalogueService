using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Moq;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Persistence.Repositories;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using System.Threading;
using FluentAssertions;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    [TestFixture]
    public class SolutionRepositoryTests
    {
        private Mock<IConfiguration> _configuration;

        private SolutionRepository _solutionRepository;

        private readonly Guid Cap1Id = Guid.NewGuid();
        private readonly Guid Cap2Id = Guid.NewGuid();
        private readonly Guid Cap3Id = Guid.NewGuid();

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync();

            await OrganisationEntityBuilder.Create()
                .WithName("OrgName1")
                .WithId("Org1")
                .Build()
                .InsertAsync();

            await OrganisationEntityBuilder.Create()
                .WithName("OrgName2")
                .WithId("Org2")
                .Build()
                .InsertAsync();

            await CapabilityEntityBuilder.Create().WithName("Cap1").WithId(Cap1Id).WithDescription("Cap1Desc").Build().InsertAsync();
            await CapabilityEntityBuilder.Create().WithName("Cap2").WithId(Cap2Id).WithDescription("Cap2Desc").Build().InsertAsync();
            await CapabilityEntityBuilder.Create().WithName("Cap3").WithId(Cap3Id).WithDescription("Cap3Desc").Build().InsertAsync();

            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(a => a["ConnectionStrings:BuyingCatalogue"]).Returns(ConnectionStrings.ServiceConnectionString());

            _solutionRepository = new SolutionRepository(new DbConnectionFactory(_configuration.Object));
        }

        [Test]
        public async Task ShouldListSingleSolution()
        {
            var organisations = await OrganisationEntity.FetchAllAsync();

            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithSummary("Sln1Summary")
                .WithOrganisationId(organisations.First(o => o.Name == "OrgName1").Id)
                .Build()
                .InsertAsync();

            var solutions = await _solutionRepository.ListAsync(new CancellationToken());
            solutions.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldListSingleSolutionWithSingleCapability()
        {
            var organisations = await OrganisationEntity.FetchAllAsync();

            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithSummary("Sln1Summary")
                .WithOrganisationId(organisations.First(o => o.Name == "OrgName1").Id)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithCapabilityId(Cap1Id)
                .Build()
                .InsertAsync();

            var solutions = await _solutionRepository.ListAsync(new CancellationToken());

            var solution = solutions.Should().ContainSingle().Subject;
            solution.SolutionId.Should().Be("Sln1");
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.OrganisationId.Should().Be("Org1");
            solution.OrganisationName.Should().Be("OrgName1");
            solution.CapabilityId.Should().Be(Cap1Id);
            solution.CapabilityName.Should().Be("Cap1");
            solution.CapabilityDescription.Should().Be("Cap1Desc");
        }


        [Test]
        public async Task ShouldListSingleSolutionWithMultipleCapability()
        {
            var organisations = await OrganisationEntity.FetchAllAsync();

            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithSummary("Sln1Summary")
                .WithOrganisationId(organisations.First(o => o.Name == "OrgName1").Id)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithCapabilityId(Cap1Id)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithCapabilityId(Cap2Id)
                .Build()
                .InsertAsync();

            var solutions = await _solutionRepository.ListAsync(new CancellationToken());
            solutions.Should().HaveCount(2);

            var solution = solutions.Should().ContainSingle(s => s.CapabilityId == Cap1Id).Subject;
            solution.SolutionId.Should().Be("Sln1");
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.OrganisationId.Should().Be("Org1");
            solution.OrganisationName.Should().Be("OrgName1");
            solution.CapabilityId.Should().Be(Cap1Id);
            solution.CapabilityName.Should().Be("Cap1");
            solution.CapabilityDescription.Should().Be("Cap1Desc");

            solution = solutions.Should().ContainSingle(s => s.CapabilityId == Cap2Id).Subject;
            solution.SolutionId.Should().Be("Sln1");
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.OrganisationId.Should().Be("Org1");
            solution.OrganisationName.Should().Be("OrgName1");
            solution.CapabilityId.Should().Be(Cap2Id);
            solution.CapabilityName.Should().Be("Cap2");
            solution.CapabilityDescription.Should().Be("Cap2Desc");
        }

        [Test]
        public async Task ShouldListMultipleSolutionsWithCapabilities()
        {
            var organisations = await OrganisationEntity.FetchAllAsync();

            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithSummary("Sln1Summary")
                .WithOrganisationId(organisations.First(o => o.Name == "OrgName1").Id)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithName("Solution2")
                .WithId("Sln2")
                .WithSummary("Sln2Summary")
                .WithOrganisationId(organisations.First(o => o.Name == "OrgName1").Id)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithCapabilityId(Cap1Id)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithCapabilityId(Cap2Id)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId("Sln2")
                .WithCapabilityId(Cap2Id)
                .Build()
                .InsertAsync();

            var solutions = await _solutionRepository.ListAsync(new CancellationToken());
            solutions.Should().HaveCount(3);

            var solution = solutions.Should().ContainSingle(s => s.SolutionId == "Sln1" && s.CapabilityId == Cap1Id).Subject;
            solution.SolutionId.Should().Be("Sln1");
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.OrganisationId.Should().Be("Org1");
            solution.OrganisationName.Should().Be("OrgName1");
            solution.CapabilityId.Should().Be(Cap1Id);
            solution.CapabilityName.Should().Be("Cap1");
            solution.CapabilityDescription.Should().Be("Cap1Desc");

            solution = solutions.Should().ContainSingle(s => s.SolutionId == "Sln1" && s.CapabilityId == Cap2Id).Subject;
            solution.SolutionId.Should().Be("Sln1");
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.OrganisationId.Should().Be("Org1");
            solution.OrganisationName.Should().Be("OrgName1");
            solution.CapabilityId.Should().Be(Cap2Id);
            solution.CapabilityName.Should().Be("Cap2");
            solution.CapabilityDescription.Should().Be("Cap2Desc");

            solution = solutions.Should().ContainSingle(s => s.SolutionId == "Sln2" && s.CapabilityId == Cap2Id).Subject;
            solution.SolutionId.Should().Be("Sln2");
            solution.SolutionName.Should().Be("Solution2");
            solution.SolutionSummary.Should().Be("Sln2Summary");
            solution.OrganisationId.Should().Be("Org1");
            solution.OrganisationName.Should().Be("OrgName1");
            solution.CapabilityId.Should().Be(Cap2Id);
            solution.CapabilityName.Should().Be("Cap2");
            solution.CapabilityDescription.Should().Be("Cap2Desc");
        }

        [Test]
        public async Task ShouldGetById()
        {
            var organisations = await OrganisationEntity.FetchAllAsync();

            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithSummary("Sln1Summary")
                .WithFullDescription("Sln1Description")
                .WithOrganisationId(organisations.First(o => o.Name == "OrgName1").Id)
                .Build()
                .InsertAsync();

            await MarketingDetailEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithAboutUrl("AboutUrl")
                .WithFeatures("Features")
                .Build()
                .InsertAsync();

            var solution = await _solutionRepository.ByIdAsync("Sln1", new CancellationToken());
            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Solution1");
            solution.Summary.Should().Be("Sln1Summary");
            solution.Description.Should().Be("Sln1Description");
            solution.AboutUrl.Should().Be("AboutUrl");
            solution.Features.Should().Be("Features");
        }

        [Test]
        public async Task ShouldGetByIdNotPresent()
        {
            var solution = await _solutionRepository.ByIdAsync("Sln1", new CancellationToken());
            solution.Should().BeNull();
        }

        [Test]
        public async Task ShouldGetByIdMarketingDataNotPresent()
        {
            var organisations = await OrganisationEntity.FetchAllAsync();

            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithSummary("Sln1Summary")
                .WithFullDescription("Sln1Description")
                .WithOrganisationId(organisations.First(o => o.Name == "OrgName1").Id)
                .Build()
                .InsertAsync();

            var solution = await _solutionRepository.ByIdAsync("Sln1", new CancellationToken());
            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Solution1");
            solution.Summary.Should().Be("Sln1Summary");
            solution.Description.Should().Be("Sln1Description");
            solution.AboutUrl.Should().BeNull();
            solution.Features.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdate()
        {
            var organisations = await OrganisationEntity.FetchAllAsync();

            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithSummary("Sln1Summary")
                .WithFullDescription("Sln1Description")
                .WithOrganisationId(organisations.First(o => o.Name == "OrgName1").Id)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithName("Solution2")
                .WithId("Sln2")
                .WithSummary("Sln2Summary")
                .WithFullDescription("Sln2Description")
                .WithOrganisationId(organisations.First(o => o.Name == "OrgName1").Id)
                .Build()
                .InsertAsync();

            await MarketingDetailEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithAboutUrl("AboutUrl")
                .WithFeatures("Features")
                .Build()
                .InsertAsync();

            var mockUpdateSolutionRequest = new Mock<IUpdateSolutionRequest>();
            mockUpdateSolutionRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionRequest.Setup(m => m.Summary).Returns("Sln4Summary");
            mockUpdateSolutionRequest.Setup(m => m.Description).Returns("Sln4Description");
            mockUpdateSolutionRequest.Setup(m => m.AboutUrl).Returns("AboutUrl4");
            mockUpdateSolutionRequest.Setup(m => m.Features).Returns("Features4");

            await _solutionRepository.UpdateAsync(mockUpdateSolutionRequest.Object, new CancellationToken());

            var solution = await SolutionEntity.GetByIdAsync("Sln1");
            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Solution1");
            solution.Summary.Should().Be("Sln4Summary");
            solution.FullDescription.Should().Be("Sln4Description");

            solution = await SolutionEntity.GetByIdAsync("Sln2");
            solution.Id.Should().Be("Sln2");
            solution.Name.Should().Be("Solution2");
            solution.Summary.Should().Be("Sln2Summary");
            solution.FullDescription.Should().Be("Sln2Description");

            var marketingData = await MarketingDetailEntity.GetBySolutionIdAsync("Sln1");
            marketingData.AboutUrl.Should().Be("AboutUrl4");
            marketingData.Features.Should().Be("Features4");
        }

        [Test]
        public void ShouldUpdateNotPresent()
        {
            var mockUpdateSolutionRequest = new Mock<IUpdateSolutionRequest>();
            mockUpdateSolutionRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionRequest.Setup(m => m.Summary).Returns("Sln4Summary");
            mockUpdateSolutionRequest.Setup(m => m.Description).Returns("Sln4Description");
            mockUpdateSolutionRequest.Setup(m => m.AboutUrl).Returns("AboutUrl4");
            mockUpdateSolutionRequest.Setup(m => m.Features).Returns("Features4");

            Assert.DoesNotThrowAsync(() => _solutionRepository.UpdateAsync(mockUpdateSolutionRequest.Object, new CancellationToken()));
        }

        [Test]
        public async Task ShouldUpdateMarketingDataNotPresent()
        {
            var organisations = await OrganisationEntity.FetchAllAsync();

            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithSummary("Sln1Summary")
                .WithFullDescription("Sln1Description")
                .WithOrganisationId(organisations.First(o => o.Name == "OrgName1").Id)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithName("Solution2")
                .WithId("Sln2")
                .WithSummary("Sln2Summary")
                .WithFullDescription("Sln2Description")
                .WithOrganisationId(organisations.First(o => o.Name == "OrgName1").Id)
                .Build()
                .InsertAsync();

            var mockUpdateSolutionRequest = new Mock<IUpdateSolutionRequest>();
            mockUpdateSolutionRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionRequest.Setup(m => m.Summary).Returns("Sln4Summary");
            mockUpdateSolutionRequest.Setup(m => m.Description).Returns("Sln4Description");
            mockUpdateSolutionRequest.Setup(m => m.AboutUrl).Returns("AboutUrl4");
            mockUpdateSolutionRequest.Setup(m => m.Features).Returns("Features4");

            await _solutionRepository.UpdateAsync(mockUpdateSolutionRequest.Object, new CancellationToken());

            var solution = await SolutionEntity.GetByIdAsync("Sln1");
            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Solution1");
            solution.Summary.Should().Be("Sln4Summary");
            solution.FullDescription.Should().Be("Sln4Description");

            var marketingData = await MarketingDetailEntity.GetBySolutionIdAsync("Sln1");
            marketingData.AboutUrl.Should().Be("AboutUrl4");
            marketingData.Features.Should().Be("Features4");
        }

        [Test]
        public async Task ShouldUpdateSupplierStatus()
        {
            var organisations = await OrganisationEntity.FetchAllAsync();

            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithSummary("Sln1Summary")
                .WithFullDescription("Sln1Description")
                .WithOrganisationId(organisations.First(o => o.Name == "OrgName1").Id)
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
        public void ShouldUpdateSupplierStatusNotPresent()
        {
            var mockUpdateSolutionSupplierStatusRequest = new Mock<IUpdateSolutionSupplierStatusRequest>();
            mockUpdateSolutionSupplierStatusRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionSupplierStatusRequest.Setup(m => m.SupplierStatusId).Returns(2);

            Assert.DoesNotThrowAsync(() =>  _solutionRepository.UpdateSupplierStatusAsync(mockUpdateSolutionSupplierStatusRequest.Object, new CancellationToken()));
        }
    }
}
