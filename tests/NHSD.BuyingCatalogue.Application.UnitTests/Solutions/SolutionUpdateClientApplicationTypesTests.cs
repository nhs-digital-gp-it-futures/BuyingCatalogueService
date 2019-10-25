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
    internal sealed class SolutionUpdateClientApplicationTypesTests
    {
        private TestContext _context;

        [SetUp]
        public void SetUpFixture()
        {
            _context = new TestContext();
        }

        [Test]
        public async Task ShouldUpdateSolutionClientApplicationTypes()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            await _context.UpdateSolutionClientApplicationTypesHandler.Handle(new UpdateSolutionClientApplicationTypesCommand("Sln1",
                new UpdateSolutionClientApplicationTypesViewModel
                {
                    ClientApplicationTypes = new HashSet<string> { "browser-based", "native-mobile" }
                }), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.Id == "Sln1"
                && JToken.Parse(r.ClientApplication).SelectToken("ClientApplicationTypes").Select(s => s.Value<string>()).Contains("browser-based")
                && JToken.Parse(r.ClientApplication).SelectToken("ClientApplicationTypes").Select(s => s.Value<string>()).Contains("native-mobile")
                && !JToken.Parse(r.ClientApplication).SelectToken("ClientApplicationTypes").Select(s => s.Value<string>()).Contains("native-desktop")
                && JToken.Parse(r.ClientApplication).SelectToken("ClientApplicationTypes").Count() == 2
                ), It.IsAny<CancellationToken>()), Times.Once());
        }
        [Test]
        public async Task ShouldUpdateSolutionClientApplicationTypesAndNothingElse()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.ClientApplication).Returns(
                "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true }");

            var calledBack = false;

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            // verification done in a callback
            _context.MockMarketingDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;

                    var clientApplicationTypes = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication)
                        .SelectToken("ClientApplicationTypes").Select(s => s.Value<string>());

                    clientApplicationTypes.Should().Contain("native-mobile");
                    clientApplicationTypes.Should().Contain("native-desktop");
                    clientApplicationTypes.Should().NotContain("browser-based");
                    clientApplicationTypes.Should().HaveCount(2);

                    var browsersSupported = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("BrowsersSupported").Select(s => s.Value<string>());

                    browsersSupported.Should().Contain("Chrome");
                    browsersSupported.Should().Contain("Edge");
                    browsersSupported.Should().HaveCount(2);

                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("MobileResponsive").Value<bool>().Should().BeTrue();
                });

            await _context.UpdateSolutionClientApplicationTypesHandler.Handle(new UpdateSolutionClientApplicationTypesCommand("Sln1",
                new UpdateSolutionClientApplicationTypesViewModel
                {
                    ClientApplicationTypes = new HashSet<string> { "native-desktop", "native-mobile" }
                }), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdateEmptySolutionClientApplicationTypes()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            await _context.UpdateSolutionClientApplicationTypesHandler.Handle(new UpdateSolutionClientApplicationTypesCommand("Sln1",
                new UpdateSolutionClientApplicationTypesViewModel
                {
                    ClientApplicationTypes = new HashSet<string>()
                }), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.Id == "Sln1"
                && !JToken.Parse(r.ClientApplication).SelectToken("ClientApplicationTypes").Select(s => s.Value<string>()).Contains("browser-based")
                && !JToken.Parse(r.ClientApplication).SelectToken("ClientApplicationTypes").Select(s => s.Value<string>()).Contains("native-mobile")
                && !JToken.Parse(r.ClientApplication).SelectToken("ClientApplicationTypes").Select(s => s.Value<string>()).Contains("native-desktop")
                && JToken.Parse(r.ClientApplication).SelectToken("ClientApplicationTypes").Count() == 0
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldUpdateEmptySolutionClientApplicationTypesAndNothingElse()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.ClientApplication).Returns(
                "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true }");

            var calledBack = false;

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            // verification done in a callback
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

                    browsersSupported.Should().Contain("Chrome");
                    browsersSupported.Should().Contain("Edge");
                    browsersSupported.Should().HaveCount(2);

                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("MobileResponsive").Value<bool>().Should().BeTrue();
                });

            await _context.UpdateSolutionClientApplicationTypesHandler.Handle(new UpdateSolutionClientApplicationTypesCommand("Sln1",
                new UpdateSolutionClientApplicationTypesViewModel
                {
                    ClientApplicationTypes = new HashSet<string>()
                }), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypes()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            await _context.UpdateSolutionClientApplicationTypesHandler.Handle(new UpdateSolutionClientApplicationTypesCommand("Sln1",
                new UpdateSolutionClientApplicationTypesViewModel
                {
                    ClientApplicationTypes = new HashSet<string> { "browser-based", "curry", "native-mobile", "native-desktop", "elephant", "anteater", "blue", null, "" }
                }), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.Id == "Sln1"
                && JToken.Parse(r.ClientApplication).SelectToken("ClientApplicationTypes").Select(s => s.Value<string>()).Contains("browser-based")
                && JToken.Parse(r.ClientApplication).SelectToken("ClientApplicationTypes").Select(s => s.Value<string>()).Contains("native-mobile")
                && JToken.Parse(r.ClientApplication).SelectToken("ClientApplicationTypes").Select(s => s.Value<string>()).Contains("native-desktop")
                && !JToken.Parse(r.ClientApplication).SelectToken("ClientApplicationTypes").Select(s => s.Value<string>()).Contains("curry")
                && !JToken.Parse(r.ClientApplication).SelectToken("ClientApplicationTypes").Select(s => s.Value<string>()).Contains("elephant")
                && !JToken.Parse(r.ClientApplication).SelectToken("ClientApplicationTypes").Select(s => s.Value<string>()).Contains("anteater")
                && !JToken.Parse(r.ClientApplication).SelectToken("ClientApplicationTypes").Select(s => s.Value<string>()).Contains("blue")
                && JToken.Parse(r.ClientApplication).SelectToken("ClientApplicationTypes").Count() == 3
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypesAndNotChangeAnythingElse()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.ClientApplication).Returns(
                "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true }");

            var calledBack = false;

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            // verification done in a callback
            _context.MockMarketingDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;

                    var clientApplicationTypes = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication)
                        .SelectToken("ClientApplicationTypes").Select(s => s.Value<string>());

                    clientApplicationTypes.Should().Contain("native-mobile");
                    clientApplicationTypes.Should().Contain("native-desktop");
                    clientApplicationTypes.Should().Contain("browser-based");
                    clientApplicationTypes.Should().NotContain("curry");
                    clientApplicationTypes.Should().NotContain("elephant");
                    clientApplicationTypes.Should().NotContain("anteater");
                    clientApplicationTypes.Should().NotContain("blue");
                    clientApplicationTypes.Should().NotContain("");
                    clientApplicationTypes.Should().HaveCount(3);

                    var browsersSupported = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("BrowsersSupported").Select(s => s.Value<string>());

                    browsersSupported.Should().Contain("Chrome");
                    browsersSupported.Should().Contain("Edge");
                    browsersSupported.Should().HaveCount(2);

                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("MobileResponsive").Value<bool>().Should().BeTrue();
                });

            await _context.UpdateSolutionClientApplicationTypesHandler.Handle(new UpdateSolutionClientApplicationTypesCommand("Sln1",
                new UpdateSolutionClientApplicationTypesViewModel
                {
                    ClientApplicationTypes = new HashSet<string> { "browser-based", "curry", "native-mobile", "native-desktop", "elephant", "anteater", "blue", null, "" }
                }), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypesAndBrowsersSupportedRemainEmpty()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            existingSolution.Setup(s => s.ClientApplication).Returns(
                "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ ]}");

            var calledBack = false;

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            // verification done in a callback
            _context.MockMarketingDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;

                    var clientApplicationTypes = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication)
                        .SelectToken("ClientApplicationTypes").Select(s => s.Value<string>());

                    clientApplicationTypes.Should().Contain("native-mobile");
                    clientApplicationTypes.Should().Contain("native-desktop");
                    clientApplicationTypes.Should().Contain("browser-based");
                    clientApplicationTypes.Should().NotContain("curry");
                    clientApplicationTypes.Should().NotContain("elephant");
                    clientApplicationTypes.Should().NotContain("anteater");
                    clientApplicationTypes.Should().NotContain("blue");
                    clientApplicationTypes.Should().NotContain("");
                    clientApplicationTypes.Should().HaveCount(3);

                    var browsersSupported = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("BrowsersSupported").Select(s => s.Value<string>());

                    browsersSupported.Should().NotContain("Chrome");
                    browsersSupported.Should().NotContain("Edge");
                    browsersSupported.Should().HaveCount(0);


                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication)
                        .SelectToken("MobileResponsive").Should().BeNullOrEmpty();
                });

            await _context.UpdateSolutionClientApplicationTypesHandler.Handle(new UpdateSolutionClientApplicationTypesCommand("Sln1",
                new UpdateSolutionClientApplicationTypesViewModel
                {
                    ClientApplicationTypes = new HashSet<string> { "browser-based", "curry", "native-mobile", "native-desktop", "elephant", "anteater", "blue", null, "" }
                }), new CancellationToken());

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            var exception = Assert.ThrowsAsync<NotFoundException>(() =>
                _context.UpdateSolutionClientApplicationTypesHandler.Handle(new UpdateSolutionClientApplicationTypesCommand("Sln1",
                    new UpdateSolutionClientApplicationTypesViewModel
                    {
                        ClientApplicationTypes = new HashSet<string> { "browser-based", "native-mobile" }
                    }), new CancellationToken()));

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}
