using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
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
                r.SolutionId == "Sln1"
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
            var validationResult = await UpdateSolutionDescriptionAsync(summary);

            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["summary"].Should().Be("required");

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());
            _context.MockSolutionDetailRepository.Verify(r => r.UpdateSummaryAsync(It.IsAny<IUpdateSolutionSummaryRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public async Task ShouldValidateForMaxLengthDescription()
        {
            var validationResult = await UpdateSolutionDescriptionAsync(description: new string('a', 1101));

            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["description"].Should().Be("maxLength");

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());
            _context.MockSolutionDetailRepository.Verify(r => r.UpdateSummaryAsync(It.IsAny<IUpdateSolutionSummaryRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public async Task ShouldValidateForMaxLengthLink()
        {
            var validationResult = await UpdateSolutionDescriptionAsync(link: new string('a', 1001));

            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["link"].Should().Be("maxLength");

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());
            _context.MockSolutionDetailRepository.Verify(r => r.UpdateSummaryAsync(It.IsAny<IUpdateSolutionSummaryRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public async Task ShouldValidateForMaxLengthSummary()
        {
            var validationResult = await UpdateSolutionDescriptionAsync(new string('a', 351));

            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["summary"].Should().Be("maxLength");

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());
            _context.MockSolutionDetailRepository.Verify(r => r.UpdateSummaryAsync(It.IsAny<IUpdateSolutionSummaryRequest>(), It.IsAny<CancellationToken>()), Times.Never());

        }

        [Test]
        public async Task ShouldValidateCombinations()
        {
            var validationResult = await UpdateSolutionDescriptionAsync("", new string('a', 1101), new string('a', 1001));

            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(3);
            results["summary"].Should().Be("required");
            results["description"].Should().Be("maxLength");
            results["link"].Should().Be("maxLength");

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());
            _context.MockSolutionDetailRepository.Verify(r => r.UpdateSummaryAsync(It.IsAny<IUpdateSolutionSummaryRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public async Task ShouldValidateValidCases()
        {
            var validationResult = await UpdateSolutionDescriptionAsync("Summary", null, null);

            validationResult.IsValid.Should().BeTrue();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(0);

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
            _context.MockSolutionDetailRepository.Verify(r => r.UpdateSummaryAsync(It.IsAny<IUpdateSolutionSummaryRequest>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Task UpdateSummary()
            {
                var summaryModel = new UpdateSolutionSummaryViewModel
                {
                    Description = "Description",
                    Link = "Link",
                    Summary = "Summary",
                };

                return _context.UpdateSolutionSummaryHandler.Handle(
                    new UpdateSolutionSummaryCommand("Sln1", summaryModel),
                    new CancellationToken());
            }

            Assert.ThrowsAsync<NotFoundException>(UpdateSummary);

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionDetailRepository.Verify(r => r.UpdateSummaryAsync(
                It.IsAny<IUpdateSolutionSummaryRequest>(),
                It.IsAny<CancellationToken>()),
                Times.Never());
        }

        private async Task<ISimpleResult> UpdateSolutionDescriptionAsync(string summary = "Summary", string description = "Description", string link = "Link")
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var validationResult = await _context.UpdateSolutionSummaryHandler.Handle(new UpdateSolutionSummaryCommand("Sln1",
                new UpdateSolutionSummaryViewModel
                {
                    Description = description,
                    Link = link,
                    Summary = summary,
                }), new CancellationToken());
            return validationResult;
        }
    }
}
