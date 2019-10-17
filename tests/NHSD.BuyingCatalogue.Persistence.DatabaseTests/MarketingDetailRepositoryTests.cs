using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class MarketingDetailRepositoryTests
    {
        private Mock<IConfiguration> _configuration;

        private MarketingDetailRepository _marketingDetailRepository;

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

            await CapabilityEntityBuilder.Create().WithName("Cap1").WithId(Cap1Id).WithDescription("Cap1Desc").Build()
                .InsertAsync();
            await CapabilityEntityBuilder.Create().WithName("Cap2").WithId(Cap2Id).WithDescription("Cap2Desc").Build()
                .InsertAsync();
            await CapabilityEntityBuilder.Create().WithName("Cap3").WithId(Cap3Id).WithDescription("Cap3Desc").Build()
                .InsertAsync();

            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(a => a["ConnectionStrings:BuyingCatalogue"])
                .Returns(ConnectionStrings.ServiceConnectionString());

            _marketingDetailRepository = new MarketingDetailRepository(new DbConnectionFactory(_configuration.Object));
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

            var mockUpdateSolutionFeaturesRequest = new Mock<IUpdateSolutionFeaturesRequest>();
            mockUpdateSolutionFeaturesRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionFeaturesRequest.Setup(m => m.Features).Returns("Features4");

            await _marketingDetailRepository.UpdateFeaturesAsync(mockUpdateSolutionFeaturesRequest.Object, new CancellationToken());

            var solution = await SolutionEntity.GetByIdAsync("Sln1");
            solution.Id.Should().Be("Sln1");

            var marketingData = await MarketingDetailEntity.GetBySolutionIdAsync("Sln1");
            marketingData.AboutUrl.Should().Be("AboutUrl");
            marketingData.Features.Should().Be("Features4");
        }

        [Test]
        public void ShouldUpdateNotPresent()
        {
            var mockUpdateSolutionFeaturesRequest = new Mock<IUpdateSolutionFeaturesRequest>();
            mockUpdateSolutionFeaturesRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionFeaturesRequest.Setup(m => m.Features).Returns("Features4");

            Assert.DoesNotThrowAsync(() => _marketingDetailRepository.UpdateFeaturesAsync(mockUpdateSolutionFeaturesRequest.Object, new CancellationToken()));
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

            var mockUpdateSolutionFeaturesRequest = new Mock<IUpdateSolutionFeaturesRequest>();
            mockUpdateSolutionFeaturesRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionFeaturesRequest.Setup(m => m.Features).Returns("Features4");

            await _marketingDetailRepository.UpdateFeaturesAsync(mockUpdateSolutionFeaturesRequest.Object, new CancellationToken());

            var solution = await SolutionEntity.GetByIdAsync("Sln1");
            solution.Id.Should().Be("Sln1");

            var marketingData = await MarketingDetailEntity.GetBySolutionIdAsync("Sln1");
            marketingData.AboutUrl.Should().BeNull();
            marketingData.Features.Should().Be("Features4");
        }


    }
}
