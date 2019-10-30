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
    internal sealed class SolutionUpdateClientApplicationTypesTests : ClientApplicationTestsBase
    {

        [Test]
        public async Task ShouldUpdateSolutionClientApplicationTypes()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            await UpdateClientApplicationTypes(new HashSet<string> { "browser-based", "native-mobile", });

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.Id == "Sln1"
                && JTokenExtension.SelectStringValues("ClientApplicationTypes", r.ClientApplication).ShouldContainOnly(new List<string> { "browser-based", "native-mobile" }).Count() == 2
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldUpdateSolutionClientApplicationTypesAndNothingElse()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true }");
            var calledBack = false;

            // verification done in a callback
            Context.MockMarketingDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    JTokenExtension.SelectStringValues("ClientApplicationTypes", updateSolutionClientApplicationRequest.ClientApplication)
                        .ShouldContainOnly(new List<string> { "native-mobile", "native-desktop" })
                        .ShouldNotContain("browser-based");

                    JTokenExtension.SelectStringValues("BrowsersSupported", updateSolutionClientApplicationRequest.ClientApplication)
                        .ShouldContainOnly(new List<string> { "Chrome", "Edge" });

                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("MobileResponsive").Value<bool>()
                        .Should().BeTrue();
                });

            await UpdateClientApplicationTypes(new HashSet<string> { "native-desktop", "native-mobile" });

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdateEmptySolutionClientApplicationTypes()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            await UpdateClientApplicationTypes(new HashSet<string>());

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.Id == "Sln1"
                && !JTokenExtension.SelectStringValues("ClientApplicationTypes", r.ClientApplication).Any()
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldUpdateEmptySolutionClientApplicationTypesAndNothingElse()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true }");
            var calledBack = false;

            // verification done in a callback
            Context.MockMarketingDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;

                    JTokenExtension.SelectStringValues("ClientApplicationTypes",
                            updateSolutionClientApplicationRequest.ClientApplication)
                        .Should().BeEmpty();

                    JTokenExtension.SelectStringValues("BrowsersSupported", updateSolutionClientApplicationRequest.ClientApplication)
                        .ShouldContainOnly(new List<string> { "Chrome", "Edge" });

                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("MobileResponsive").Value<bool>()
                        .Should().BeTrue();
                });

            await UpdateClientApplicationTypes(new HashSet<string>());

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypes()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            await UpdateClientApplicationTypes(new HashSet<string> { "browser-based", "curry", "native-mobile", "native-desktop", "elephant", "anteater", "blue", null, "" });

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.Id == "Sln1"
                && JTokenExtension.SelectStringValues("ClientApplicationTypes", r.ClientApplication).ShouldContainOnly(new List<string> { "browser-based", "native-mobile", "native-desktop" }).Count() == 3
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypesAndNotChangeAnythingElse()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true }");

            var calledBack = false;

            // verification done in a callback
            Context.MockMarketingDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;

                    JTokenExtension.SelectStringValues("ClientApplicationTypes", updateSolutionClientApplicationRequest.ClientApplication)
                        .ShouldContainOnly(new List<string> { "native-mobile", "native-desktop", "browser-based" });

                    JTokenExtension.SelectStringValues("BrowsersSupported", updateSolutionClientApplicationRequest.ClientApplication)
                        .ShouldContainOnly(new List<string> { "Chrome", "Edge" });

                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("MobileResponsive").Value<bool>()
                        .Should().BeTrue();
                });

            await UpdateClientApplicationTypes(new HashSet<string> { "browser-based", "curry", "native-mobile", "native-desktop", "elephant", "anteater", "blue", null, "" });

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypesAndBrowsersSupportedRemainEmpty()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ ]}");
            var calledBack = false;

            // verification done in a callback
            Context.MockMarketingDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;

                    JTokenExtension.SelectStringValues("ClientApplicationTypes", updateSolutionClientApplicationRequest.ClientApplication)
                        .ShouldContainOnly(new List<string> { "native-mobile", "native-desktop", "browser-based" })
                        .ShouldNotContainAnyOf(new List<string> { "curry", "elephant", "anteater", "blue", "" });

                    JTokenExtension.SelectStringValues("BrowsersSupported", updateSolutionClientApplicationRequest.ClientApplication)
                        .Should().BeEmpty();

                    JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication).SelectToken("MobileResponsive")
                        .Should().BeNullOrEmpty();
                });

            await UpdateClientApplicationTypes(new HashSet<string> { "browser-based", "curry", "native-mobile", "native-desktop", "elephant", "anteater", "blue", null, "" });

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() =>
                UpdateClientApplicationTypes(new HashSet<string> { "browser-based", "native-mobile" }));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockMarketingDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        private async Task UpdateClientApplicationTypes(HashSet<string> clientApplicationTypes)
        {
            await Context.UpdateSolutionClientApplicationTypesHandler.Handle(new UpdateSolutionClientApplicationTypesCommand("Sln1",
                new UpdateSolutionClientApplicationTypesViewModel
                {
                    ClientApplicationTypes = clientApplicationTypes
                }), new CancellationToken());

        }
    }
}
