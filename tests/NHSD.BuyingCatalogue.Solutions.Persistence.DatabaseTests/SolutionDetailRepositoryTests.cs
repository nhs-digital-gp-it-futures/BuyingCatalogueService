using System;
using System.Data.SqlClient;
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
    internal sealed class SolutionDetailRepositoryTests
    {
        private const string Solution1Id = "Sln1";
        private const string Solution2Id = "Sln2";

        private const string SupplierId = "Sup 1";

        private ISolutionDetailRepository solutionDetailRepository;

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync();

            await SupplierEntityBuilder.Create()
                .WithId(SupplierId)
                .Build()
                .InsertAsync();

            TestContext testContext = new TestContext();
            solutionDetailRepository = testContext.SolutionDetailRepository;
        }

        [Test]
        public async Task ShouldUpdateFeatures()
        {
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

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithAboutUrl("AboutUrl")
                .WithFeatures("Features")
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            var mockUpdateSolutionFeaturesRequest = new Mock<IUpdateSolutionFeaturesRequest>();
            mockUpdateSolutionFeaturesRequest.Setup(m => m.SolutionId).Returns(Solution1Id);
            mockUpdateSolutionFeaturesRequest.Setup(m => m.Features).Returns("Features4");

            await solutionDetailRepository.UpdateFeaturesAsync(
                mockUpdateSolutionFeaturesRequest.Object,
                CancellationToken.None);

            var solution = await SolutionEntity.GetByIdAsync(Solution1Id);
            solution.Id.Should().Be(Solution1Id);

            var marketingData = await SolutionDetailEntity.GetBySolutionIdAsync(Solution1Id);
            marketingData.AboutUrl.Should().Be("AboutUrl");
            marketingData.Features.Should().Be("Features4");

            (await marketingData.LastUpdated.SecondsFromNow()).Should().BeLessOrEqualTo(5);
        }

        [Test]
        public void ShouldThrowOnUpdateFeaturesNotPresent()
        {
            var mockUpdateSolutionFeaturesRequest = new Mock<IUpdateSolutionFeaturesRequest>();
            mockUpdateSolutionFeaturesRequest.Setup(m => m.SolutionId).Returns(Solution1Id);
            mockUpdateSolutionFeaturesRequest.Setup(m => m.Features).Returns("Features4");

            Assert.ThrowsAsync<SqlException>(() => solutionDetailRepository.UpdateFeaturesAsync(
                mockUpdateSolutionFeaturesRequest.Object,
                CancellationToken.None));
        }

        [Test]
        public async Task ShouldUpdateClientApplicationType()
        {
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

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithAboutUrl("AboutUrl")
                .WithClientApplication("Native-Desktop")
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            var mockUpdateSolutionClientApplicationRequest = new Mock<IUpdateSolutionClientApplicationRequest>();
            mockUpdateSolutionClientApplicationRequest.Setup(m => m.SolutionId).Returns(Solution1Id);
            mockUpdateSolutionClientApplicationRequest.Setup(m => m.ClientApplication).Returns("Browser-based");

            await solutionDetailRepository.UpdateClientApplicationAsync(
                mockUpdateSolutionClientApplicationRequest.Object,
                CancellationToken.None);

            var solution = await SolutionEntity.GetByIdAsync(Solution1Id);
            solution.Id.Should().Be(Solution1Id);

            var marketingData = await SolutionDetailEntity.GetBySolutionIdAsync(Solution1Id);
            marketingData.AboutUrl.Should().Be("AboutUrl");
            marketingData.ClientApplication.Should().Be("Browser-based");

            (await marketingData.LastUpdated.SecondsFromNow()).Should().BeLessOrEqualTo(5);
        }

        [Test]
        public void ShouldThrowOnUpdateClientApplicationSolutionNotPresent()
        {
            var mockUpdateSolutionClientApplicationRequest = new Mock<IUpdateSolutionClientApplicationRequest>();
            mockUpdateSolutionClientApplicationRequest.Setup(m => m.SolutionId).Returns(Solution1Id);
            mockUpdateSolutionClientApplicationRequest.Setup(m => m.ClientApplication).Returns("Browser-based");

            Assert.ThrowsAsync<SqlException>(() => solutionDetailRepository.UpdateClientApplicationAsync(
                mockUpdateSolutionClientApplicationRequest.Object,
                CancellationToken.None));
        }

        [Test]
        public async Task ShouldUpdateSummary()
        {
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

            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(Solution2Id)
                .WithName(Solution2Id)
                .WithPublishedStatusId((int)PublishedStatus.Published)
                .WithSupplierId(SupplierId)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(Solution2Id)
                .Build()
                .InsertAsync();

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithAboutUrl("AboutUrl1")
                .WithFeatures("Features")
                .WithClientApplication("Browser-based")
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            var mockUpdateSolutionSummaryRequest = new Mock<IUpdateSolutionSummaryRequest>();
            mockUpdateSolutionSummaryRequest.Setup(m => m.SolutionId).Returns(Solution1Id);
            mockUpdateSolutionSummaryRequest.Setup(m => m.Summary).Returns("Sln4Summary");
            mockUpdateSolutionSummaryRequest.Setup(m => m.Description).Returns("Sln4Description");
            mockUpdateSolutionSummaryRequest.Setup(m => m.AboutUrl).Returns("AboutUrl4");

            await solutionDetailRepository.UpdateSummaryAsync(
                mockUpdateSolutionSummaryRequest.Object,
                CancellationToken.None);

            var solution = await SolutionEntity.GetByIdAsync(Solution1Id);
            solution.Id.Should().Be(Solution1Id);

            solution = await SolutionEntity.GetByIdAsync(Solution2Id);
            solution.Id.Should().Be(Solution2Id);

            var solutionDetail = await SolutionDetailEntity.GetBySolutionIdAsync(Solution1Id);
            solutionDetail.Summary.Should().Be("Sln4Summary");
            solutionDetail.FullDescription.Should().Be("Sln4Description");
            solutionDetail.AboutUrl.Should().Be("AboutUrl4");
            solutionDetail.Features.Should().Be("Features");
            solutionDetail.ClientApplication.Should().Be("Browser-based");

            (await solutionDetail.LastUpdated.SecondsFromNow()).Should().BeLessOrEqualTo(5);
        }

        [Test]
        public void ShouldThrowOnUpdateSummarySolutionNotPresent()
        {
            var mockUpdateSolutionSummaryRequest = new Mock<IUpdateSolutionSummaryRequest>();
            mockUpdateSolutionSummaryRequest.Setup(m => m.SolutionId).Returns(Solution1Id);
            mockUpdateSolutionSummaryRequest.Setup(m => m.Summary).Returns("Sln4Summary");
            mockUpdateSolutionSummaryRequest.Setup(m => m.Description).Returns("Sln4Description");
            mockUpdateSolutionSummaryRequest.Setup(m => m.AboutUrl).Returns("AboutUrl4");

            Assert.ThrowsAsync<SqlException>(() => solutionDetailRepository.UpdateSummaryAsync(
                mockUpdateSolutionSummaryRequest.Object,
                CancellationToken.None));
        }

        [Test]
        public async Task ShouldRetrieveClientApplicationDetailsWhenPresent()
        {
            const string expectedClientApplication = "I am the client application string";

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

            await SolutionDetailEntityBuilder.Create()
                .WithClientApplication(expectedClientApplication)
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            var result = await solutionDetailRepository.GetClientApplicationBySolutionIdAsync(
                Solution1Id,
                CancellationToken.None);

            result.ClientApplication.Should().Be(expectedClientApplication);
        }

        [Test]
        public async Task ShouldRetrieveNullClientApplicationWhenSolutionDetailDoesNotExist()
        {
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

            var result = await solutionDetailRepository.GetClientApplicationBySolutionIdAsync(
                Solution1Id,
                CancellationToken.None);

            result.ClientApplication.Should().BeNull();
        }

        [Test]
        public async Task ShouldRetrieveNullResultWhenSolutionDoesNotExist()
        {
            var result = await solutionDetailRepository.GetClientApplicationBySolutionIdAsync(
                Solution1Id,
                CancellationToken.None);

            result.Should().BeNull();
        }

        [Test]
        public void ShouldThrowOnUpdateFeaturesNullRequest()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => solutionDetailRepository.UpdateFeaturesAsync(
                null,
                CancellationToken.None));
        }

        [Test]
        public void ShouldThrowOnUpdateClientApplicationNullRequest()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => solutionDetailRepository.UpdateClientApplicationAsync(
                null,
                CancellationToken.None));
        }

        [Test]
        public void ShouldThrowOnUpdateSummaryNullRequest()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => solutionDetailRepository.UpdateSummaryAsync(
                null,
                CancellationToken.None));
        }

        [Test]
        public async Task ShouldUpdateHosting()
        {
            const string expectedResult = "{ 'SomethingElse': [] }";

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

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithHosting("{ 'Something': [] }")
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            var mockUpdateHostingRequest = new Mock<IUpdateSolutionHostingRequest>();
            mockUpdateHostingRequest.Setup(m => m.SolutionId).Returns(Solution1Id);
            mockUpdateHostingRequest.Setup(m => m.Hosting).Returns(expectedResult);

            await solutionDetailRepository.UpdateHostingAsync(
                mockUpdateHostingRequest.Object,
                CancellationToken.None);

            var solution = await SolutionEntity.GetByIdAsync(Solution1Id);
            solution.Id.Should().Be(Solution1Id);

            var marketingData = await SolutionDetailEntity.GetBySolutionIdAsync(Solution1Id);
            marketingData.Hosting.Should().Be(expectedResult);

            (await marketingData.LastUpdated.SecondsFromNow()).Should().BeLessOrEqualTo(5);
        }

        [Test]
        public async Task ShouldRetrieveHostingDetailsWhenPresent()
        {
            const string expectedHostingString = "I am the hosting string";

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

            await SolutionDetailEntityBuilder.Create()
                .WithHosting(expectedHostingString)
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            var result = await solutionDetailRepository.GetHostingBySolutionIdAsync(
                Solution1Id,
                CancellationToken.None);

            result.Hosting.Should().Be(expectedHostingString);
        }

        [Test]
        public async Task ShouldRetrieveNullHostingWhenSolutionDetailDoesNotExist()
        {
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

            var result = await solutionDetailRepository.GetHostingBySolutionIdAsync(
                Solution1Id,
                CancellationToken.None);

            result.Hosting.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateRoadMap()
        {
            const string expectedResult = "some road map description";

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

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithRoadMap(expectedResult)
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            var mockRequest = new Mock<IUpdateRoadMapRequest>();
            mockRequest.Setup(m => m.SolutionId).Returns(Solution1Id);
            mockRequest.Setup(m => m.Description).Returns(expectedResult);

            await solutionDetailRepository.UpdateRoadMapAsync(mockRequest.Object, CancellationToken.None);

            var solution = await SolutionEntity.GetByIdAsync(Solution1Id);
            solution.Id.Should().Be(Solution1Id);

            var marketingData = await SolutionDetailEntity.GetBySolutionIdAsync(Solution1Id);
            marketingData.RoadMap.Should().Be(expectedResult);

            (await marketingData.LastUpdated.SecondsFromNow()).Should().BeLessOrEqualTo(5);
        }

        [Test]
        public async Task ShouldRetrieveRoadMapWhenPresent()
        {
            const string expectedRoadMapString = "I am the road map string";

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

            await SolutionDetailEntityBuilder.Create()
                .WithRoadMap(expectedRoadMapString)
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            var result = await solutionDetailRepository.GetRoadMapBySolutionIdAsync(
                Solution1Id,
                CancellationToken.None);

            result.Summary.Should().Be(expectedRoadMapString);
        }

        [Test]
        public async Task ShouldRetrieveNullRoadMapWhenSolutionDetailDoesNotExist()
        {
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

            var result = await solutionDetailRepository.GetRoadMapBySolutionIdAsync(
                Solution1Id,
                CancellationToken.None);

            result.Summary.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateImplementationTimescales()
        {
            const string expectedResult = "some implementation timescales description";

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

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithImplementationTimescales(expectedResult)
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            var mockRequest = new Mock<IUpdateImplementationTimescalesRequest>();
            mockRequest.Setup(m => m.SolutionId).Returns(Solution1Id);
            mockRequest.Setup(m => m.Description).Returns(expectedResult);

            await solutionDetailRepository.UpdateImplementationTimescalesAsync(
                mockRequest.Object,
                CancellationToken.None);

            var solution = await SolutionEntity.GetByIdAsync(Solution1Id);
            solution.Id.Should().Be(Solution1Id);

            var marketingData = await SolutionDetailEntity.GetBySolutionIdAsync(Solution1Id);
            marketingData.ImplementationDetail.Should().Be(expectedResult);

            (await marketingData.LastUpdated.SecondsFromNow()).Should().BeLessOrEqualTo(5);
        }

        [Test]
        public async Task ShouldRetrieveImplementationTimescalesWhenPresent()
        {
            const string expectedImplementationTimescalesString = "I am the integrations timescales description";

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

            await SolutionDetailEntityBuilder.Create()
                .WithImplementationTimescales(expectedImplementationTimescalesString)
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            var result = await solutionDetailRepository.GetImplementationTimescalesBySolutionIdAsync(
                Solution1Id,
                CancellationToken.None);

            result.Description.Should().Be(expectedImplementationTimescalesString);
        }

        [Test]
        public async Task ShouldRetrieveNullImplementationTimescalesWhenSolutionDetailDoesNotExist()
        {
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

            var result = await solutionDetailRepository.GetImplementationTimescalesBySolutionIdAsync(
                Solution1Id,
                CancellationToken.None);

            result.Description.Should().BeNull();
        }
    }
}
