using System.Collections.Generic;
using System.Linq;
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
    internal sealed class SolutionUpdateBrowsersSupportedTests
    {
        private TestContext _context;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
        }

        [Test]
        public async Task ShouldUpdateSolutionBrowsersSupported()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            await _context.UpdateSolutionBrowsersSupportedHandler.Handle(new UpdateSolutionBrowsersSupportedCommand("Sln1",
                new UpdateSolutionBrowsersSupportedViewModel
                {
                    BrowsersSupported = new HashSet<string> { "Edge", "Google Chrome" },
                    MobileResponsive = "yes"
                    
                }), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.Id == "Sln1"
                && JToken.Parse(r.ClientApplication).SelectToken("BrowsersSupported").Select(s => s.Value<string>()).Contains("Edge")
                && JToken.Parse(r.ClientApplication).SelectToken("BrowsersSupported").Select(s => s.Value<string>()).Contains("Google Chrome")
                && !JToken.Parse(r.ClientApplication).SelectToken("BrowsersSupported").Select(s => s.Value<string>()).Contains("IE6")
                && JToken.Parse(r.ClientApplication).SelectToken("MobileResponsive").Value<bool>() == true
                && JToken.Parse(r.ClientApplication).SelectToken("BrowsersSupported").Count() == 2
                ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldUpdateEmptySolutionBrowsersSupported()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            await _context.UpdateSolutionBrowsersSupportedHandler.Handle(new UpdateSolutionBrowsersSupportedCommand("Sln1",
                new UpdateSolutionBrowsersSupportedViewModel
                {
                    BrowsersSupported = new HashSet<string>()
                }), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.Id == "Sln1"
                && !JToken.Parse(r.ClientApplication).SelectToken("BrowsersSupported").Select(s => s.Value<string>()).Contains("Edge")
                && !JToken.Parse(r.ClientApplication).SelectToken("BrowsersSupported").Select(s => s.Value<string>()).Contains("Google Chrome")
                && !JToken.Parse(r.ClientApplication).SelectToken("BrowsersSupported").Select(s => s.Value<string>()).Contains("IE6")
                && JToken.Parse(r.ClientApplication).SelectToken("BrowsersSupported").Count() == 0
            && JToken.Parse(r.ClientApplication).SelectToken("MobileResponsive").Value<bool?>() == null
            ), It.IsAny<CancellationToken>()), Times.Once());
        }
        
        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            var exception = Assert.ThrowsAsync<NotFoundException>(() =>
                _context.UpdateSolutionBrowsersSupportedHandler.Handle(new UpdateSolutionBrowsersSupportedCommand("Sln1",
                    new UpdateSolutionBrowsersSupportedViewModel
                    {
                        BrowsersSupported = new HashSet<string> { "Edge", "Google Chrome" }
                    }), new CancellationToken()));

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}
