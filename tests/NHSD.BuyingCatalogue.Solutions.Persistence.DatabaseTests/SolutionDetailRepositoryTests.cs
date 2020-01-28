using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests
{
    [TestFixture]
    public class SolutionDetailRepositoryTests
    {
        private readonly string _supplierId = "Sup 1";

        private readonly string _solution1Id = "Sln1";
        private readonly string _solution2Id = "Sln2";

        private ISolutionDetailRepository _solutionDetailRepository;

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync().ConfigureAwait(false);

            await SupplierEntityBuilder.Create()
                .WithId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);
            
            TestContext testContext = new TestContext();
            _solutionDetailRepository = testContext.SolutionDetailRepository;
        }

        [Test]
        public async Task ShouldUpdateFeatures()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId(_solution1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(_solution1Id)
                .WithAboutUrl("AboutUrl")
                .WithFeatures("Features")
                .Build()
                .InsertAndSetCurrentForSolutionAsync()
                .ConfigureAwait(false);

            var mockUpdateSolutionFeaturesRequest = new Mock<IUpdateSolutionFeaturesRequest>();
            mockUpdateSolutionFeaturesRequest.Setup(m => m.SolutionId).Returns(_solution1Id);
            mockUpdateSolutionFeaturesRequest.Setup(m => m.Features).Returns("Features4");

            await _solutionDetailRepository.UpdateFeaturesAsync(mockUpdateSolutionFeaturesRequest.Object, new CancellationToken())
                .ConfigureAwait(false);

            var solution = await SolutionEntity.GetByIdAsync(_solution1Id)
                .ConfigureAwait(false);
            solution.Id.Should().Be(_solution1Id);

            var marketingData = await SolutionDetailEntity.GetBySolutionIdAsync(_solution1Id)
                .ConfigureAwait(false);
            marketingData.AboutUrl.Should().Be("AboutUrl");
            marketingData.Features.Should().Be("Features4");

            (await marketingData.LastUpdated.SecondsFromNow().ConfigureAwait(false)).Should().BeLessOrEqualTo(5);
        }

        [Test]
        public void ShouldThrowOnUpdateFeaturesNotPresent()
        {
            var mockUpdateSolutionFeaturesRequest = new Mock<IUpdateSolutionFeaturesRequest>();
            mockUpdateSolutionFeaturesRequest.Setup(m => m.SolutionId).Returns(_solution1Id);
            mockUpdateSolutionFeaturesRequest.Setup(m => m.Features).Returns("Features4");

            Assert.ThrowsAsync<SqlException>(() => _solutionDetailRepository.UpdateFeaturesAsync(mockUpdateSolutionFeaturesRequest.Object, new CancellationToken()));
        }

        [Test]
        public async Task ShouldThrowOnUpdateSolutionDetailNotPresent()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId(_solution1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            var mockUpdateSolutionFeaturesRequest = new Mock<IUpdateSolutionFeaturesRequest>();
            mockUpdateSolutionFeaturesRequest.Setup(m => m.SolutionId).Returns(_solution1Id);
            mockUpdateSolutionFeaturesRequest.Setup(m => m.Features).Returns("Features4");

            Assert.ThrowsAsync<SqlException>(() => _solutionDetailRepository.UpdateFeaturesAsync(mockUpdateSolutionFeaturesRequest.Object, new CancellationToken()));

        }

        [Test]
        public async Task ShouldUpdateClientApplicationType()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId(_solution1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(_solution1Id)
                .WithAboutUrl("AboutUrl")
                .WithClientApplication("Native-Desktop")
                .Build()
                .InsertAndSetCurrentForSolutionAsync()
                .ConfigureAwait(false);

            var mockUpdateSolutionClientApplicationRequest = new Mock<IUpdateSolutionClientApplicationRequest>();
            mockUpdateSolutionClientApplicationRequest.Setup(m => m.SolutionId).Returns(_solution1Id);
            mockUpdateSolutionClientApplicationRequest.Setup(m => m.ClientApplication).Returns("Browser-based");

            await _solutionDetailRepository.UpdateClientApplicationAsync(mockUpdateSolutionClientApplicationRequest.Object, new CancellationToken())
                .ConfigureAwait(false);

            var solution = await SolutionEntity.GetByIdAsync(_solution1Id)
                .ConfigureAwait(false);
            solution.Id.Should().Be(_solution1Id);

            var marketingData = await SolutionDetailEntity.GetBySolutionIdAsync(_solution1Id)
                .ConfigureAwait(false);
            marketingData.AboutUrl.Should().Be("AboutUrl");
            marketingData.ClientApplication.Should().Be("Browser-based");

            (await marketingData.LastUpdated.SecondsFromNow().ConfigureAwait(false)).Should().BeLessOrEqualTo(5);
        }

        [Test]
        public void ShouldThrowOnUpdateClientApplicationSolutionNotPresent()
        {
            var mockUpdateSolutionClientApplicationRequest = new Mock<IUpdateSolutionClientApplicationRequest>();
            mockUpdateSolutionClientApplicationRequest.Setup(m => m.SolutionId).Returns(_solution1Id);
            mockUpdateSolutionClientApplicationRequest.Setup(m => m.ClientApplication).Returns("Browser-based");

            Assert.ThrowsAsync<SqlException>(() => _solutionDetailRepository.UpdateClientApplicationAsync(mockUpdateSolutionClientApplicationRequest.Object, new CancellationToken()));
        }

        [Test]
        public async Task ShouldThrowOnUpdateClientApplicationSolutionDetailNotPresent()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId(_solution1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            var mockUpdateSolutionClientApplicationRequest = new Mock<IUpdateSolutionClientApplicationRequest>();
            mockUpdateSolutionClientApplicationRequest.Setup(m => m.SolutionId).Returns(_solution1Id);
            mockUpdateSolutionClientApplicationRequest.Setup(m => m.ClientApplication).Returns("Browser-based");

            Assert.ThrowsAsync<SqlException>(() => _solutionDetailRepository.UpdateClientApplicationAsync(mockUpdateSolutionClientApplicationRequest.Object, new CancellationToken()));
        }

        [Test]
        public async Task ShouldUpdateSummary()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId(_solution1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SolutionEntityBuilder.Create()
                .WithName("Solution2")
                .WithId(_solution2Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(_solution1Id)
                .WithAboutUrl("AboutUrl1")
                .WithFeatures("Features")
                .WithClientApplication("Browser-based")
                .Build()
                .InsertAndSetCurrentForSolutionAsync()
                .ConfigureAwait(false);

            var mockUpdateSolutionSummaryRequest = new Mock<IUpdateSolutionSummaryRequest>();
            mockUpdateSolutionSummaryRequest.Setup(m => m.SolutionId).Returns(_solution1Id);
            mockUpdateSolutionSummaryRequest.Setup(m => m.Summary).Returns("Sln4Summary");
            mockUpdateSolutionSummaryRequest.Setup(m => m.Description).Returns("Sln4Description");
            mockUpdateSolutionSummaryRequest.Setup(m => m.AboutUrl).Returns("AboutUrl4");

            await _solutionDetailRepository.UpdateSummaryAsync(mockUpdateSolutionSummaryRequest.Object, new CancellationToken())
                .ConfigureAwait(false);

            var solution = await SolutionEntity.GetByIdAsync(_solution1Id)
                .ConfigureAwait(false);
            solution.Id.Should().Be(_solution1Id);
            solution.Name.Should().Be("Solution1");

            solution = await SolutionEntity.GetByIdAsync(_solution2Id)
                .ConfigureAwait(false);
            solution.Id.Should().Be(_solution2Id);
            solution.Name.Should().Be("Solution2");

            var solutionDetail = await SolutionDetailEntity.GetBySolutionIdAsync(_solution1Id)
                .ConfigureAwait(false);
            solutionDetail.Summary.Should().Be("Sln4Summary");
            solutionDetail.FullDescription.Should().Be("Sln4Description");
            solutionDetail.AboutUrl.Should().Be("AboutUrl4");
            solutionDetail.Features.Should().Be("Features");
            solutionDetail.ClientApplication.Should().Be("Browser-based");
            (await solutionDetail.LastUpdated.SecondsFromNow().ConfigureAwait(false)).Should().BeLessOrEqualTo(5);
        }

        [Test]
        public void ShouldThrowOnUpdateSummarySolutionNotPresent()
        {
            var mockUpdateSolutionSummaryRequest = new Mock<IUpdateSolutionSummaryRequest>();
            mockUpdateSolutionSummaryRequest.Setup(m => m.SolutionId).Returns(_solution1Id);
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
                .WithId(_solution1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            var mockUpdateSolutionSummaryRequest = new Mock<IUpdateSolutionSummaryRequest>();
            mockUpdateSolutionSummaryRequest.Setup(m => m.SolutionId).Returns(_solution1Id);
            mockUpdateSolutionSummaryRequest.Setup(m => m.Summary).Returns("Sln4Summary");
            mockUpdateSolutionSummaryRequest.Setup(m => m.Description).Returns("Sln4Description");
            mockUpdateSolutionSummaryRequest.Setup(m => m.AboutUrl).Returns("AboutUrl4");

            Assert.ThrowsAsync<SqlException>(() => _solutionDetailRepository.UpdateSummaryAsync(mockUpdateSolutionSummaryRequest.Object, new CancellationToken()));
        }

        [Test]
        public async Task ShouldRetrieveClientApplicationDetailsWhenPresent()
        {
            const string expectedClientApplication = "I am the client application string";
            
            await SolutionEntityBuilder.Create()
                .WithId(_solution1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);
            await SolutionDetailEntityBuilder.Create()
                .WithClientApplication(expectedClientApplication)
                .Build()
                .InsertAndSetCurrentForSolutionAsync()
                .ConfigureAwait(false);

            var result = await _solutionDetailRepository.GetClientApplicationBySolutionIdAsync(_solution1Id, new CancellationToken())
                .ConfigureAwait(false);
            result.ClientApplication.Should().Be(expectedClientApplication);
        }

        [Test]
        public async Task ShouldRetrieveNullClientApplicationWhenSolutionDetailDoesNotExist()
        {
            await SolutionEntityBuilder.Create()
                .WithId(_solution1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            var result = await _solutionDetailRepository.GetClientApplicationBySolutionIdAsync(_solution1Id, new CancellationToken())
                .ConfigureAwait(false);
            result.ClientApplication.Should().BeNull();
        }

        [Test]
        public async Task ShouldRetrieveNullResultWhenSolutionDoesNotExist()
        {
            var result = await _solutionDetailRepository.GetClientApplicationBySolutionIdAsync(_solution1Id, new CancellationToken())
                .ConfigureAwait(false);
            result.Should().BeNull();
        }

        [Test]
        public void ShouldThrowOnUpdateFeaturesNullRequest()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _solutionDetailRepository.UpdateFeaturesAsync(null, new CancellationToken()));
        }

        [Test]
        public void ShouldThrowOnUpdateClientApplicationNullRequest()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _solutionDetailRepository.UpdateClientApplicationAsync(null, new CancellationToken()));
        }

        [Test]
        public void ShouldThrowOnUpdateSummaryNullRequest()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _solutionDetailRepository.UpdateSummaryAsync(null, new CancellationToken()));
        }

        [Test]
        public async Task ShouldUpdateHosting()
        {
            string expectedResult = "{ 'SomethingElse': [] }";

            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId(_solution1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(_solution1Id)
                .WithHosting("{ 'Something': [] }")
                .Build()
                .InsertAndSetCurrentForSolutionAsync()
                .ConfigureAwait(false);

            var mockUpdateHostingRequest = new Mock<IUpdateSolutionHostingRequest>();
            mockUpdateHostingRequest.Setup(m => m.SolutionId).Returns(_solution1Id);
            mockUpdateHostingRequest.Setup(m => m.Hosting).Returns(expectedResult);

            await _solutionDetailRepository.UpdateHostingAsync(mockUpdateHostingRequest.Object, new CancellationToken())
                .ConfigureAwait(false);

            var solution = await SolutionEntity.GetByIdAsync(_solution1Id)
                .ConfigureAwait(false);
            solution.Id.Should().Be(_solution1Id);

            var marketingData = await SolutionDetailEntity.GetBySolutionIdAsync(_solution1Id)
                .ConfigureAwait(false);
            marketingData.Hosting.Should().Be(expectedResult);

            (await marketingData.LastUpdated.SecondsFromNow().ConfigureAwait(false)).Should().BeLessOrEqualTo(5);
        }

        [Test]
        public async Task ShouldRetrieveHostingDetailsWhenPresent()
        {
            const string expectedHostingString = "I am the hosting string";

            await SolutionEntityBuilder.Create()
                .WithId(_solution1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);
            await SolutionDetailEntityBuilder.Create()
                .WithHosting(expectedHostingString)
                .Build()
                .InsertAndSetCurrentForSolutionAsync()
                .ConfigureAwait(false);

            var result = await _solutionDetailRepository.GetHostingBySolutionIdAsync(_solution1Id, new CancellationToken())
                .ConfigureAwait(false);
            result.Hosting.Should().Be(expectedHostingString);
        }

        [Test]
        public async Task ShouldRetrieveNullHostingWhenSolutionDetailDoesNotExist()
        {
            await SolutionEntityBuilder.Create()
                .WithId(_solution1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            var result = await _solutionDetailRepository.GetHostingBySolutionIdAsync(_solution1Id, new CancellationToken())
                .ConfigureAwait(false);
            result.Hosting.Should().BeNull();
        }

        [Test]
        public async Task ShouldUpdateRoadmap()
        {
            string expectedResult = "some roadmap description";
            
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId(_solution1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(_solution1Id)
                .WithRoadMap(expectedResult)
                .Build()
                .InsertAndSetCurrentForSolutionAsync()
                .ConfigureAwait(false);

            var mockRequest = new Mock<IUpdateRoadmapRequest>();
            mockRequest.Setup(m => m.SolutionId).Returns(_solution1Id);
            mockRequest.Setup(m => m.Description).Returns(expectedResult);

            await _solutionDetailRepository.UpdateRoadmapAsync(mockRequest.Object, new CancellationToken())
                .ConfigureAwait(false);

            var solution = await SolutionEntity.GetByIdAsync(_solution1Id)
                .ConfigureAwait(false);
            solution.Id.Should().Be(_solution1Id);

            var marketingData = await SolutionDetailEntity.GetBySolutionIdAsync(_solution1Id)
                .ConfigureAwait(false);
            marketingData.RoadMap.Should().Be(expectedResult);

            (await marketingData.LastUpdated.SecondsFromNow().ConfigureAwait(false)).Should().BeLessOrEqualTo(5);
        }

        [Test]
        public async Task ShouldRetrieveRoadmapWhenPresent()
        {
            const string expectedRoadmapString = "I am the roadmap string";
            
            await SolutionEntityBuilder.Create()
                .WithId(_solution1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);
            await SolutionDetailEntityBuilder.Create()
                .WithRoadMap(expectedRoadmapString)
                .Build()
                .InsertAndSetCurrentForSolutionAsync()
                .ConfigureAwait(false);

            var result = await _solutionDetailRepository.GetRoadMapBySolutionIdAsync(_solution1Id, new CancellationToken())
                .ConfigureAwait(false);
            result.Summary.Should().Be(expectedRoadmapString);
        }

        [Test]
        public async Task ShouldRetrieveNullRoadmapWhenSolutionDetailDoesNotExist()
        {
            await SolutionEntityBuilder.Create()
                .WithId(_solution1Id)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            var result = await _solutionDetailRepository.GetRoadMapBySolutionIdAsync(_solution1Id, new CancellationToken())
                .ConfigureAwait(false);
            result.Summary.Should().BeNull();
        }
    }
}
