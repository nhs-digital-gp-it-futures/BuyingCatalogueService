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

        private readonly Guid _org1Id = Guid.NewGuid();
        private readonly string _orgName = "Org1";

        private readonly string _supplierId = "Sup 1";

        private ISolutionListRepository _solutionListRepository;

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync().ConfigureAwait(false);

            await OrganisationEntityBuilder.Create()
                .WithName(_orgName)
                .WithId(_org1Id)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await SupplierEntityBuilder.Create()
                .WithId(_supplierId)
                .WithOrganisation(_org1Id)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await CapabilityEntityBuilder.Create().WithName("Cap1").WithId(_cap1Id).WithDescription("Cap1Desc").Build().InsertAsync().ConfigureAwait(false);
            await CapabilityEntityBuilder.Create().WithName("Cap2").WithId(_cap2Id).WithDescription("Cap2Desc").Build().InsertAsync().ConfigureAwait(false);
            
            TestContext testContext = new TestContext();
            _solutionListRepository = testContext.SolutionListRepository;
        }

        [Test]
        public async Task ShouldListSingleSolution()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId(Solution1Id)
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId(3)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            var solutions = await _solutionListRepository.ListAsync(false, new CancellationToken()).ConfigureAwait(false);

            solutions.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldListSingleSolutionWithSingleCapability()
        {
            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId(Solution1Id)
                .WithOrganisationId(_org1Id)
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

            var solutions = await _solutionListRepository.ListAsync(false, new CancellationToken()).ConfigureAwait(false);

            var solution = solutions.Should().ContainSingle().Subject;
            solution.SolutionId.Should().Be(Solution1Id);
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
                .WithId(Solution1Id)
                .WithOrganisationId(_org1Id)
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

            var solutions = await _solutionListRepository.ListAsync(false, new CancellationToken()).ConfigureAwait(false);

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
                .WithOrganisationId(_org1Id)
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
                (await _solutionListRepository.ListAsync(false, new CancellationToken()).ConfigureAwait(false)).ToList();
            solutions.Should().HaveCount(2);

            var solution = solutions.Should().ContainSingle(s => s.CapabilityId == _cap1Id).Subject;
            solution.SolutionId.Should().Be(Solution1Id);
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.OrganisationId.Should().Be(_org1Id);
            solution.OrganisationName.Should().Be(_orgName);
            solution.CapabilityId.Should().Be(_cap1Id);
            solution.CapabilityName.Should().Be("Cap1");
            solution.CapabilityDescription.Should().Be("Cap1Desc");

            solution = solutions.Should().ContainSingle(s => s.CapabilityId == _cap2Id).Subject;
            solution.SolutionId.Should().Be(Solution1Id);
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
                .WithId(Solution1Id)
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId(3)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await SolutionEntityBuilder.Create()
                .WithName("Solution2")
                .WithId("Sln2")
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
                .WithPublishedStatusId(3)
                .Build()
                .InsertAsync().ConfigureAwait(false);

            await SolutionEntityBuilder.Create()
                .WithName("Solution3")
                .WithId("Sln3")
                .WithOrganisationId(_org1Id)
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
                (await _solutionListRepository.ListAsync(false, new CancellationToken()).ConfigureAwait(false)).ToList();

            solutions.Should().HaveCount(3);

            var solution = solutions.Should().ContainSingle(s => s.SolutionId == Solution1Id && s.CapabilityId == _cap1Id).Subject;
            solution.SolutionId.Should().Be(Solution1Id);
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.OrganisationId.Should().Be(_org1Id);
            solution.OrganisationName.Should().Be(_orgName);
            solution.CapabilityId.Should().Be(_cap1Id);
            solution.CapabilityName.Should().Be("Cap1");
            solution.CapabilityDescription.Should().Be("Cap1Desc");

            solution = solutions.Should().ContainSingle(s => s.SolutionId == Solution1Id && s.CapabilityId == _cap2Id).Subject;
            solution.SolutionId.Should().Be(Solution1Id);
            solution.SolutionName.Should().Be("Solution1");
            solution.SolutionSummary.Should().Be("Sln1Summary");
            solution.OrganisationId.Should().Be(_org1Id);
            solution.OrganisationName.Should().Be(_orgName);
            solution.CapabilityId.Should().Be(_cap2Id);
            solution.CapabilityName.Should().Be("Cap2");
            solution.CapabilityDescription.Should().Be("Cap2Desc");

            solution = solutions.Should().ContainSingle(s => s.SolutionId == Solution2Id && s.CapabilityId == _cap2Id).Subject;
            solution.SolutionId.Should().Be(Solution2Id);
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

            var solutions = await _solutionListRepository.ListAsync(true, new CancellationToken()).ConfigureAwait(false);

            solutions.Should().HaveCount(2);
            var solution = solutions.Should().ContainSingle(s => s.SolutionId == Solution1Id).Subject;
            solution.IsFoundation.Should().Be(true);
            solution = solutions.Should().ContainSingle(s => s.SolutionId == Solution4Id).Subject;
            solution.IsFoundation.Should().Be(true);
        }

        private async Task CreateSimpleSolutionWithOneCap(string solutionId)
        {
            await SolutionEntityBuilder.Create()
                .WithName(solutionId)
                .WithId(solutionId)
                .WithOrganisationId(_org1Id)
                .WithSupplierId(_supplierId)
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
