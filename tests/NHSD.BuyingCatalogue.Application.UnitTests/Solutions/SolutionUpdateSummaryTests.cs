using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionUpdateSummaryTests
    {
        private TestContext _context;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
        }

        [Test]
        public async Task ShouldUpdateSolutionSummary()
        {
            var validationResult = await UpdateSolutionDescriptionAsync();
            validationResult.IsValid.Should().BeTrue();

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionDetailRepository.Verify(r => r.UpdateSummaryAsync(It.Is<IUpdateSolutionSummaryRequest>(r =>
                r.Id == "Sln1"
                && r.AboutUrl == "Link"
                && r.Description == "Description"
                && r.Summary == "Summary"
                ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("	")]//tab
        [TestCase(" ")]//space
        public async Task ShouldValidateForExistenceOfSummary(string summary)
        {
            var validationResult = await UpdateSolutionDescriptionAsync(summary: summary);

            validationResult.IsValid.Should().BeFalse();
            validationResult.Required.Should().BeEquivalentTo(new [] {"summary"});
            validationResult.MaxLength.Should().BeEmpty();

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());
            _context.MockSolutionDetailRepository.Verify(r => r.UpdateSummaryAsync(It.IsAny<IUpdateSolutionSummaryRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public async Task ShouldValidateForMaxLengthDescription()
        {
            var validationResult = await UpdateSolutionDescriptionAsync(description: new string('a', 1001));

            validationResult.IsValid.Should().BeFalse();
            validationResult.Required.Should().BeEmpty();
            validationResult.MaxLength.Should().BeEquivalentTo(new[] { "description" });

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());
            _context.MockSolutionDetailRepository.Verify(r => r.UpdateSummaryAsync(It.IsAny<IUpdateSolutionSummaryRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public async Task ShouldValidateForMaxLengthLink()
        {
            var validationResult = await UpdateSolutionDescriptionAsync(link: new string('a', 1001));

            validationResult.IsValid.Should().BeFalse();
            validationResult.Required.Should().BeEmpty();
            validationResult.MaxLength.Should().BeEquivalentTo(new[] { "link" });

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());
            _context.MockSolutionDetailRepository.Verify(r => r.UpdateSummaryAsync(It.IsAny<IUpdateSolutionSummaryRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public async Task ShouldValidateForMaxLengthSummary()
        {
            var validationResult = await UpdateSolutionDescriptionAsync(summary: new string('a', 301));

            validationResult.IsValid.Should().BeFalse();
            validationResult.Required.Should().BeEmpty();
            validationResult.MaxLength.Should().BeEquivalentTo(new[] { "summary" });

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());
            _context.MockSolutionDetailRepository.Verify(r => r.UpdateSummaryAsync(It.IsAny<IUpdateSolutionSummaryRequest>(), It.IsAny<CancellationToken>()), Times.Never());

        }

        [Test]
        public async Task ShouldValidateCombinations()
        {
            var validationResult = await UpdateSolutionDescriptionAsync(summary: "", description: new string('a', 1001), link: new string('a', 1001));

            validationResult.IsValid.Should().BeFalse();
            validationResult.Required.Should().BeEquivalentTo(new[] { "summary" });
            validationResult.MaxLength.Should().BeEquivalentTo(new[] { "description", "link" });

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());
            _context.MockSolutionDetailRepository.Verify(r => r.UpdateSummaryAsync(It.IsAny<IUpdateSolutionSummaryRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public async Task ShouldValidateValidCases()
        {
            var validationResult = await UpdateSolutionDescriptionAsync(summary: "Summary", description: null, link: null);

            validationResult.IsValid.Should().BeTrue();
            validationResult.Required.Should().BeEmpty();
            validationResult.MaxLength.Should().BeEmpty();

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
            _context.MockSolutionDetailRepository.Verify(r => r.UpdateSummaryAsync(It.IsAny<IUpdateSolutionSummaryRequest>(), It.IsAny<CancellationToken>()), Times.Once());

        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            var exception = Assert.ThrowsAsync<NotFoundException>(() =>  
             _context.UpdateSolutionSummaryHandler.Handle(new UpdateSolutionSummaryCommand("Sln1",
                new UpdateSolutionSummaryViewModel
                {
                    Description = "Description",
                    Link = "Link",
                    Summary = "Summary"
                }), new CancellationToken()));

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionDetailRepository.Verify(r => r.UpdateSummaryAsync(It.IsAny<IUpdateSolutionSummaryRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        private async Task<UpdateSolutionSummaryValidationResult> UpdateSolutionDescriptionAsync(string summary = "Summary", string description = "Description", string link = "Link")
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var validationResult = await _context.UpdateSolutionSummaryHandler.Handle(new UpdateSolutionSummaryCommand("Sln1",
                new UpdateSolutionSummaryViewModel
                {
                    Description = description,
                    Link = link,
                    Summary = summary
                }), new CancellationToken());
            return validationResult;
        }
    }
}
