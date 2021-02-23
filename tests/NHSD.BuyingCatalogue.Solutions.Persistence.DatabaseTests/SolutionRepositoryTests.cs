using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests
{
    [TestFixture]
    internal sealed class SolutionRepositoryTests
    {
        private const string SupplierId = "Sup 1";
        private const string SupplierName = "Supplier1";
        private const string Solution1Id = "Sln1";

        private readonly Guid cap1Id = Guid.NewGuid();
        private readonly Guid cap2Id = Guid.NewGuid();
        private readonly DateTime lastUpdated = DateTime.Today;

        private ISolutionRepository solutionRepository;

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync();

            await SupplierEntityBuilder.Create()
                .WithId(SupplierId)
                .WithName(SupplierName)
                .Build()
                .InsertAsync();

            await CapabilityEntityBuilder
                .Create()
                .WithName("Cap1")
                .WithId(cap1Id)
                .WithDescription("Cap1Desc")
                .Build()
                .InsertAsync();

            await CapabilityEntityBuilder
                .Create()
                .WithName("Cap2")
                .WithId(cap2Id)
                .WithDescription("Cap2Desc")
                .Build()
                .InsertAsync();

            TestContext testContext = new TestContext();
            solutionRepository = testContext.SolutionRepository;
        }

        [Test]
        public async Task ShouldGetById()
        {
            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(Solution1Id)
                .WithName("Solution1")
                .WithPublishedStatusId((int)PublishedStatus.Published)
                .WithSupplierId(SupplierId)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(Solution1Id)
                .WithSummary("Sln1Summary")
                .WithFullDescription("Sln1Description")
                .WithAboutUrl("AboutUrl")
                .WithFeatures("Features")
                .WithClientApplication("Browser-based")
                .WithHosting("Hosting")
                .WithLastUpdated(lastUpdated)
                .Build()
                .InsertAsync();

            var solution = await solutionRepository.ByIdAsync(Solution1Id, CancellationToken.None);
            solution.Id.Should().Be(Solution1Id);
            solution.Name.Should().Be("Solution1");
            solution.LastUpdated.Should().Be(lastUpdated);
            solution.Summary.Should().Be("Sln1Summary");
            solution.Description.Should().Be("Sln1Description");
            solution.AboutUrl.Should().Be("AboutUrl");
            solution.Features.Should().Be("Features");
            solution.ClientApplication.Should().Be("Browser-based");
            solution.Hosting.Should().Be("Hosting");
            solution.IsFoundation.Should().BeFalse();
            solution.PublishedStatus.Should().Be(PublishedStatus.Published);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldGetByIdFoundation(bool isFoundation)
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

            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithFoundation(isFoundation)
                .Build()
                .InsertAsync();

            var solution = await solutionRepository.ByIdAsync(Solution1Id, CancellationToken.None);
            solution.Id.Should().Be(Solution1Id);
            solution.IsFoundation.Should().Be(isFoundation);
        }

        [Test]
        public async Task ShouldGetByIdNotPresent()
        {
            var solution = await solutionRepository.ByIdAsync(Solution1Id, CancellationToken.None);
            solution.Should().BeNull();
        }

        [Test]
        public async Task ShouldGetByIdSolutionDetailNotPresent()
        {
            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(Solution1Id)
                .WithName("Solution1")
                .WithPublishedStatusId((int)PublishedStatus.Published)
                .WithSupplierId(SupplierId)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(Solution1Id)
                .WithOnLastUpdated(lastUpdated)
                .Build()
                .InsertAsync();

            var solution = await solutionRepository.ByIdAsync(Solution1Id, CancellationToken.None);
            solution.Id.Should().Be(Solution1Id);
            solution.Name.Should().Be("Solution1");
            solution.LastUpdated.Should().Be(lastUpdated);
            solution.Summary.Should().BeNull();
            solution.Description.Should().BeNull();
            solution.AboutUrl.Should().BeNull();
            solution.Features.Should().BeNull();
            solution.ClientApplication.Should().BeNull();
        }

        [Test]
        public void SolutionIdDoesNotExist()
        {
            solutionRepository.CheckExists(Solution1Id, CancellationToken.None).Result.Should().BeFalse();
        }
    }
}
