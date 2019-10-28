using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldUpdateSolutionBrowsersSupportedAndNothingElse()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.ClientApplication).Returns(
                "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false }");

            var calledBack = false;

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);
            _context.MockMarketingDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;

                    var clientApplicationTypes = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication)
                        .SelectToken("ClientApplicationTypes").Select(s => s.Value<string>());

                    clientApplicationTypes.Should().Contain("native-mobile");
                    clientApplicationTypes.Should().NotContain("native-desktop");
                    clientApplicationTypes.Should().Contain("browser-based");
                    clientApplicationTypes.Should().HaveCount(2);

                    var browsersSupported = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("BrowsersSupported").Select(s => s.Value<string>());

                    browsersSupported.Should().Contain("Google Chrome");
                    browsersSupported.Should().Contain("Edge");
                    browsersSupported.Should().NotContain("Mozilla Firefox");
                    browsersSupported.Should().HaveCount(2);
                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("MobileResponsive").Value<bool>().Should().BeTrue();
                });

            await _context.UpdateSolutionBrowsersSupportedHandler.Handle(new UpdateSolutionBrowsersSupportedCommand("Sln1",
                new UpdateSolutionBrowsersSupportedViewModel
                {
                    BrowsersSupported = new HashSet<string> { "Edge", "Google Chrome" },
                    MobileResponsive = "yes"

                }), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
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
        public async Task ShouldUpdateEmptySolutionBrowsersSupportedAndNothingElse()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.ClientApplication).Returns(
                "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false }");
            var calledBack = false;

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);
            _context.MockMarketingDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;

                    var clientApplicationTypes = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication)
                        .SelectToken("ClientApplicationTypes").Select(s => s.Value<string>());

                    clientApplicationTypes.Should().Contain("native-mobile");
                    clientApplicationTypes.Should().NotContain("native-desktop");
                    clientApplicationTypes.Should().Contain("browser-based");
                    clientApplicationTypes.Should().HaveCount(2);

                    var browsersSupported = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("BrowsersSupported").Select(s => s.Value<string>());

                    browsersSupported.Should().NotContain("Edge");
                    browsersSupported.Should().NotContain("Mozilla Firefox");
                    browsersSupported.Should().HaveCount(0);
                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication)
                        .SelectToken("MobileResponsive").Should().BeNullOrEmpty();
                });

            await _context.UpdateSolutionBrowsersSupportedHandler.Handle(new UpdateSolutionBrowsersSupportedCommand("Sln1",
                new UpdateSolutionBrowsersSupportedViewModel
                {
                    BrowsersSupported = new HashSet<string>(),
                }), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdateEmptySolutionBrowsersSupportedAndClientApplicationTypesRemainEmpty()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.ClientApplication).Returns(
                "{ 'ClientApplicationTypes' : [ ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false }");
            var calledBack = false;
            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);
            _context.MockMarketingDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;

                    var clientApplicationTypes = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication)
                        .SelectToken("ClientApplicationTypes").Select(s => s.Value<string>());

                    clientApplicationTypes.Should().NotContain("native-mobile");
                    clientApplicationTypes.Should().NotContain("native-desktop");
                    clientApplicationTypes.Should().NotContain("browser-based");
                    clientApplicationTypes.Should().HaveCount(0);

                    var browsersSupported = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("BrowsersSupported").Select(s => s.Value<string>());

                    browsersSupported.Should().NotContain("Edge");
                    browsersSupported.Should().NotContain("Mozilla Firefox");
                    browsersSupported.Should().HaveCount(0);
                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication)
                        .SelectToken("MobileResponsive").Should().BeNullOrEmpty();
                });

            await _context.UpdateSolutionBrowsersSupportedHandler.Handle(new UpdateSolutionBrowsersSupportedCommand("Sln1",
                new UpdateSolutionBrowsersSupportedViewModel
                {
                    BrowsersSupported = new HashSet<string>(),
                }), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
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
