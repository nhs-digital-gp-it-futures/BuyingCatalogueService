using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    internal sealed class SolutionListTests
    {
        private readonly Dictionary<string, (string Name, string Id)> suppliers = new()
        {
            { "Sup1", (Name: "Supplier 1 Name", Id: "Sup1") },
            { "Sup2", (Name: "Supplier 2 Name", Id: "Sup2") },
            { "Sup3", (Name: "Supplier 3 Name", Id: "Sup3") },
        };

        private readonly Dictionary<string, (string Name, string Description)> capabilities = new()
        {
            { "C1", ("Cap1Name", "Cap1Desc") },
            { "C2", ("Cap2Name", "Cap2Desc") },
            { "C3", ("Cap3Name", "Cap3Desc") },
            { "C4", ("Cap4Name", "Cap4Desc") },
            { "C5", ("Cap5Name", "Cap5Desc") },
        };

        private TestContext context;

        [SetUp]
        public void SetUpFixture()
        {
            context = new TestContext();
        }

        [Test]
        public void ShouldListSolutionsWithNoCapabilityFilter()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup1", false, "C2", "C3"));

            context.MockSolutionListRepository
                .Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositorySolutions);

            var solutions = GetSolutionsWithEmptyCapabilityReferencesFilterAsync();

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithSupplierIdFilter()
        {
            const string supplierId = "Sup1";

            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", supplierId, false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", supplierId, false, "C2", "C3"));

            context.MockSolutionListRepository
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

            context.MockSolutionListRepository
                .Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositorySolutions);

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

            context.MockSolutionListRepository
                .Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositorySolutions);

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

            context.MockSolutionListRepository
                .Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositorySolutions);

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
            context.MockSolutionListRepository
                .Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositorySolutions);

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
            context.MockSolutionListRepository
                .Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<ISolutionListResult>());

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

            context.MockSolutionListRepository
                .Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositorySolutions);

            var solutions = context.ListSolutionsHandler.Handle(
                new ListSolutionsQuery(Filter("C2")),
                CancellationToken.None);

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

            context.MockSolutionListRepository
                .Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositorySolutions);

            var solutions = context.ListSolutionsHandler.Handle(
                new ListSolutionsQuery(Filter("C1")),
                CancellationToken.None);

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

            context.MockSolutionListRepository
                .Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositorySolutions);

            var solutions = context.ListSolutionsHandler.Handle(
                new ListSolutionsQuery(Filter("C1")),
                CancellationToken.None);

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

            context.MockSolutionListRepository
                .Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositorySolutions);

            var solutions = context.ListSolutionsHandler.Handle(
                new ListSolutionsQuery(Filter("C2")),
                CancellationToken.None);

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

            context.MockSolutionListRepository
                .Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositorySolutions);

            var solutions = context.ListSolutionsHandler.Handle(
                new ListSolutionsQuery(Filter("C2")),
                CancellationToken.None);

            solutions.Result.Solutions.Should().HaveCount(4);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S3");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S4");
        }

        [Test]
        public void ShouldListSolutionsEmptyWithCapabilityFilter()
        {
            context.MockSolutionListRepository
                .Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<ISolutionListResult>());

            var solutions = context.ListSolutionsHandler.Handle(
                new ListSolutionsQuery(Filter("C2")),
                CancellationToken.None);

            solutions.Result.Solutions.Should().HaveCount(0);
        }

        [TestCase(new[] { "C1", "C2" }, new[] { "S123", "S124" })]
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

            context.MockSolutionListRepository
                .Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositorySolutions);

            if (filterCapabilityReferences is null)
            {
                throw new ArgumentNullException(nameof(filterCapabilityReferences));
            }

            var solutions = context.ListSolutionsHandler.Handle(
                new ListSolutionsQuery(Filter(filterCapabilityReferences)),
                CancellationToken.None);

            solutions.Result.Solutions.Select(s => s.Id).Should().BeEquivalentTo(expectedFilteredSolutions);
        }

        [TestCase(new[] { "C1", "C2" }, new[] { "S123", "S124" })]
        [TestCase(new[] { "C1" }, new[] { "S123", "S124", "S1", "S145", "S15", "S13" })]
        [TestCase(new[] { "C1", "C2", "C3" }, new[] { "S123" })]
        [TestCase(new[] { "C3", "C4" }, new string[0])]
        [TestCase(new[] { "C1", "C5" }, new[] { "S145", "S15" })]
        [TestCase(new[] { "C1", "C4" }, new[] { "S124", "S145" })]
        public void ShouldFilterByCapabilityMultipleOrganisations(
            string[] filterCapabilityReferences,
            string[] expectedFilteredSolutions)
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S123", "Sup1", false, "C1", "C2", "C3"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S124", "Sup2", false, "C1", "C2", "C4"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C1"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S145", "Sup2", false, "C1", "C4", "C5"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S15", "Sup1", false, "C1", "C5"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S13", "Sup2", false, "C1", "C3"));

            context.MockSolutionListRepository
                .Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositorySolutions);

            if (filterCapabilityReferences is null)
            {
                throw new ArgumentNullException(nameof(filterCapabilityReferences));
            }

            var solutions = context.ListSolutionsHandler.Handle(
                new ListSolutionsQuery(Filter(filterCapabilityReferences)),
                CancellationToken.None);

            solutions.Result.Solutions.Select(s => s.Id).Should().BeEquivalentTo(expectedFilteredSolutions);
        }

        [Test]
        public void ShouldListSolutionsDetail()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Sup1", false, "C1", "C2"));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Sup2", true, "C2"));

            context.MockSolutionListRepository
                .Setup(r => r.ListAsync(false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositorySolutions);

            var solutions = GetSolutionsWithEmptyCapabilityReferencesFilterAsync();

            solutions.Result.Solutions.Should().HaveCount(2);
            var solution = solutions.Result.Solutions.Should()
                .ContainSingle(s => string.Equals(s.Id, "S1", StringComparison.OrdinalIgnoreCase)).Subject;

            solution.Name.Should().Be("S1Name");
            solution.Summary.Should().Be("S1Summary");
            solution.IsFoundation.Should().BeFalse();
            solution.Supplier.Id.Should().Be(suppliers["Sup1"].Id);
            solution.Supplier.Name.Should().Be(suppliers["Sup1"].Name);
            solution.Capabilities.Should().HaveCount(2);

            var capability = solution.Capabilities.Should()
                .ContainSingle(c => string.Equals(c.CapabilityReference, "C1", StringComparison.OrdinalIgnoreCase)).Subject;

            capability.Name.Should().Be(capabilities["C1"].Name);
            capability = solution.Capabilities.Should()
                .ContainSingle(c => string.Equals(c.CapabilityReference, "C2", StringComparison.OrdinalIgnoreCase)).Subject;

            capability.Name.Should().Be(capabilities["C2"].Name);

            solution = solutions.Result.Solutions.Should()
                .ContainSingle(s => string.Equals(s.Id, "S2", StringComparison.OrdinalIgnoreCase)).Subject;

            solution.Name.Should().Be("S2Name");
            solution.Summary.Should().Be("S2Summary");
            solution.IsFoundation.Should().BeTrue();
            solution.Supplier.Id.Should().Be(suppliers["Sup2"].Id);
            solution.Supplier.Name.Should().Be(suppliers["Sup2"].Name);
            solution.Capabilities.Should().HaveCount(1);

            capability = solution.Capabilities.Should()
                .ContainSingle(c => string.Equals(c.CapabilityReference, "C2", StringComparison.OrdinalIgnoreCase)).Subject;

            capability.Name.Should().Be(capabilities["C2"].Name);
        }

        [Test]
        public void ShouldRequestFoundationSolutions()
        {
            context.MockSolutionListRepository
                .Setup(r => r.ListAsync(true, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<ISolutionListResult>());

            var solutions = GetSolutionsWithEmptyCapabilityReferencesFilterAsync(true);

            solutions.Result.Solutions.Should().HaveCount(0);
            context.MockSolutionListRepository.Verify(r => r.ListAsync(true, null, It.IsAny<CancellationToken>()));
        }

        [Test]
        public void QueryShouldThrowNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new ListSolutionsQuery(null));
        }

        private static IListSolutionsQueryData Filter(string capabilityReference)
        {
            return Filter(new[] { capabilityReference });
        }

        private static IListSolutionsQueryData Filter(IEnumerable<string> capabilityReferences)
        {
            var capRefMocks = capabilityReferences.Select(cr => Mock.Of<ICapabilityReference>(c => c.Reference == cr));

            // ReSharper disable once PossibleUnintendedReferenceComparison
            var filter = Mock.Of<IListSolutionsQueryData>(
                data => data.CapabilityReferences == new List<ICapabilityReference>(capRefMocks).ToList());

            return filter;
        }

        private async Task<ISolutionList> GetSolutionsWithEmptyCapabilityReferencesFilterAsync(
            bool isFoundation = false,
            string supplierId = null)
        {
            // ReSharper disable once PossibleUnintendedReferenceComparison
            Expression<Func<IListSolutionsQueryData, bool>> queryData = data =>
                data.CapabilityReferences == new HashSet<ICapabilityReference>()
                && data.IsFoundation == isFoundation
                && data.SupplierId == supplierId;

            return await context.ListSolutionsHandler.Handle(
                new ListSolutionsQuery(Mock.Of(queryData)),
                CancellationToken.None);
        }

        private IEnumerable<ISolutionListResult> GetSolutionWithCapabilities(
            string solutionId,
            string supplierId,
            bool isFoundation,
            params string[] capabilityReferences)
        {
            return capabilityReferences.Select(
                capabilityReference => GetSolution(solutionId, supplierId, capabilityReference, isFoundation));
        }

        private ISolutionListResult GetSolution(
            string solutionId,
            string supplierId,
            string capabilityReference,
            bool isFoundation)
        {
            (string capabilityName, string capabilityDescription) = capabilities[capabilityReference];

            var solution = new Mock<ISolutionListResult>();
            solution.Setup(c => c.SolutionId).Returns($"{solutionId}");
            solution.Setup(c => c.SolutionName).Returns($"{solutionId}Name");
            solution.Setup(c => c.SolutionSummary).Returns($"{solutionId}Summary");
            solution.Setup(c => c.IsFoundation).Returns(isFoundation);

            solution.Setup(c => c.SupplierId).Returns(suppliers[supplierId].Id);
            solution.Setup(c => c.SupplierName).Returns(suppliers[supplierId].Name);

            solution.Setup(c => c.CapabilityReference).Returns(capabilityReference);
            solution.Setup(c => c.CapabilityName).Returns(capabilityName);
            solution.Setup(c => c.CapabilityDescription).Returns(capabilityDescription);

            return solution.Object;
        }
    }
}
