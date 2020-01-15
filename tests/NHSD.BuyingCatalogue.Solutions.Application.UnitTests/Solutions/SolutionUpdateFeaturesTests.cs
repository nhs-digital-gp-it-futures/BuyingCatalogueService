using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionUpdateFeaturesTests
    {
        private TestContext _context;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
        }

        [Test]
        public async Task ShouldUpdateSolutionFeatures()
        {
            var listing = new List<string> { "sheep", "cow", "donkey" };

            var validationResult = await UpdateSolutionFeaturesAsync(listing)
                .ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(0);

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionDetailRepository.Verify(r => r.UpdateFeaturesAsync(It.Is<IUpdateSolutionFeaturesRequest>(r =>
                r.SolutionId == SolutionId
                && r.Features == JsonConvert.SerializeObject(listing)
                ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldValidateSingleMaxLength()
        {
            var validationResult = await UpdateSolutionFeaturesAsync(new List<string>() { new string('a', 101), "test" })
                .ConfigureAwait(false);
            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["listing-1"].Should().Be("maxLength");

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Never());
            _context.MockSolutionDetailRepository.Verify(r => r.UpdateFeaturesAsync(It.IsAny<IUpdateSolutionFeaturesRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task ShouldValidateMultipleMaxLength()
        {
            var validationResult = await UpdateSolutionFeaturesAsync(new List<string>() { new string('a', 101), "test", new string('b', 200), "test", "test", new string('c', 105) })
                .ConfigureAwait(false);
            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(3);
            results["listing-1"].Should().Be("maxLength");
            results["listing-3"].Should().Be("maxLength");
            results["listing-6"].Should().Be("maxLength");

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Never());
            _context.MockSolutionDetailRepository.Verify(r => r.UpdateFeaturesAsync(It.IsAny<IUpdateSolutionFeaturesRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            var listing = new List<string> { "sheep", "cow", "donkey" };

            var exception = Assert.ThrowsAsync<NotFoundException>(() =>
                _context.UpdateSolutionFeaturesHandler.Handle(new UpdateSolutionFeaturesCommand(SolutionId,
                    new UpdateSolutionFeaturesViewModel
                    {
                        Listing = listing
                    }), new CancellationToken()));

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionDetailRepository.Verify(
                r => r.UpdateFeaturesAsync(It.IsAny<IUpdateSolutionFeaturesRequest>(), It.IsAny<CancellationToken>()),
                Times.Never());
        }

        private async Task<ISimpleResult> UpdateSolutionFeaturesAsync(List<string> listing)
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns(SolutionId);

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            var validationResult = await _context.UpdateSolutionFeaturesHandler.Handle(
                new UpdateSolutionFeaturesCommand(SolutionId,
                    new UpdateSolutionFeaturesViewModel() { Listing = listing }), new CancellationToken())
                .ConfigureAwait(false);
            return validationResult;
        }
    }
}
