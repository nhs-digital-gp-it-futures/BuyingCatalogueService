using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionClientApplicationTypes;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionUpdateClientApplicationTypesTests : ClientApplicationTestsBase
    {

        [Test]
        public async Task ShouldUpdateSolutionClientApplicationTypes()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateClientApplicationTypes(new HashSet<string> { "browser-based", "native-mobile", })
                .ConfigureAwait(false);
            validationResult.IsValid.Should().Be(true);

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == "Sln1"
                && JToken.Parse(r.ClientApplication).ReadStringArray("ClientApplicationTypes").ShouldContainOnly(new List<string> { "browser-based", "native-mobile" }).Count() == 2
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldUpdateSolutionClientApplicationTypesAndNothingElse()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true }");
            var calledBack = false;

            // verification done in a callback
            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                    json.ReadStringArray("ClientApplicationTypes")
                        .ShouldContainOnly(new List<string> { "native-mobile", "native-desktop" })
                        .ShouldNotContain("browser-based");
                    json.ReadStringArray("BrowsersSupported")
                        .ShouldContainOnly(new List<string> { "Chrome", "Edge" });
                    json.SelectToken("MobileResponsive").Value<bool>()
                        .Should().BeTrue();
                });

            var validationResult = await UpdateClientApplicationTypes(new HashSet<string> { "native-desktop", "native-mobile" })
                .ConfigureAwait(false);
            validationResult.IsValid.Should().Be(true);

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldFailValidationUpdateEmptySolutionClientApplicationTypes()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateClientApplicationTypes(new HashSet<string>())
                .ConfigureAwait(false);
            validationResult.IsValid.Should().Be(false);
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["client-application-types"].Should().Be("required");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never);

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypes()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateClientApplicationTypes(new HashSet<string> { "browser-based", "curry", "native-mobile", "native-desktop", "elephant", "anteater", "blue", null, "" })
                .ConfigureAwait(false);
            validationResult.IsValid.Should().Be(true);

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == "Sln1"
                && JToken.Parse(r.ClientApplication).ReadStringArray("ClientApplicationTypes").ShouldContainOnly(new List<string> { "browser-based", "native-mobile", "native-desktop" }).Count() == 3
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypesAndNotChangeAnythingElse()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true }");

            var calledBack = false;

            // verification done in a callback
            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                    json.ReadStringArray("ClientApplicationTypes").ShouldContainOnly(new List<string> { "native-mobile", "native-desktop", "browser-based" });
                    json.ReadStringArray("BrowsersSupported").ShouldContainOnly(new List<string> { "Chrome", "Edge" });
                    json.SelectToken("MobileResponsive").Value<bool>().Should().BeTrue();
                });

            var validationResult = await UpdateClientApplicationTypes(new HashSet<string> { "browser-based", "curry", "native-mobile", "native-desktop", "elephant", "anteater", "blue", null, "" })
                .ConfigureAwait(false);
            validationResult.IsValid.Should().Be(true);

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypesAndBrowsersSupportedRemainEmpty()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ ]}");
            var calledBack = false;

            // verification done in a callback
            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                    json.ReadStringArray("ClientApplicationTypes")
                        .ShouldContainOnly(new List<string> { "native-mobile", "native-desktop", "browser-based" })
                        .ShouldNotContainAnyOf(new List<string> { "curry", "elephant", "anteater", "blue", "" });
                    json.ReadStringArray("BrowsersSupported").Should().BeEmpty();
                    json.SelectToken("MobileResponsive").Should().BeNullOrEmpty();
                });

            var validationResult = await UpdateClientApplicationTypes(new HashSet<string> { "browser-based", "curry", "native-mobile", "native-desktop", "elephant", "anteater", "blue", null, "" })
                .ConfigureAwait(false);
            validationResult.IsValid.Should().Be(true);

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() =>
                UpdateClientApplicationTypes(new HashSet<string> { "browser-based", "native-mobile" }));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        private async Task<ISimpleResult> UpdateClientApplicationTypes(HashSet<string> clientApplicationTypes)
        {
            return await Context.UpdateSolutionClientApplicationTypesHandler.Handle(new UpdateSolutionClientApplicationTypesCommand("Sln1",
                new UpdateSolutionClientApplicationTypesViewModel
                {
                    ClientApplicationTypes = clientApplicationTypes
                }), new CancellationToken())
                .ConfigureAwait(false);
        }
    }
}
