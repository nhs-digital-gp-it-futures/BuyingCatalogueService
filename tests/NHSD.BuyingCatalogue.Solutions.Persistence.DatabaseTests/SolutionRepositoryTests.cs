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
    public class SolutionRepositoryTests
    {
        private readonly Guid _cap1Id = Guid.NewGuid();
        private readonly Guid _cap2Id = Guid.NewGuid();

        private readonly string _supplierId = "Sup 1";
        private readonly string _supplierName = "Supplier1";

        private readonly DateTime _lastUpdated = DateTime.Today;

        private readonly string _solution1Id = "Sln1";

        private ISolutionRepository _solutionRepository;

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync();

            await SupplierEntityBuilder.Create()
                .WithId(_supplierId)
                .WithName(_supplierName)
                .Build()
                .InsertAsync();

            await CapabilityEntityBuilder.Create().WithName("Cap1").WithId(_cap1Id).WithDescription("Cap1Desc").Build().InsertAsync();
            await CapabilityEntityBuilder.Create().WithName("Cap2").WithId(_cap2Id).WithDescription("Cap2Desc").Build().InsertAsync();

            TestContext testContext = new TestContext();
            _solutionRepository = testContext.SolutionRepository;
        }

        [Test]
        public async Task ShouldGetById()
        {
            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(_solution1Id)
                .WithName("Solution1")
                .WithPublishedStatusId((int)PublishedStatus.Published)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(_solution1Id)
                .WithOnLastUpdated(_lastUpdated)
                .Build()
                .InsertAsync();

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(_solution1Id)
                .WithSummary("Sln1Summary")
                .WithFullDescription("Sln1Description")
                .WithAboutUrl("AboutUrl")
                .WithFeatures("Features")
                .WithClientApplication("Browser-based")
                .WithHosting("Hosting")
                .WithLastUpdated(_lastUpdated)
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            var solution = await _solutionRepository.ByIdAsync(_solution1Id, new CancellationToken());
            solution.Id.Should().Be(_solution1Id);
            solution.Name.Should().Be("Solution1");
            solution.LastUpdated.Should().Be(_lastUpdated);
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
                .WithCatalogueItemId(_solution1Id)
                .WithName(_solution1Id)
                .WithPublishedStatusId((int)PublishedStatus.Published)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(_solution1Id)
                .Build()
                .InsertAsync();

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(_solution1Id)
                .Build()
                .InsertAndSetCurrentForSolutionAsync();

            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId(_solution1Id)
                .WithFoundation(isFoundation)
                .Build()
                .InsertAsync();

            var solution = await _solutionRepository.ByIdAsync(_solution1Id, new CancellationToken());
            solution.Id.Should().Be(_solution1Id);
            solution.IsFoundation.Should().Be(isFoundation);
        }

        [Test]
        public async Task ShouldGetByIdNotPresent()
        {
            var solution = await _solutionRepository.ByIdAsync(_solution1Id, new CancellationToken());
            solution.Should().BeNull();
        }

        [Test]
        public async Task ShouldGetByIdSolutionDetailNotPresent()
        {
            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(_solution1Id)
                .WithName("Solution1")
                .WithPublishedStatusId((int)PublishedStatus.Published)
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(_solution1Id)
                .WithOnLastUpdated(_lastUpdated)
                .Build()
                .InsertAsync();

            var solution = await _solutionRepository.ByIdAsync(_solution1Id, new CancellationToken());
            solution.Id.Should().Be(_solution1Id);
            solution.Name.Should().Be("Solution1");
            solution.LastUpdated.Should().Be(_lastUpdated);
            solution.Summary.Should().BeNull();
            solution.Description.Should().BeNull();
            solution.AboutUrl.Should().BeNull();
            solution.Features.Should().BeNull();
            solution.ClientApplication.Should().BeNull();
        }

        [Test]
        public void SolutionIdDoesNotExist()
        {
            _solutionRepository.CheckExists(_solution1Id, new CancellationToken()).Result.Should().BeFalse();
        }
    }
}
