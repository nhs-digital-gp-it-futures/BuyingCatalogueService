using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.SolutionLists.API.ViewModels;
using NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.UnitTests
{
    [TestFixture]
    public class SolutionListTests
    {
        private TestContext _context;

        private readonly Dictionary<string, (string name, string id)> Suppliers = new Dictionary<string, (string name, string id)>
        {
            { "Sup1", (name: "Supplier 1 Name", id: "Sup1")},
            { "Sup2", (name: "Supplier 2 Name", id: "Sup2")},
            { "Sup3", (name: "Supplier 3 Name", id: "Sup3")},
        };

        private readonly Dictionary<int, (string Reference, string Name, string Description)> Capabilities = new Dictionary<int, (string Reference, string Name, string Description)>
        {
            { 1, ("C1", "Cap1Name", "Cap1Desc")},
            { 2, ("C2", "Cap2Name", "Cap2Desc")},
            { 3, ("C3", "Cap3Name", "Cap3Desc")},
            { 4, ("C4", "Cap4Name", "Cap4Desc")},
            { 5, ("C5", "Cap5Name", "Cap5Desc")},
        };

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
        }

        [Test]
        public void ShouldListSolutionsWithNoCapabilityFilter()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false,  1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup1", false, 2, 3));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(new ListSolutionsFilterViewModel()), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithNoCapabilityFilterReturnDistinctBySupplier()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup2", false, 1, 2));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(new ListSolutionsFilterViewModel()), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithNoCapabilityFilterReturnDistinctBySupplierAndCapability()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, 1, 2));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(new ListSolutionsFilterViewModel()), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(1);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
        }

        [Test]
        public void ShouldListSolutionsWithNoCapabilityFilterReturnDistinctByCapability()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup1", false, 2, 3));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(new ListSolutionsFilterViewModel()), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithNoCapabilityFilterReturnDistinctBySupplierAndCapabilityTwoSuppliers()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup1", false, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S3", "Sup2", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S4", "Sup2", false, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S4", "Sup2", false, 2, 3));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(new ListSolutionsFilterViewModel()), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(4);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S3");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S4");
        }

        [Test]
        public void ShouldListSolutionsEmptyWithNoCapabilityFilter()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(new ListSolutionsFilterViewModel()), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(0);
        }

        [Test]
        public void ShouldListSolutionsWithCapabilityFilter()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup1", false, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup1", false, 3, 4));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter(2)), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithCapabilityFilterReturnDistinctBySupplier()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, 4, 5));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup2", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup2", false, 3, 4));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter(1)), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithCapabilityFilterReturnDistinctBySupplierAndCapability()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, 3, 4));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter(1)), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(1);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
        }

        [Test]
        public void ShouldListSolutionsWithCapabilityFilterReturnDistinctByCapability()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup1", false, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S3", "Sup1", false, 3, 4));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter(2)), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithCapabilityFilterReturnDistinctBySupplierAndCapabilityTwoSuppliers()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup1", false, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S3", "Sup2", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S4", "Sup2", false, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S4", "Sup2", false, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S6", "Sup2", false, 4, 3));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter(2)), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(4);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S3");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S4");
        }

        [Test]
        public void ShouldListSolutionsEmptyWithCapabilityFilter()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter(2)), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(0);
        }

        [TestCase(new[] { 1, 2 }, new []{ "S123", "S124" })]
        [TestCase(new[] { 1 }, new[] { "S123", "S124", "S1", "S145", "S15", "S13" })]
        [TestCase(new[] { 1, 2, 3 }, new[] { "S123" })]
        [TestCase(new[] { 3, 4 }, new string[0])]
        [TestCase(new[] { 1, 5 }, new[] { "S145", "S15" })]
        [TestCase(new[] { 1, 4 }, new[] { "S124", "S145" })]
        public void ShouldFilterByCapability(int[] filterCapabilityIds, string[] expectedFilteredSolutions)
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S123", "Sup1", false, 1, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S124", "Sup1", false, 1, 2, 4));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, 1));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S145", "Sup1", false, 1, 4, 5));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S15", "Sup1", false, 1, 5));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S13", "Sup1", false, 1, 3));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter(filterCapabilityIds.ThrowIfNull())), new CancellationToken());

            solutions.Result.Solutions.Select(s => s.Id).Should().BeEquivalentTo(expectedFilteredSolutions);
        }

        [TestCase(new[] { 1, 2 }, new[] { "S123", "S124" })]
        [TestCase(new[] { 1 }, new[] { "S123", "S124", "S1", "S145", "S15", "S13" })]
        [TestCase(new[] { 1, 2, 3 }, new[] { "S123" })]
        [TestCase(new[] { 3, 4 }, new string[0])]
        [TestCase(new[] { 1, 5 }, new[] { "S145", "S15" })]
        [TestCase(new[] { 1, 4 }, new[] { "S124", "S145" })]
        public void ShouldFilterByCapabilityMultipleOrgs(int[] filterCapabilityIds, string[] expectedFilteredSolutions)
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S123", "Sup1", false, 1, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S124", "Sup2", false, 1, 2, 4));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, 1));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S145", "Sup2", false, 1, 4, 5));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S15", "Sup1", false, 1, 5));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S13", "Sup2", false, 1, 3));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter(filterCapabilityIds.ThrowIfNull())), new CancellationToken());

            solutions.Result.Solutions.Select(s => s.Id).Should().BeEquivalentTo(expectedFilteredSolutions);
        }

        [Test]
        public void ShouldListSolutionsDetail()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup2", true, 2));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(new ListSolutionsFilterViewModel()), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(2);
            var solution = solutions.Result.Solutions.Should().ContainSingle(s => string.Equals(s.Id, "S1", StringComparison.InvariantCulture)).Subject;
            solution.Name.Should().Be("S1Name");
            solution.Summary.Should().Be("S1Summary");
            solution.IsFoundation.Should().BeFalse();
            solution.Supplier.Id.Should().Be(Suppliers["Sup1"].id);
            solution.Supplier.Name.Should().Be(Suppliers["Sup1"].name);
            solution.Capabilities.Should().HaveCount(2);
            solution.Capabilities.Single(c => c.CapabilityReference.Equals(Capabilities[1].Reference, StringComparison.InvariantCulture));

            var capability  = solution.Capabilities.Should().ContainSingle(c => c.CapabilityReference == Capabilities[1].Reference).Subject;
            capability.Name.Should().Be(Capabilities[1].Name);
            capability = solution.Capabilities.Should().ContainSingle(c => c.CapabilityReference == Capabilities[2].Reference).Subject;
            capability.Name.Should().Be(Capabilities[2].Name);

            solution = solutions.Result.Solutions.Should().ContainSingle(s => string.Equals(s.Id, "S2", StringComparison.InvariantCulture)).Subject;
            solution.Name.Should().Be("S2Name");
            solution.Summary.Should().Be("S2Summary");
            solution.IsFoundation.Should().BeTrue();
            solution.Supplier.Id.Should().Be(Suppliers["Sup2"].id);
            solution.Supplier.Name.Should().Be(Suppliers["Sup2"].name);
            solution.Capabilities.Should().HaveCount(1);
            solution.Capabilities.Single(c => c.CapabilityReference.Equals(Capabilities[2].Reference, StringComparison.InvariantCulture));

            capability = solution.Capabilities.Should().ContainSingle(c => c.CapabilityReference == Capabilities[2].Reference).Subject;
            capability.Name.Should().Be(Capabilities[2].Name);
        }

        [Test]
        public void ShouldRequestFoundationSolutions()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(true, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(new ListSolutionsFilterViewModel{IsFoundation = true}), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(0);
            _context.MockSolutionListRepository.Verify(r => r.ListAsync(true, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void QueryShouldThrowNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ListSolutionsQuery(null));
        }

        private ListSolutionsFilterViewModel Filter(int capabilityId)
        {
            return Filter(new int[] {capabilityId});
        }

        private ListSolutionsFilterViewModel Filter(IEnumerable<int> capabilityIds)
        {
            var filter = new ListSolutionsFilterViewModel();
            foreach (var capabilityId in capabilityIds)
            {
                filter.Capabilities.Add(new CapabilityReferenceViewModel {Reference = Capabilities[capabilityId].Reference});
            }

            return filter;
        }

        private IEnumerable<ISolutionListResult> GetSolutionWithCapabilities(string solnId, string supplierId, bool isFoundation, params int[] capabilityIds)
        {
            return capabilityIds.Select(capabilityId => GetSolution(solnId, supplierId, capabilityId, isFoundation));
        }

        private ISolutionListResult GetSolution(string solnId, string supplierId, int capabilityId, bool isFoundation)
        {
            var capability = Capabilities[capabilityId];
            var solution = new Mock<ISolutionListResult>();
            solution.Setup(c => c.SolutionId).Returns($"{solnId}");
            solution.Setup(c => c.SolutionName).Returns($"{solnId}Name");
            solution.Setup(c => c.SolutionSummary).Returns($"{solnId}Summary");
            solution.Setup(c => c.IsFoundation).Returns(isFoundation);

            solution.Setup(c => c.SupplierId).Returns(Suppliers[supplierId].id);
            solution.Setup(c => c.SupplierName).Returns(Suppliers[supplierId].name);

            solution.Setup(c => c.CapabilityReference).Returns(capability.Reference);
            solution.Setup(c => c.CapabilityName).Returns(capability.Name);
            solution.Setup(c => c.CapabilityDescription).Returns(capability.Description);
            return solution.Object;
        }
    }
}
