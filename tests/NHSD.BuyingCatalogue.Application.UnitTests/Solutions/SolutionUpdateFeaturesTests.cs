using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionFeatures;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionUpdateFeaturesTests
    {
        private TestContext _context;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
        }

        [Test]
        public async Task ShouldUpdateSolutionFeatures()
        {
            var listing = new List<string> { "sheep", "cow", "donkey" };

            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            await _context.UpdateSolutionFeaturesHandler.Handle(new UpdateSolutionFeaturesCommand("Sln1",
                new UpdateSolutionFeaturesViewModel
                {
                    Listing = listing
                }), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockMarketingDetailRepository.Verify(r => r.UpdateFeaturesAsync(It.Is<IUpdateSolutionFeaturesRequest>(r =>
                r.Id == "Sln1"
                && r.Features == JsonConvert.SerializeObject(listing).ToString()
                ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            var listing = new List<string> { "sheep", "cow", "donkey" };

            var exception = Assert.ThrowsAsync<NotFoundException>(() =>
                _context.UpdateSolutionFeaturesHandler.Handle(new UpdateSolutionFeaturesCommand("Sln1",
                    new UpdateSolutionFeaturesViewModel
                    {
                        Listing = listing
                    }), new CancellationToken()));

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockMarketingDetailRepository.Verify(r => r.UpdateFeaturesAsync(It.IsAny<IUpdateSolutionFeaturesRequest>(), It.IsAny<CancellationToken>()), Times.Never());

        }
    }
}
