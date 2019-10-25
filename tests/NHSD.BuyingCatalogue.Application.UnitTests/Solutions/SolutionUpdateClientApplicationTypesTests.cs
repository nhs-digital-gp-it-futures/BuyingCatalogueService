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
            SetUpMockSolutionRepository("{}");

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
            SetUpMockSolutionRepository("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true }");
            var calledBack = false;

            // verification done in a callback
            _context.MockMarketingDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;

                    GetFieldFromJsonString("ClientApplicationTypes", updateSolutionClientApplicationRequest.ClientApplication)
                        .ShouldContainOnly(new List<string> {"native-mobile","native-desktop"})
                        .ShouldNotContain("browser-based");

                    var browsersSupported =
                        GetFieldFromJsonString("BrowsersSupported", updateSolutionClientApplicationRequest.ClientApplication);
                    browsersSupported.ShouldContainOnly(new List<string> {"Chrome", "Edge"});

                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("MobileResponsive").Value<bool>().Should().BeTrue();
                });

           await UpdateClientApplicationTypes(new HashSet<string> {"native-desktop", "native-mobile"});

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdateEmptySolutionClientApplicationTypes()
        {
            SetUpMockSolutionRepository("{}");

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
            SetUpMockSolutionRepository("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true }");
            var calledBack = false;

          
            // verification done in a callback
            _context.MockMarketingDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;

                    GetFieldFromJsonString("ClientApplicationTypes",
                            updateSolutionClientApplicationRequest.ClientApplication).Should().BeEmpty();

                    GetFieldFromJsonString("BrowsersSupported",updateSolutionClientApplicationRequest.ClientApplication).ShouldContainOnly(new List<string> { "Chrome", "Edge" });

                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("MobileResponsive").Value<bool>().Should().BeTrue();
                });

            await UpdateClientApplicationTypes(new HashSet<string> { });

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypes()
        {
            SetUpMockSolutionRepository("{}");

            await UpdateClientApplicationTypes(new HashSet<string> {"browser-based", "curry", "native-mobile", "native-desktop", "elephant", "anteater", "blue", null, ""});

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            _context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.Id == "Sln1"
                && GetFieldFromJsonString("ClientApplicationTypes", r.ClientApplication).ShouldContainOnly(new List<string> {"browser-based","native-mobile","native-desktop"}).Count()==3
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypesAndNotChangeAnythingElse()
        {
            SetUpMockSolutionRepository("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true }");

            var calledBack = false;

            // verification done in a callback
            _context.MockMarketingDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;

                    GetFieldFromJsonString("ClientApplicationTypes", updateSolutionClientApplicationRequest.ClientApplication).ShouldContainOnly(new List<string>
                    {
                        "native-mobile", "native-desktop", "browser-based"
                    });

                    GetFieldFromJsonString("BrowsersSupported",updateSolutionClientApplicationRequest.ClientApplication).ShouldContainOnly(new List<string> {"Chrome", "Edge"});

                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("MobileResponsive").Value<bool>().Should().BeTrue();
                });

            await UpdateClientApplicationTypes(new HashSet<string> { "browser-based", "curry", "native-mobile", "native-desktop", "elephant", "anteater", "blue", null, "" });

            _context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypesAndBrowsersSupportedRemainEmpty()
        {
            SetUpMockSolutionRepository("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ ]}");
            var calledBack = false;


            // verification done in a callback
            _context.MockMarketingDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;

                    GetFieldFromJsonString("ClientApplicationTypes", updateSolutionClientApplicationRequest.ClientApplication)
                        .ShouldContainOnly(new List<string> {"native-mobile", "native-desktop", "browser-based"})
                        .ShouldNotContainAnyOf(new List<string>{"curry", "elephant","anteater","blue", ""});

                    GetFieldFromJsonString("BrowsersSupported",updateSolutionClientApplicationRequest.ClientApplication)
                        .Should().BeEmpty();

                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("MobileResponsive").Should().BeNullOrEmpty();
                });

           await UpdateClientApplicationTypes(new HashSet<string>{ "browser-based", "curry", "native-mobile", "native-desktop", "elephant", "anteater", "blue", null, "" });

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

        private IEnumerable<string> GetFieldFromJsonString(string fieldName, string json)
        {
            return JToken.Parse(json).SelectToken(fieldName).Select(s => s.Value<string>()).ToList();
        }

        private void SetUpMockSolutionRepository(string clientApplicationJson)
        {
            var existingSolution = new Mock<ISolutionResult>();
            existingSolution.Setup(s => s.Id).Returns("Sln1");

            existingSolution.Setup(s => s.ClientApplication).Returns(clientApplicationJson);

            _context.MockSolutionRepository.Setup(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>())).ReturnsAsync(existingSolution.Object);

        }
    }
}
