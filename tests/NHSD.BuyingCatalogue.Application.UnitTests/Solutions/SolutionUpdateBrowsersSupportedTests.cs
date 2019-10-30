using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Application.UnitTests.Tools;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NUnit.Framework;


namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionUpdateBrowsersSupportedTests : ClientApplicationTestsBase
    {

        [Test]
        public async Task ShouldUpdateSolutionBrowsersSupported()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            await UpdateBrowsersSupported(new HashSet<string> { "Edge", "Google Chrome" }, "yes");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
            Context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.Id == "Sln1"
                && JToken.Parse(r.ClientApplication).ReadStringArray("BrowsersSupported").ShouldContainOnly(new List<string> { "Edge", "Google Chrome" }).Count() == 2
                && JToken.Parse(r.ClientApplication).SelectToken("MobileResponsive").Value<bool>() == true
            ), It.IsAny<CancellationToken>()), Times.Once());

        }

        [Test]
        public async Task ShouldUpdateSolutionBrowsersSupportedAndNothingElse()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false }");

            var calledBack = false;

            Context.MockMarketingDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                    json.ReadStringArray("ClientApplicationTypes").ShouldContainOnly(new List<string> { "native-mobile", "browser-based" });
                    json.ReadStringArray("BrowsersSupported").ShouldContainOnly(new List<string> { "Google Chrome", "Edge" });
                    json.SelectToken("MobileResponsive").Value<bool>().Should().BeTrue();
                });

            await UpdateBrowsersSupported(new HashSet<string> { "Edge", "Google Chrome" }, "yes");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdateEmptySolutionBrowsersSupported()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            await UpdateBrowsersSupported(new HashSet<string>());

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.Id == "Sln1"
                && !JToken.Parse(r.ClientApplication).ReadStringArray("BrowsersSupported").Any()
                && JToken.Parse(r.ClientApplication).SelectToken("MobileResponsive").Value<bool?>() == null
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldUpdateEmptySolutionBrowsersSupportedAndNothingElse()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false }");
            var calledBack = false;

            Context.MockMarketingDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                    json.ReadStringArray("ClientApplicationTypes").ShouldContainOnly(new List<string> { "native-mobile", "browser-based" });
                    json.ReadStringArray("BrowsersSupported").Should().BeEmpty();
                    json.SelectToken("MobileResponsive").Should().BeNullOrEmpty();
                });

            await UpdateBrowsersSupported(new HashSet<string>());

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdateEmptySolutionBrowsersSupportedAndClientApplicationTypesRemainEmpty()
        {
            SetUpMockSolutionRepositoryGetByIdAsync(
                "{ 'ClientApplicationTypes' : [ ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false }");
            var calledBack = false;
            Context.MockMarketingDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                    json.ReadStringArray("ClientApplicationTypes").Should().BeEmpty();
                    json.ReadStringArray("BrowsersSupported").Should().BeEmpty();
                    json.SelectToken("MobileResponsive").Should().BeNullOrEmpty();
                });

            await UpdateBrowsersSupported(new HashSet<string>());

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateBrowsersSupported(new HashSet<string> { "Edge", "Google Chrome" }));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        private async Task UpdateBrowsersSupported(HashSet<string> browsersSupported, string mobileResponsive = null)
        {
            UpdateSolutionBrowsersSupportedViewModel viewModel = new UpdateSolutionBrowsersSupportedViewModel { BrowsersSupported = browsersSupported };
            if (mobileResponsive != null)
            {
                viewModel.MobileResponsive = mobileResponsive;
            }

            await Context.UpdateSolutionBrowsersSupportedHandler.Handle(new UpdateSolutionBrowsersSupportedCommand("Sln1", viewModel), new CancellationToken());
        }
    }
}
