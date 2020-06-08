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
            await Database.ClearAsync().ConfigureAwait(false);

            await SupplierEntityBuilder.Create()
                .WithId(_supplierId)
                .WithName(_supplierName)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await CapabilityEntityBuilder.Create().WithName("Cap1").WithId(_cap1Id).WithCapabilityRef(CapabilityReference1).WithDescription("Cap1Desc").Build().InsertAsync().ConfigureAwait(false);
            await CapabilityEntityBuilder.Create().WithName("Cap2").WithId(_cap2Id).WithCapabilityRef(CapabilityReference2).WithDescription("Cap2Desc").Build().InsertAsync().ConfigureAwait(false);
            
            TestContext testContext = new TestContext();
            _solutionListRepository = testContext.SolutionListRepository;
        }

        [Test]
        public async Task ShouldListSingleSolution()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId(Solution1Id)
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId(3)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            var solutions = await _solutionListRepository.ListAsync(false, null, new CancellationToken()).ConfigureAwait(false);

            solutions.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldListSingleSolutionWithSingleCapability()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId(Solution1Id)
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId(3)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithSummary("Sln1Summary")
                .Build()
                .InsertAndSetCurrentForSolutionAsync().ConfigureAwait(false);

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithCapabilityId(_cap1Id)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            var solutions = await _solutionListRepository.ListAsync(false, null, new CancellationToken()).ConfigureAwait(false);

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
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId(Solution1Id)
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId(3)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithSummary("Sln1Summary")
                .Build()
                .InsertAndSetCurrentForSolutionAsync().ConfigureAwait(false);

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithCapabilityId(_cap1Id)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithFoundation(isFoundation)
                .Build().InsertAsync().ConfigureAwait(false);

            var solutions = await _solutionListRepository.ListAsync(false, null, new CancellationToken()).ConfigureAwait(false);

            var solution = solutions.Should().ContainSingle().Subject;
            solution.SolutionId.Should().Be(Solution1Id);
            solution.IsFoundation.Should().Be(isFoundation);
        }

        [Test]
        public async Task ShouldListSingleSolutionWithMultipleCapability()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId(Solution1Id)
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId(3)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithSummary("Sln1Summary")
                .Build()
                .InsertAndSetCurrentForSolutionAsync().ConfigureAwait(false);

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithCapabilityId(_cap1Id)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithCapabilityId(_cap2Id)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            var solutions =
                (await _solutionListRepository.ListAsync(false, null, new CancellationToken()).ConfigureAwait(false)).ToList();
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
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId(Solution1Id)
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId(3)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await SolutionEntityBuilder.Create()
                .WithName("Solution2")
                .WithId("Sln2")
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId(3)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await SolutionEntityBuilder.Create()
                .WithName("Solution3")
                .WithId("Sln3")
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId(1)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithSummary("Sln1Summary")
                .Build()
                .InsertAndSetCurrentForSolutionAsync().ConfigureAwait(false);

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId("Sln2")
                .WithSummary("Sln2Summary")
                .Build()
                .InsertAndSetCurrentForSolutionAsync().ConfigureAwait(false);

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId("Sln3")
                .WithSummary("Sln3Summary")
                .Build()
                .InsertAndSetCurrentForSolutionAsync().ConfigureAwait(false);

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithCapabilityId(_cap1Id)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithCapabilityId(_cap2Id)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId("Sln2")
                .WithCapabilityId(_cap2Id)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            var solutions =
                (await _solutionListRepository.ListAsync(false, null, new CancellationToken()).ConfigureAwait(false)).ToList();

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
            await CreateSimpleSolutionWithOneCap(Solution1Id).ConfigureAwait(false);
            await CreateSimpleSolutionWithOneCap(Solution2Id).ConfigureAwait(false);
            await CreateSimpleSolutionWithOneCap(Solution3Id).ConfigureAwait(false);
            await CreateSimpleSolutionWithOneCap(Solution4Id).ConfigureAwait(false);

            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId(Solution1Id)
                .WithFoundation(true)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId(Solution2Id)
                .WithFoundation(false)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId(Solution4Id)
                .WithFoundation(true)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            var solutions = await _solutionListRepository.ListAsync(true, null, new CancellationToken()).ConfigureAwait(false);

            solutions.Should().HaveCount(2);
            var solution = solutions.Should().ContainSingle(s => s.SolutionId == Solution1Id).Subject;
            solution.IsFoundation.Should().Be(true);
            solution = solutions.Should().ContainSingle(s => s.SolutionId == Solution4Id).Subject;
            solution.IsFoundation.Should().Be(true);
        }

        [Test]
        public async Task ListAsync_FilterBySupplierId_ReturnsFilteredResult()
        {
            var supId2 = "Sup2";

            await SupplierEntityBuilder.Create()
                .WithId(supId2)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await CreateSimpleSolutionWithOneCap(Solution1Id).ConfigureAwait(false);
            await CreateSimpleSolutionWithOneCap(Solution2Id, supId2).ConfigureAwait(false);
            await CreateSimpleSolutionWithOneCap(Solution3Id).ConfigureAwait(false);
            await CreateSimpleSolutionWithOneCap(Solution4Id, supId2).ConfigureAwait(false);

            var solutions = await _solutionListRepository.ListAsync(false, _supplierId, new CancellationToken()).ConfigureAwait(false);

            solutions.Count().Should().Be(2);
            solutions.Should().Contain(x => x.SupplierId == _supplierId);
        }
        
        [Test]
        public async Task ListAsync_FilterByInvalidSupplierId_ReturnsEmptyList()
        {
            await CreateSimpleSolutionWithOneCap(Solution1Id).ConfigureAwait(false);

            var solutions = await _solutionListRepository.ListAsync(false, "INVALID", new CancellationToken()).ConfigureAwait(false);

            solutions.Should().BeEmpty();
        }

        private async Task CreateSimpleSolutionWithOneCap(string solutionId, string supplierId = _supplierId)
        {
            await SolutionEntityBuilder.Create()
                .WithName(solutionId)
                .WithId(solutionId)
                .WithSupplierId(supplierId)
                .WithPublishedStatusId(3)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await SolutionDetailEntityBuilder.Create()
                .WithSolutionId(solutionId)
                .Build()
                .InsertAndSetCurrentForSolutionAsync().ConfigureAwait(false);

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId(solutionId)
                .WithCapabilityId(_cap1Id)
                .Build()
                .InsertAsync().ConfigureAwait(false);
        }
    }
}
