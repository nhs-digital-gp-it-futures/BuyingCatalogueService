using System;
using System.Data.SqlClient;
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
    public class SolutionDetailRepositoryTests
    {
        private Mock<IConfiguration> _configuration;

        private SolutionDetailRepository _solutionDetailRepository;

        private readonly Guid _org1Id = Guid.NewGuid();

        private readonly string _supplierId = "Sup 1";

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
                .WithId(_supplierId)
                .Build()
                .InsertAsync();

            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(a => a["ConnectionStrings:BuyingCatalogue"])
                .Returns(ConnectionStrings.ServiceConnectionString());

            _solutionDetailRepository = new SolutionDetailRepository(new DbConnectionFactory(_configuration.Object));
        }

        [Test]
        public async Task ShouldUpdateFeatures()
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
                .WithAboutUrl("AboutUrl")
                .WithFeatures("Features")
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            var mockUpdateSolutionFeaturesRequest = new Mock<IUpdateSolutionFeaturesRequest>();
            mockUpdateSolutionFeaturesRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionFeaturesRequest.Setup(m => m.Features).Returns("Features4");

            await _solutionDetailRepository.UpdateFeaturesAsync(mockUpdateSolutionFeaturesRequest.Object, new CancellationToken());

            var solution = await SolutionEntity.GetByIdAsync("Sln1");
            solution.Id.Should().Be("Sln1");

            var marketingData = await SolutionDetailEntity.GetBySolutionIdAsync("Sln1");
            marketingData.AboutUrl.Should().Be("AboutUrl");
            marketingData.Features.Should().Be("Features4");
        }

        [Test]
        public void ShouldThrowOnUpdateFeaturesNotPresent()
        {
            var mockUpdateSolutionFeaturesRequest = new Mock<IUpdateSolutionFeaturesRequest>();
            mockUpdateSolutionFeaturesRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionFeaturesRequest.Setup(m => m.Features).Returns("Features4");

            Assert.ThrowsAsync<SqlException>(() => _solutionDetailRepository.UpdateFeaturesAsync(mockUpdateSolutionFeaturesRequest.Object, new CancellationToken()));
        }

        [Test]
        public async Task ShouldThrowOnUpdateSolutionDetailNotPresent()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            var mockUpdateSolutionFeaturesRequest = new Mock<IUpdateSolutionFeaturesRequest>();
            mockUpdateSolutionFeaturesRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionFeaturesRequest.Setup(m => m.Features).Returns("Features4");

            Assert.ThrowsAsync<SqlException>(() => _solutionDetailRepository.UpdateFeaturesAsync(mockUpdateSolutionFeaturesRequest.Object, new CancellationToken()));

        }

        [Test]
        public async Task ShouldUpdateClientApplicationType()
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
                .WithAboutUrl("AboutUrl")
                .WithClientApplication("Browser-based")
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            var mockUpdateSolutionClientApplicationRequest = new Mock<IUpdateSolutionClientApplicationRequest>();
            mockUpdateSolutionClientApplicationRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionClientApplicationRequest.Setup(m => m.ClientApplication).Returns("Browser-based");

            await _solutionDetailRepository.UpdateClientApplicationAsync(mockUpdateSolutionClientApplicationRequest.Object, new CancellationToken());

            var solution = await SolutionEntity.GetByIdAsync("Sln1");
            solution.Id.Should().Be("Sln1");

            var marketingData = await SolutionDetailEntity.GetBySolutionIdAsync("Sln1");
            marketingData.AboutUrl.Should().Be("AboutUrl");
            marketingData.ClientApplication.Should().Be("Browser-based");
        }

        [Test]
        public void ShouldThrowOnUpdateClientApplicationSolutionNotPresent()
        {
            var mockUpdateSolutionClientApplicationRequest = new Mock<IUpdateSolutionClientApplicationRequest>();
            mockUpdateSolutionClientApplicationRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionClientApplicationRequest.Setup(m => m.ClientApplication).Returns("Browser-based");

            Assert.ThrowsAsync<SqlException>(() => _solutionDetailRepository.UpdateClientApplicationAsync(mockUpdateSolutionClientApplicationRequest.Object, new CancellationToken()));
        }

        [Test]
        public async Task ShouldThrowOnUpdateClientApplicationSolutionDetailNotPresent()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            var mockUpdateSolutionClientApplicationRequest = new Mock<IUpdateSolutionClientApplicationRequest>();
            mockUpdateSolutionClientApplicationRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionClientApplicationRequest.Setup(m => m.ClientApplication).Returns("Browser-based");

            Assert.ThrowsAsync<SqlException>(() => _solutionDetailRepository.UpdateClientApplicationAsync(mockUpdateSolutionClientApplicationRequest.Object, new CancellationToken()));
        }

        [Test]
        public async Task ShouldUpdateSummary()
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
                .WithAboutUrl("AboutUrl1")
                .WithFeatures("Features")
                .WithClientApplication("Browser-based")
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            var mockUpdateSolutionSummaryRequest = new Mock<IUpdateSolutionSummaryRequest>();
            mockUpdateSolutionSummaryRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionSummaryRequest.Setup(m => m.Summary).Returns("Sln4Summary");
            mockUpdateSolutionSummaryRequest.Setup(m => m.Description).Returns("Sln4Description");
            mockUpdateSolutionSummaryRequest.Setup(m => m.AboutUrl).Returns("AboutUrl4");

            await _solutionDetailRepository.UpdateSummaryAsync(mockUpdateSolutionSummaryRequest.Object, new CancellationToken());

            var solution = await SolutionEntity.GetByIdAsync("Sln1");
            solution.Id.Should().Be("Sln1");
            solution.Name.Should().Be("Solution1");

            solution = await SolutionEntity.GetByIdAsync("Sln2");
            solution.Id.Should().Be("Sln2");
            solution.Name.Should().Be("Solution2");

            var solutionDetail = await SolutionDetailEntity.GetBySolutionIdAsync("Sln1");
            solutionDetail.Summary.Should().Be("Sln4Summary");
            solutionDetail.FullDescription.Should().Be("Sln4Description");
            solutionDetail.AboutUrl.Should().Be("AboutUrl4");
            solutionDetail.Features.Should().Be("Features");
            solutionDetail.ClientApplication.Should().Be("Browser-based");
        }

        [Test]
        public void ShouldThrowOnUpdateSummarySolutionNotPresent()
        {
            var mockUpdateSolutionSummaryRequest = new Mock<IUpdateSolutionSummaryRequest>();
            mockUpdateSolutionSummaryRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionSummaryRequest.Setup(m => m.Summary).Returns("Sln4Summary");
            mockUpdateSolutionSummaryRequest.Setup(m => m.Description).Returns("Sln4Description");
            mockUpdateSolutionSummaryRequest.Setup(m => m.AboutUrl).Returns("AboutUrl4");

            Assert.ThrowsAsync<SqlException>(() => _solutionDetailRepository.UpdateSummaryAsync(mockUpdateSolutionSummaryRequest.Object, new CancellationToken()));
        }

        [Test]
        public async Task ShouldThrowOnUpdateSummarySolutionDetailNotPresent()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            var mockUpdateSolutionSummaryRequest = new Mock<IUpdateSolutionSummaryRequest>();
            mockUpdateSolutionSummaryRequest.Setup(m => m.Id).Returns("Sln1");
            mockUpdateSolutionSummaryRequest.Setup(m => m.Summary).Returns("Sln4Summary");
            mockUpdateSolutionSummaryRequest.Setup(m => m.Description).Returns("Sln4Description");
            mockUpdateSolutionSummaryRequest.Setup(m => m.AboutUrl).Returns("AboutUrl4");

            Assert.ThrowsAsync<SqlException>(() => _solutionDetailRepository.UpdateSummaryAsync(mockUpdateSolutionSummaryRequest.Object, new CancellationToken()));
        }
    }
}
