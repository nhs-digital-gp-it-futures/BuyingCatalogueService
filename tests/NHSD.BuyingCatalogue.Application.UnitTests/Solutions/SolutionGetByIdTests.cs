using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionGetByIdTests
    {
        private TestContext _context;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
        }

        [Test]
        public async Task ShouldGetSolutionById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.Description).Returns("Description");
            existingSolution.Setup(s => s.Summary).Returns("Summary");
            existingSolution.Setup(s => s.AboutUrl).Returns("AboutUrl");
            existingSolution.Setup(s => s.Features).Returns("[ 'Marmite', 'Jam', 'Marmelade' ]");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken());

            solution.Solution.Id.Should().Be("Sln1");
            solution.Solution.Name.Should().Be("Name");
            solution.Solution.MarketingData.Sections.Should().HaveCount(2);

            var solutionDescriptionSection = (SolutionDescriptionSection)solution.Solution.MarketingData.Sections.Should().Contain(s => s.Id.Equals("solution-description")).Subject;
            solutionDescriptionSection.Requirement.Should().Be("Mandatory");
            solutionDescriptionSection.Status.Should().Be("COMPLETE");
            solutionDescriptionSection.Data.Summary.Should().Be("Summary");
            solutionDescriptionSection.Data.Description.Should().Be("Description");
            solutionDescriptionSection.Data.Link.Should().Be("AboutUrl");
            solutionDescriptionSection.Mandatory.Should().BeEquivalentTo( new string[] { "summary" });

            var featuresSection = (FeaturesSection)solution.Solution.MarketingData.Sections.Should().Contain(s => s.Id.Equals("features")).Subject;
            featuresSection.Requirement.Should().Be("Optional");
            featuresSection.Status.Should().Be("COMPLETE");
            featuresSection.Data.Listing.Should().BeEquivalentTo(new string []{ "Marmite", "Jam", "Marmelade" });
            featuresSection.Mandatory.Should().BeEquivalentTo(new string[0]);

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }


        [Test]
        public async Task ShouldGetEmptySolutionById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.Description).Returns((string)null);
            existingSolution.Setup(s => s.Summary).Returns((string)null);
            existingSolution.Setup(s => s.AboutUrl).Returns((string)null);
            existingSolution.Setup(s => s.Features).Returns((string)null);

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken());

            solution.Solution.Id.Should().Be("Sln1");
            solution.Solution.Name.Should().Be("Name");
            solution.Solution.MarketingData.Sections.Should().HaveCount(2);

            var solutionDescriptionSection = (SolutionDescriptionSection)solution.Solution.MarketingData.Sections.Should().Contain(s => s.Id.Equals("solution-description")).Subject;
            solutionDescriptionSection.Requirement.Should().Be("Mandatory");
            solutionDescriptionSection.Status.Should().Be("INCOMPLETE");
            solutionDescriptionSection.Data.Summary.Should().BeNullOrEmpty();
            solutionDescriptionSection.Data.Description.Should().BeNullOrEmpty();
            solutionDescriptionSection.Data.Link.Should().BeNullOrEmpty();

            var featuresSection = (FeaturesSection)solution.Solution.MarketingData.Sections.Should().Contain(s => s.Id.Equals("features")).Subject;
            featuresSection.Requirement.Should().Be("Optional");
            featuresSection.Status.Should().Be("INCOMPLETE");
            featuresSection.Data.Listing.Should().BeEmpty();

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldGetPartialSolutionById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.Description).Returns((string)null);
            existingSolution.Setup(s => s.Summary).Returns("Summary");
            existingSolution.Setup(s => s.AboutUrl).Returns((string)null);
            existingSolution.Setup(s => s.Features).Returns((string)null);

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken());

            solution.Solution.Id.Should().Be("Sln1");
            solution.Solution.Name.Should().Be("Name");
            solution.Solution.MarketingData.Sections.Should().HaveCount(2);

            var solutionDescriptionSection = (SolutionDescriptionSection)solution.Solution.MarketingData.Sections.Should().Contain(s => s.Id.Equals("solution-description")).Subject;
            solutionDescriptionSection.Requirement.Should().Be("Mandatory");
            solutionDescriptionSection.Status.Should().Be("COMPLETE");
            solutionDescriptionSection.Data.Summary.Should().Be("Summary");
            solutionDescriptionSection.Data.Description.Should().BeNullOrEmpty();
            solutionDescriptionSection.Data.Link.Should().BeNullOrEmpty();

            var featuresSection = (FeaturesSection)solution.Solution.MarketingData.Sections.Should().Contain(s => s.Id.Equals("features")).Subject;
            featuresSection.Requirement.Should().Be("Optional");
            featuresSection.Status.Should().Be("INCOMPLETE");
            featuresSection.Data.Listing.Should().BeEmpty(); 

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldGetPartialSolutionWithMarketingDataById()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.Description).Returns((string)null);
            existingSolution.Setup(s => s.Summary).Returns((string)null);
            existingSolution.Setup(s => s.AboutUrl).Returns((string)null);
            existingSolution.Setup(s => s.Features).Returns("[ 'Marmite', 'Jam', 'Marmelade' ]");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken());

            solution.Solution.Id.Should().Be("Sln1");
            solution.Solution.Name.Should().Be("Name");
            solution.Solution.MarketingData.Sections.Should().HaveCount(2);

            var solutionDescriptionSection = (SolutionDescriptionSection)solution.Solution.MarketingData.Sections.Should().Contain(s => s.Id.Equals("solution-description")).Subject;
            solutionDescriptionSection.Requirement.Should().Be("Mandatory");
            solutionDescriptionSection.Status.Should().Be("INCOMPLETE");
            solutionDescriptionSection.Data.Summary.Should().BeNullOrEmpty();
            solutionDescriptionSection.Data.Description.Should().BeNullOrEmpty();
            solutionDescriptionSection.Data.Link.Should().BeNullOrEmpty();

            var featuresSection = (FeaturesSection)solution.Solution.MarketingData.Sections.Should().Contain(s => s.Id.Equals("features")).Subject;
            featuresSection.Requirement.Should().Be("Optional");
            featuresSection.Status.Should().Be("COMPLETE");
            featuresSection.Data.Listing.Should().BeEquivalentTo(new string[] { "Marmite", "Jam", "Marmelade" });

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldInterpretWhitespaceAsMissing()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.Name).Returns("Name");
            existingSolution.Setup(s => s.Description).Returns((string)null);
            existingSolution.Setup(s => s.Summary).Returns("   ");//whitespace
            existingSolution.Setup(s => s.AboutUrl).Returns((string)null);
            existingSolution.Setup(s => s.Features).Returns("[ '   ', ' ', '' ]");//whitespace

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var solution = await _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken());

            solution.Solution.Id.Should().Be("Sln1");
            solution.Solution.Name.Should().Be("Name");
            solution.Solution.MarketingData.Sections.Should().HaveCount(2);

            var solutionDescriptionSection = (SolutionDescriptionSection)solution.Solution.MarketingData.Sections.Should().Contain(s => s.Id.Equals("solution-description")).Subject;
            solutionDescriptionSection.Requirement.Should().Be("Mandatory");
            solutionDescriptionSection.Status.Should().Be("INCOMPLETE");
            solutionDescriptionSection.Data.Summary.Should().Be("   ");
            solutionDescriptionSection.Data.Description.Should().BeNullOrEmpty();
            solutionDescriptionSection.Data.Link.Should().BeNullOrEmpty();

            var featuresSection = (FeaturesSection)solution.Solution.MarketingData.Sections.Should().Contain(s => s.Id.Equals("features")).Subject;
            featuresSection.Requirement.Should().Be("Optional");
            featuresSection.Status.Should().Be("INCOMPLETE");
            featuresSection.Data.Listing.Should().BeEquivalentTo(new string[] { "   ", " ", "" });

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            var exception = Assert.ThrowsAsync<NotFoundException>(() =>
                _context.GetSolutionByIdHandler.Handle(new GetSolutionByIdQuery("Sln1"), new CancellationToken()));

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
