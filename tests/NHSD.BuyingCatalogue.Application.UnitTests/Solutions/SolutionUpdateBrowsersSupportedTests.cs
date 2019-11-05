using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionBrowsersSupported;
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

            var validationResult = await UpdateBrowsersSupported(new HashSet<string> { "Edge", "Google Chrome" }, "yes");
            validationResult.IsValid.Should().BeTrue();

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

            var validationResult = await UpdateBrowsersSupported(new HashSet<string> { "Edge", "Google Chrome" }, "yes");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotUpdateEmptySolutionBrowsersSupported()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateBrowsersSupported(new HashSet<string>());
            validationResult.IsValid.Should().BeFalse();
            validationResult.Required.Should().BeEquivalentTo(new[] {"browsers-supported", "mobile-responsive"});

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());

            Context.MockMarketingDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateBrowsersSupported(new HashSet<string> { "Edge", "Google Chrome" }, "yes"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        private async Task<UpdateSolutionBrowserSupportedValidationResult> UpdateBrowsersSupported(HashSet<string> browsersSupported, string mobileResponsive = null)
        {
            return await Context.UpdateSolutionBrowsersSupportedHandler.Handle(new UpdateSolutionBrowsersSupportedCommand("Sln1", new UpdateSolutionBrowsersSupportedViewModel()
            {
                BrowsersSupported = browsersSupported,
                MobileResponsive = mobileResponsive
            }), new CancellationToken());
        }
    }
}
