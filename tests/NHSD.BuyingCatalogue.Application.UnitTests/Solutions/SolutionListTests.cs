using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Application.SolutionList.Queries.ListSolutions;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions
{
    [TestFixture]
    public class SolutionListTests
    {
        private TestContext _context;

        private readonly Dictionary<string, (string name, Guid id)> Organisations = new Dictionary<string, (string name, Guid id)>
        {
            { "Org1", (name: "Org1Name", id: Guid.NewGuid())},
            { "Org2", (name: "Org2Name", id: Guid.NewGuid())},
            { "Org3", (name: "Org3Name", id: Guid.NewGuid())},
        };

        private readonly Dictionary<int, (Guid Id, string Name, string Description)> Capabilities = new Dictionary<int, (Guid Id, string Name, string Description)>
        {
            { 1, (Guid.NewGuid(), "Cap1Name", "Cap1Desc")},
            { 2, (Guid.NewGuid(), "Cap2Name", "Cap2Desc")},
            { 3, (Guid.NewGuid(), "Cap3Name", "Cap3Desc")},
            { 4, (Guid.NewGuid(), "Cap4Name", "Cap4Desc")},
            { 5, (Guid.NewGuid(), "Cap5Name", "Cap5Desc")},
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
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Org1", false,  1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Org1", false, 2, 3));
            _context.MockSolutionRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithNoCapabilityFilterReturnDistinctByOrganisation()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Org1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Org2", false, 1, 2));
            _context.MockSolutionRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithNoCapabilityFilterReturnDistinctByOrganisationAndCapability()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Org1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Org1", false, 1, 2));
            _context.MockSolutionRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(1);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
        }

        [Test]
        public void ShouldListSolutionsWithNoCapabilityFilterReturnDistinctByCapability()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Org1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Org1", false, 2, 3));
            _context.MockSolutionRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithNoCapabilityFilterReturnDistinctByOrganisationAndCapabilityTwoOrganisations()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Org1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Org1", false, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S3", "Org2", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S4", "Org2", false, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S4", "Org2", false, 2, 3));
            _context.MockSolutionRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(), new CancellationToken());

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
            _context.MockSolutionRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(0);
        }

        [Test]
        public void ShouldListSolutionsWithCapabilityFilter()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Org1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Org1", false, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Org1", false, 3, 4));
            _context.MockSolutionRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter(2)), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithCapabilityFilterReturnDistinctByOrganisation()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Org1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Org1", false, 4, 5));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Org2", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Org2", false, 3, 4));
            _context.MockSolutionRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter(1)), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithCapabilityFilterReturnDistinctByOrganisationAndCapability()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Org1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Org1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Org1", false, 3, 4));
            _context.MockSolutionRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter(1)), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(1);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
        }

        [Test]
        public void ShouldListSolutionsWithCapabilityFilterReturnDistinctByCapability()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Org1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Org1", false, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S3", "Org1", false, 3, 4));
            _context.MockSolutionRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter(2)), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(2);
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S1");
            solutions.Result.Solutions.Select(s => s.Id).Should().Contain("S2");
        }

        [Test]
        public void ShouldListSolutionsWithCapabilityFilterReturnDistinctByOrganisationAndCapabilityTwoOrganisations()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Org1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Org1", false, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S3", "Org2", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S4", "Org2", false, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S4", "Org2", false, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S6", "Org2", false, 4, 3));
            _context.MockSolutionRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

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
            _context.MockSolutionRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

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
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S123", "Org1", false, 1, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S124", "Org1", false, 1, 2, 4));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Org1", false, 1));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S145", "Org1", false, 1, 4, 5));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S15", "Org1", false, 1, 5));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S13", "Org1", false, 1, 3));
            _context.MockSolutionRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter(filterCapabilityIds)), new CancellationToken());

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
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S123", "Org1", false, 1, 2, 3));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S124", "Org2", false, 1, 2, 4));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Org1", false, 1));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S145", "Org2", false, 1, 4, 5));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S15", "Org1", false, 1, 5));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S13", "Org2", false, 1, 3));
            _context.MockSolutionRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(Filter(filterCapabilityIds)), new CancellationToken());

            solutions.Result.Solutions.Select(s => s.Id).Should().BeEquivalentTo(expectedFilteredSolutions);
        }

        [Test]
        public void ShouldListSolutionsDetail()
        {
            var repositorySolutions = new List<ISolutionListResult>();
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S1", "Org1", false, 1, 2));
            repositorySolutions.AddRange(GetSolutionWithCapabilities("S2", "Org2", true, 2));
            _context.MockSolutionRepository.Setup(r => r.ListAsync(false, It.IsAny<CancellationToken>())).ReturnsAsync(repositorySolutions);

            var solutions = _context.ListSolutionsHandler.Handle(new ListSolutionsQuery(), new CancellationToken());

            solutions.Result.Solutions.Should().HaveCount(2);
            var solution = solutions.Result.Solutions.Should().ContainSingle(s => s.Id.Equals("S1")).Subject;
            solution.Name.Should().Be("S1Name");
            solution.Summary.Should().Be("S1Summary");
            solution.IsFoundation.Should().BeFalse();
            solution.Organisation.Id.Should().Be(Organisations["Org1"].id);
            solution.Organisation.Name.Should().Be("Org1Name");
            solution.Capabilities.Should().HaveCount(2);
            solution.Capabilities.Single(c => c.Id.Equals(Capabilities[1].Id));

            var capability  = solution.Capabilities.Should().ContainSingle(c => c.Id == Capabilities[1].Id).Subject;
            capability.Name.Should().Be(Capabilities[1].Name);
            capability = solution.Capabilities.Should().ContainSingle(c => c.Id == Capabilities[2].Id).Subject;
            capability.Name.Should().Be(Capabilities[2].Name);

            solution = solutions.Result.Solutions.Should().ContainSingle(s => s.Id.Equals("S2")).Subject;
            solution.Name.Should().Be("S2Name");
            solution.Summary.Should().Be("S2Summary");
            solution.IsFoundation.Should().BeTrue();
            solution.Organisation.Id.Should().Be(Organisations["Org2"].id);
            solution.Organisation.Name.Should().Be("Org2Name");
            solution.Capabilities.Should().HaveCount(1);
            solution.Capabilities.Single(c => c.Id.Equals(Capabilities[2].Id));

            capability = solution.Capabilities.Should().ContainSingle(c => c.Id == Capabilities[2].Id).Subject;
            capability.Name.Should().Be(Capabilities[2].Name);
        }

        private ListSolutionsFilter Filter(int capabilityId)
        {
            return Filter(new int[] {capabilityId});
        }

        private ListSolutionsFilter Filter(IEnumerable<int> capabilityIds)
        {
            var filter = new ListSolutionsFilter();
            foreach (var capabilityId in capabilityIds)
            {
                filter.Capabilities.Add(Capabilities[capabilityId].Id);
            }

            return filter;
        }

        private IEnumerable<ISolutionListResult> GetSolutionWithCapabilities(string solnId, string orgId, bool isFoundation, params int[] capabilityIds)
        {
            return capabilityIds.Select(capabilityId => GetSolution(solnId, orgId, capabilityId, isFoundation));
        }

        private ISolutionListResult GetSolution(string solnId, string orgId, int capabilityId, bool isFoundation)
        {
            var capability = Capabilities[capabilityId];
            var solution = new Mock<ISolutionListResult>();
            solution.Setup(c => c.SolutionId).Returns($"{solnId}");
            solution.Setup(c => c.SolutionName).Returns($"{solnId}Name");
            solution.Setup(c => c.SolutionSummary).Returns($"{solnId}Summary");
            solution.Setup(c => c.IsFoundation).Returns(isFoundation);

            solution.Setup(c => c.OrganisationId).Returns(Organisations[orgId].id);
            solution.Setup(c => c.OrganisationName).Returns(Organisations[orgId].name);

            solution.Setup(c => c.CapabilityId).Returns(capability.Id);
            solution.Setup(c => c.CapabilityName).Returns(capability.Name);
            solution.Setup(c => c.CapabilityDescription).Returns(capability.Description);
            
            return solution.Object;
        }
    }
}
