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
using NHSD.BuyingCatalogue.Application.UnitTests.Tools;
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
            var existingSolution = SetUpSimpleSolution();

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            await UpdateClientApplicationTypes(new HashSet<string>
            {
                "browser-based",
                "native-mobile",
            });

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
            var existingSolution = SetUpSimpleSolution();
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

                    var clientApplicationTypes = GetClientApplicationTypesFromJsonString(updateSolutionClientApplicationRequest.ClientApplication);
                    clientApplicationTypes.ShouldContainOnly(new List<string> {"native-mobile","native-desktop"});
                    clientApplicationTypes.ShouldNotContain("browser-based");

                    var browsersSupported =
                        GetBrowsersSupportedFromJsonString(updateSolutionClientApplicationRequest.ClientApplication);
                    browsersSupported.ShouldContainOnly(new List<string> {"Chrome", "Edge"});

                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("MobileResponsive").Value<bool>().Should().BeTrue();
                });

           await UpdateClientApplicationTypes(new HashSet<string>
            {
                "native-desktop",
                "native-mobile",
            });

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdateEmptySolutionClientApplicationTypes()
        {
            var existingSolution = SetUpSimpleSolution();

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            await UpdateClientApplicationTypes(new HashSet<string> { });

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
            var existingSolution = SetUpSimpleSolution();
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

                    var clientApplicationTypes =
                        GetClientApplicationTypesFromJsonString(
                            updateSolutionClientApplicationRequest.ClientApplication);

                    clientApplicationTypes.Should().BeEmpty();

                    var browsersSupported =
                        GetBrowsersSupportedFromJsonString(updateSolutionClientApplicationRequest.ClientApplication);

                    browsersSupported.ShouldContainOnly(new List<string> { "Chrome", "Edge" });

                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("MobileResponsive").Value<bool>().Should().BeTrue();
                });

            await UpdateClientApplicationTypes(new HashSet<string> { });

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypes()
        {
            var existingSolution = SetUpSimpleSolution();

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

            await UpdateClientApplicationTypes(new HashSet<string>
            {
                "browser-based",
                "curry",
                "native-mobile",
                "native-desktop",
                "elephant",
                "anteater",
                "blue",
                null,
                ""
            });

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.Id == "Sln1"
                && GetClientApplicationTypesFromJsonString(r.ClientApplication).ShouldContainOnly(new List<string> {"browser-based","native-mobile","native-desktop"}).Count()==3
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypesAndNotChangeAnythingElse()
        {
            var existingSolution = SetUpSimpleSolution();
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

                    var clientApplicationTypes = GetClientApplicationTypesFromJsonString(updateSolutionClientApplicationRequest.ClientApplication);

                    clientApplicationTypes.ShouldContainOnly(new List<string>
                    {
                        "native-mobile", "native-desktop", "browser-based"
                    });

                    var browsersSupported = GetBrowsersSupportedFromJsonString(updateSolutionClientApplicationRequest.ClientApplication);
                    browsersSupported.ShouldContainOnly(new List<string> {"Chrome", "Edge"});

                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("MobileResponsive").Value<bool>().Should().BeTrue();
                });

            await UpdateClientApplicationTypes(new HashSet<string>
            {
                "browser-based",
                "curry",
                "native-mobile",
                "native-desktop",
                "elephant",
                "anteater",
                "blue",
                null,
                ""
            });

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypesAndBrowsersSupportedRemainEmpty()
        {
            var existingSolution = SetUpSimpleSolution();
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

                    var clientApplicationTypes = GetClientApplicationTypesFromJsonString(
                            updateSolutionClientApplicationRequest.ClientApplication);

                    clientApplicationTypes.ShouldContainOnly(new List<string> {"native-mobile", "native-desktop", "browser-based"})
                        .ShouldNotContainAnyOf(new List<string>{"curry", "elephant","anteater","blue", ""});

                    var browsersSupported = GetBrowsersSupportedFromJsonString(updateSolutionClientApplicationRequest.ClientApplication);
                    browsersSupported.Should().BeEmpty();

                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication)
                        .SelectToken("MobileResponsive").Should().BeNullOrEmpty();
                });

           await UpdateClientApplicationTypes(new HashSet<string>
            {
                "browser-based",
                "curry",
                "native-mobile",
                "native-desktop",
                "elephant",
                "anteater",
                "blue",
                null,
                ""
            });

           _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
             Assert.ThrowsAsync<NotFoundException>(() =>
                 UpdateClientApplicationTypes(new HashSet<string>
                {
                    "browser-based",
                    "native-mobile",
                }));
            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        private async Task UpdateClientApplicationTypes(HashSet<string> clientApplicationTypes)
        {
                await _context.UpdateSolutionClientApplicationTypesHandler.Handle(new UpdateSolutionClientApplicationTypesCommand("Sln1",
                new UpdateSolutionClientApplicationTypesViewModel
                {
                    ClientApplicationTypes = clientApplicationTypes
                }), new CancellationToken());

        }

        private IEnumerable<string> GetClientApplicationTypesFromJsonString (string json)
        {
            return JToken.Parse(json).SelectToken("ClientApplicationTypes")
                .Select(s => s.Value<string>()).ToList();
        }

        private IEnumerable<string> GetBrowsersSupportedFromJsonString(string json)
        {
            return JToken.Parse(json).SelectToken("BrowsersSupported").Select(s => s.Value<string>()).ToList();
        }

        private Mock<ISolutionResult> SetUpSimpleSolution()
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");
            return existingSolution;
        }
    }
}
