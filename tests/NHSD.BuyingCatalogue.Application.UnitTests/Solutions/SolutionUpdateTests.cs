using System.Threading;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionUpdateTests
    {
        private TestContext _context;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
        }

        [Test]
        public async Task ShouldUpdateSolution()
        {
            var marketingData = JObject.Parse("{ 'MarketingData': 'some data' }");

            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            await _context.UpdateSolutionHandler.Handle(new UpdateSolutionCommand("Sln1",
                new UpdateSolutionViewModel
                {
                    Description = "Description",
                    AboutUrl = "AboutUrl",
                    MarketingData = marketingData,
                    Summary = "Summary"
                }), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionRepository.Verify(r => r.UpdateAsync(It.Is<IUpdateSolutionRequest>(r =>
                r.Id == "Sln1"
                && r.AboutUrl == "AboutUrl"
                && r.Description == "Description"
                && r.Summary == "Summary"
                && r.Features == marketingData.ToString()
                ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            var marketingData = JObject.Parse("{ 'MarketingData': 'some data' }");

            var exception = Assert.ThrowsAsync<NotFoundException>(() =>  
             _context.UpdateSolutionHandler.Handle(new UpdateSolutionCommand("Sln1",
                new UpdateSolutionViewModel
                {
                    Description = "Description",
                    AboutUrl = "AboutUrl",
                    MarketingData = marketingData,
                    Summary = "Summary"
                }), new CancellationToken()));

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockSolutionRepository.Verify(r => r.UpdateAsync(It.IsAny<IUpdateSolutionRequest>(), It.IsAny<CancellationToken>()), Times.Never());

        }
    }
}
