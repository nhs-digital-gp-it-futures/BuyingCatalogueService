using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;
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

        private readonly Dictionary<string, (string Name, string Description)> Capabilities = new Dictionary<string, (string Name, string Description)>
        {
            { "C1", ("Cap1Name", "Cap1Desc")},
            { "C2", ("Cap2Name", "Cap2Desc")},
            { "C3", ("Cap3Name", "Cap3Desc")},
            { "C4", ("Cap4Name", "Cap4Desc")},
            { "C5", ("Cap5Name", "Cap5Desc")},
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
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false,  "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup1", false, "C2", "C3"));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = GetSolutionsWithEmptyCapabilityReferencesFilterAsync();

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithSupplierIdFilter()
        {
            var supplierId = "Sup1";

            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", supplierId, false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", supplierId, false, "C2", "C3"));
            _context.MockSolutionListRepository
                .Setup(r => r.ListAsync(false, supplierId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositorySolutions);

            var solutions = GetSolutionsWithEmptyCapabilityReferencesFilterAsync(false, supplierId);

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithNoCapabilityFilterReturnDistinctBySupplier()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup2", false, "C1", "C2"));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = GetSolutionsWithEmptyCapabilityReferencesFilterAsync();

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithNoCapabilityFilterReturnDistinctBySupplierAndCapability()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C1", "C2"));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = GetSolutionsWithEmptyCapabilityReferencesFilterAsync();

            solutions.Result.Solutions.Should().HaveCount(1);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
        }

        [Test]
        public void ShouldListSolutionsWithNoCapabilityFilterReturnDistinctByCapability()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup1", false, "C2", "C3"));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = GetSolutionsWithEmptyCapabilityReferencesFilterAsync();

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithNoCapabilityFilterReturnDistinctBySupplierAndCapabilityTwoSuppliers()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup1", false, "C2", "C3"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S3", "Sup2", false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S4", "Sup2", false, "C2", "C3"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S4", "Sup2", false, "C2", "C3"));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = GetSolutionsWithEmptyCapabilityReferencesFilterAsync();

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
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = GetSolutionsWithEmptyCapabilityReferencesFilterAsync();

            solutions.Result.Solutions.Should().HaveCount(0);
        }

        [Test]
        public void ShouldListSolutionsWithCapabilityFilter()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup1", false, "C2", "C3"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup1", false, "C3", "C4"));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter("C2")), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithCapabilityFilterReturnDistinctBySupplier()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C4", "C5"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup2", false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup2", false, "C3", "C4"));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter("C1")), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithCapabilityFilterReturnDistinctBySupplierAndCapability()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C3", "C4"));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter("C1")), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(1);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
        }

        [Test]
        public void ShouldListSolutionsWithCapabilityFilterReturnDistinctByCapability()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup1", false, "C2", "C3"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S3", "Sup1", false, "C3", "C4"));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter("C2")), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithCapabilityFilterReturnDistinctBySupplierAndCapabilityTwoSuppliers()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup1", false, "C2", "C3"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S3", "Sup2", false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S4", "Sup2", false, "C2", "C3"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S4", "Sup2", false, "C2", "C3"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S6", "Sup2", false, "C4", "C3"));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter("C2")), new CancellationToken());

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
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter("C2")), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(0);
        }

        [TestCase(new[] { "C1", "C2" }, new []{ "S123", "S124" })]
        [TestCase(new[] { "C1" }, new[] { "S123", "S124", "S1", "S145", "S15", "S13" })]
        [TestCase(new[] { "C1", "C2", "C3" }, new[] { "S123" })]
        [TestCase(new[] { "C3", "C4" }, new string[0])]
        [TestCase(new[] { "C1", "C5" }, new[] { "S145", "S15" })]
        [TestCase(new[] { "C1", "C4" }, new[] { "S124", "S145" })]
        public void ShouldFilterByCapability(string[] filterCapabilityReferences, string[] expectedFilteredSolutions)
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S123", "Sup1", false, "C1", "C2", "C3"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S124", "Sup1", false, "C1", "C2", "C4"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C1"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S145", "Sup1", false, "C1", "C4", "C5"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S15", "Sup1", false, "C1", "C5"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S13", "Sup1", false, "C1", "C3"));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            if (filterCapabilityReferences is null)
            {
                throw new ArgumentNullException(nameof(filterCapabilityReferences));
            }

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter(filterCapabilityReferences)), new CancellationToken());

            solutions.Result.Solutions.Select(s => s.Id).Should().BeEquivalentTo(expectedFilteredSolutions);
        }

        [TestCase(new[] { "C1", "C2" }, new[] { "S123", "S124" })]
        [TestCase(new[] { "C1" }, new[] { "S123", "S124", "S1", "S145", "S15", "S13" })]
        [TestCase(new[] { "C1", "C2", "C3" }, new[] { "S123" })]
        [TestCase(new[] { "C3", "C4" }, new string[0])]
        [TestCase(new[] { "C1", "C5" }, new[] { "S145", "S15" })]
        [TestCase(new[] { "C1", "C4" }, new[] { "S124", "S145" })]
        public void ShouldFilterByCapabilityMultipleOrgs(string[] filterCapabilityReferences, string[] expectedFilteredSolutions)
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S123", "Sup1", false, "C1", "C2", "C3"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S124", "Sup2", false, "C1", "C2", "C4"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C1"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S145", "Sup2", false, "C1", "C4", "C5"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S15", "Sup1", false, "C1", "C5"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S13", "Sup2", false, "C1", "C3"));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            if (filterCapabilityReferences is null)
            {
                throw new ArgumentNullException(nameof(filterCapabilityReferences));
            }

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter(filterCapabilityReferences)), new CancellationToken());

            solutions.Result.Solutions.Select(s => s.Id).Should().BeEquivalentTo(expectedFilteredSolutions);
        }

        [Test]
        public void ShouldListSolutionsDetail()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup2", true, "C2"));
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = GetSolutionsWithEmptyCapabilityReferencesFilterAsync();

            solutions.Result.Solutions.Should().HaveCount(2);
            var solution = solutions.Result.Solutions.Should().ContainSingle(s => string.Equals(s.Id, "S1", StringComparison.OrdinalIgnoreCase)).Subject;
            solution.Name.Should().Be("S1Name");
            solution.Summary.Should().Be("S1Summary");
            solution.IsFoundation.Should().BeFalse();
            solution.Supplier.Id.Should().Be(Suppliers["Sup1"].id);
            solution.Supplier.Name.Should().Be(Suppliers["Sup1"].name);
            solution.Capabilities.Should().HaveCount(2);

            var capability = solution.Capabilities.Should().ContainSingle(c => string.Equals(c.CapabilityReference, "C1", StringComparison.OrdinalIgnoreCase)).Subject;
            capability.Name.Should().Be(Capabilities["C1"].Name); 
            capability = solution.Capabilities.Should().ContainSingle(c => string.Equals(c.CapabilityReference, "C2", StringComparison.OrdinalIgnoreCase)).Subject;
            capability.Name.Should().Be(Capabilities["C2"].Name);

            solution = solutions.Result.Solutions.Should().ContainSingle(s => string.Equals(s.Id, "S2", StringComparison.OrdinalIgnoreCase)).Subject;
            solution.Name.Should().Be("S2Name");
            solution.Summary.Should().Be("S2Summary");
            solution.IsFoundation.Should().BeTrue();
            solution.Supplier.Id.Should().Be(Suppliers["Sup2"].id);
            solution.Supplier.Name.Should().Be(Suppliers["Sup2"].name);
            solution.Capabilities.Should().HaveCount(1);
            solution.Capabilities.Single(c => string.Equals(c.CapabilityReference, "C2", StringComparison.OrdinalIgnoreCase));

            capability = solution.Capabilities.Should().ContainSingle(c => string.Equals(c.CapabilityReference, "C2", StringComparison.OrdinalIgnoreCase)).Subject;
            capability.Name.Should().Be(Capabilities["C2"].Name);
        }

        [Test]
        public void ShouldRequestFoundationSolutions()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            _context.MockSolutionListRepository.Setup(r => r.ListAsync(true, null, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = GetSolutionsWithEmptyCapabilityReferencesFilterAsync(true);

            solutions.Result.Solutions.Should().HaveCount(0);
            _context.MockSolutionListRepository.Verify(r => r.ListAsync(true, null, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void QueryShouldThrowNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ListSolutionsQuery(null));
        }

        private async Task <ISolutionList> GetSolutionsWithEmptyCapabilityReferencesFilterAsync(bool isFoundation = false, string supplierId = null)
        {
            return await _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Mock.Of<IListSolutionsQueryData>(
                        data => data.CapabilityReferences == new HashSet<ICapabilityReference>() && 
                                data.IsFoundation == isFoundation &&
                                data.SupplierId == supplierId)), new CancellationToken())
                .ConfigureAwait(false);
        }

        private static IListSolutionsQueryData Filter(string capabilityReference)
        {
            return Filter(new string[] {capabilityReference});
        }

        private static IListSolutionsQueryData Filter(IEnumerable<string> capabilityReferences)
        {
            var filter = Mock.Of<IListSolutionsQueryData>(
                data => data.CapabilityReferences == new List<ICapabilityReference>(
                            capabilityReferences.Select(cr => Mock.Of<ICapabilityReference>(c => c.Reference == cr)).ToList()));
            return filter;
        }

        private IEnumerable<ISolutionListResult> GetSolutionWithCapabilities(string solnId, string supplierId, bool isFoundation, params string[] capabilityReferences)
        {
            return capabilityReferences.Select(capabilityReference => GetSolution(solnId, supplierId, capabilityReference, isFoundation));
        }

        private ISolutionListResult GetSolution(string solnId, string supplierId, string capabilityReference, bool isFoundation)
        {
            (string capabilityName, string capabilityDescription) = Capabilities[capabilityReference];
            var solution = new Mock<ISolutionListResult>();
            solution.Setup(c => c.SolutionId).Returns($"{solnId}");
            solution.Setup(c => c.SolutionName).Returns($"{solnId}Name");
            solution.Setup(c => c.SolutionSummary).Returns($"{solnId}Summary");
            solution.Setup(c => c.IsFoundation).Returns(isFoundation);

            solution.Setup(c => c.SupplierId).Returns(Suppliers[supplierId].id);
            solution.Setup(c => c.SupplierName).Returns(Suppliers[supplierId].name);

            solution.Setup(c => c.CapabilityReference).Returns(capabilityReference);
            solution.Setup(c => c.CapabilityName).Returns(capabilityName);
            solution.Setup(c => c.CapabilityDescription).Returns(capabilityDescription);
            return solution.Object;
        }
    }
}
