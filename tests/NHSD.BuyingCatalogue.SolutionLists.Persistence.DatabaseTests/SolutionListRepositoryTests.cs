using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.SolutionLists.Persistence.DatabaseTests
{
    [TestFixture]
    public class SolutionListRepositoryTests
    {
        private const string Solution1Id = "Sln1";
        private const string Solution2Id = "Sln2";
        private const string Solution3Id = "Sln3";
        private const string Solution4Id = "Sln4";

        private readonly Guid _cap1Id = Guid.NewGuid();
        private readonly Guid _cap2Id = Guid.NewGuid();

        private const string CapabilityReference1 = "C1";
        private const string CapabilityReference2 = "C2";

        private const string _supplierId = "Sup 1";
        private readonly string _supplierName = "Supplier 1";

        private ISolutionListRepository _solutionListRepository;

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync();

            await SupplierEntityBuilder.Create()
                .WithId(_supplierId)
                .WithName(_supplierName)
                .Build()
                .InsertAsync();

            await CapabilityEntityBuilder.Create().WithName("Cap1").WithId(_cap1Id).WithCapabilityRef(CapabilityReference1).WithDescription("Cap1Desc").Build().InsertAsync();
            await CapabilityEntityBuilder.Create().WithName("Cap2").WithId(_cap2Id).WithCapabilityRef(CapabilityReference2).WithDescription("Cap2Desc").Build().InsertAsync();

            TestContext testContext = new TestContext();
            _solutionListRepository = testContext.SolutionListRepository;
        }

        [Test]
        public async Task ShouldListSingleSolution()
        {
            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(Solution1Id)
                .WithName("Solution1")
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId(3)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(Solution1Id)
                .Build()
                .InsertAsync();

            var solutions = await _solutionListRepository.ListAsync(false, null, new CancellationToken());

            solutions.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldListSingleSolutionWithSingleCapability()
        {
            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(Solution1Id)
                .WithName("Solution1")
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId(3)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(Solution1Id)
                .WithSummary("Sln1Summary")
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithCapabilityId(_cap1Id)
                .Build()
                .InsertAsync();

            var solutions = await _solutionListRepository.ListAsync(false, null, new CancellationToken());

            var solution = solutions.Should().ContainSingle().Subject;
            solution.SolutionId.Should().Be(Solution1Id);
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.SupplierId.Should().Be(_supplierId);
            solution.SupplierName.Should().Be(_supplierName);
            solution.CapabilityReference.Should().Be(CapabilityReference1);
            solution.CapabilityName.Should().Be("Cap1");
            solution.CapabilityDescription.Should().Be("Cap1Desc");
            solution.IsFoundation.Should().BeFalse();
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldListSingleSolutionAsFoundation(bool isFoundation)
        {
            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(Solution1Id)
                .WithName("Solution1")
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId(3)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(Solution1Id)
                .WithSummary("Sln1Summary")
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithCapabilityId(_cap1Id)
                .Build()
                .InsertAsync();

            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithFoundation(isFoundation)
                .Build().InsertAsync();

            var solutions = await _solutionListRepository.ListAsync(false, null, new CancellationToken());

            var solution = solutions.Should().ContainSingle().Subject;
            solution.SolutionId.Should().Be(Solution1Id);
            solution.IsFoundation.Should().Be(isFoundation);
        }

        [Test]
        public async Task ShouldListSingleSolutionWithMultipleCapability()
        {
            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(Solution1Id)
                .WithName("Solution1")
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId(3)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(Solution1Id)
                .WithSummary("Sln1Summary")
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithCapabilityId(_cap1Id)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithCapabilityId(_cap2Id)
                .Build()
                .InsertAsync();

            var solutions =
                (await _solutionListRepository.ListAsync(false, null, new CancellationToken())).ToList();
            solutions.Should().HaveCount(2);

            var solution = solutions.Should().ContainSingle(s => s.CapabilityReference == CapabilityReference1).Subject;
            solution.SolutionId.Should().Be(Solution1Id);
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.SupplierId.Should().Be(_supplierId);
            solution.SupplierName.Should().Be(_supplierName);
            solution.CapabilityReference.Should().Be(CapabilityReference1);
            solution.CapabilityName.Should().Be("Cap1");
            solution.CapabilityDescription.Should().Be("Cap1Desc");

            solution = solutions.Should().ContainSingle(s => s.CapabilityReference == CapabilityReference2).Subject;
            solution.SolutionId.Should().Be(Solution1Id);
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.SupplierId.Should().Be(_supplierId);
            solution.SupplierName.Should().Be(_supplierName);
            solution.CapabilityReference.Should().Be(CapabilityReference2);
            solution.CapabilityName.Should().Be("Cap2");
            solution.CapabilityDescription.Should().Be("Cap2Desc");
        }

        [Test]
        public async Task ShouldListMultipleSolutionsWithCapabilities()
        {
            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(Solution1Id)
                .WithName("Solution1")
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId(3)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(Solution1Id)
                .WithSummary("Sln1Summary")
                .Build()
                .InsertAsync();

            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId("Sln2")
                .WithName("Solution2")
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId(3)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId("Sln2")
                .WithSummary("Sln2Summary")
                .Build()
                .InsertAsync();

            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId("Sln3")
                .WithName("Solution3")
                .WithSupplierId(_supplierId)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId("Sln3")
                .WithSummary("Sln3Summary")
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithCapabilityId(_cap1Id)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithCapabilityId(_cap2Id)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId("Sln2")
                .WithCapabilityId(_cap2Id)
                .Build()
                .InsertAsync();

            var solutions =
                (await _solutionListRepository.ListAsync(false, null, new CancellationToken())).ToList();

            solutions.Should().HaveCount(3);

            var solution = solutions.Should().ContainSingle(s => s.SolutionId == Solution1Id && s.CapabilityReference == CapabilityReference1).Subject;
            solution.SolutionId.Should().Be(Solution1Id);
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.SupplierId.Should().Be(_supplierId);
            solution.SupplierName.Should().Be(_supplierName);
            solution.CapabilityReference.Should().Be(CapabilityReference1);
            solution.CapabilityName.Should().Be("Cap1");
            solution.CapabilityDescription.Should().Be("Cap1Desc");

            solution = solutions.Should().ContainSingle(s => s.SolutionId == Solution1Id && s.CapabilityReference == CapabilityReference2).Subject;
            solution.SolutionId.Should().Be(Solution1Id);
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.SupplierId.Should().Be(_supplierId);
            solution.SupplierName.Should().Be(_supplierName);
            solution.CapabilityReference.Should().Be(CapabilityReference2);
            solution.CapabilityName.Should().Be("Cap2");
            solution.CapabilityDescription.Should().Be("Cap2Desc");

            solution = solutions.Should().ContainSingle(s => s.SolutionId == Solution2Id && s.CapabilityReference == CapabilityReference2).Subject;
            solution.SolutionId.Should().Be(Solution2Id);
            solution.SolutionName.Should().Be("Solution2");
            solution.SolutionSummary.Should().Be("Sln2Summary");
            solution.SupplierId.Should().Be(_supplierId);
            solution.SupplierName.Should().Be(_supplierName);
            solution.CapabilityReference.Should().Be(CapabilityReference2);
            solution.CapabilityName.Should().Be("Cap2");
            solution.CapabilityDescription.Should().Be("Cap2Desc");
        }

        [Test]
        public async Task ShouldFilterByFoundation()
        {
            await CreateSimpleSolutionWithOneCap(Solution1Id);
            await CreateSimpleSolutionWithOneCap(Solution2Id);
            await CreateSimpleSolutionWithOneCap(Solution3Id);
            await CreateSimpleSolutionWithOneCap(Solution4Id);

            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithFoundation(true)
                .Build()
                .InsertAsync();

            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId(Solution2Id)
                .WithFoundation(false)
                .Build()
                .InsertAsync();

            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId(Solution4Id)
                .WithFoundation(true)
                .Build()
                .InsertAsync();

            var solutions = await _solutionListRepository.ListAsync(true, null, new CancellationToken());

            solutions.Should().HaveCount(2);
            var solution = solutions.Should().ContainSingle(s => s.SolutionId == Solution1Id).Subject;
            solution.IsFoundation.Should().Be(true);
            solution = solutions.Should().ContainSingle(s => s.SolutionId == Solution4Id).Subject;
            solution.IsFoundation.Should().Be(true);
        }

        [Test]
        public async Task ListAsync_FilterBySupplierId_ReturnsFilteredResult()
        {
            const string supId2 = "Sup2";

            await SupplierEntityBuilder.Create()
                .WithId(supId2)
                .Build()
                .InsertAsync();

            await CreateSimpleSolutionWithOneCap(Solution1Id);
            await CreateSimpleSolutionWithOneCap(Solution2Id, supId2);
            await CreateSimpleSolutionWithOneCap(Solution3Id);
            await CreateSimpleSolutionWithOneCap(Solution4Id, supId2);

            var solutions = await _solutionListRepository.ListAsync(false, _supplierId, new CancellationToken());

            solutions.Count().Should().Be(2);
            solutions.Should().Contain(x => x.SupplierId == _supplierId);
        }

        [Test]
        public async Task ListAsync_FilterByInvalidSupplierId_ReturnsEmptyList()
        {
            await CreateSimpleSolutionWithOneCap(Solution1Id);

            var solutions = await _solutionListRepository.ListAsync(false, "INVALID", new CancellationToken());

            solutions.Should().BeEmpty();
        }

        private async Task CreateSimpleSolutionWithOneCap(string solutionId, string supplierId = _supplierId)
        {
            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(solutionId)
                .WithName(solutionId)
                .WithSupplierId(supplierId)
                .WithPublishedStatusId(3)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(solutionId)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId(solutionId)
                .WithCapabilityId(_cap1Id)
                .Build()
                .InsertAsync();
        }
    }
}
