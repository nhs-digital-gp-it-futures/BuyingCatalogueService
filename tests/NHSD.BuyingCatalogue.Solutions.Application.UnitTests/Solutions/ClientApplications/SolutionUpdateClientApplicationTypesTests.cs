using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.UpdateSolutionClientApplicationTypes;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.ClientApplications
{
    [TestFixture]
    internal sealed class SolutionUpdateClientApplicationTypesTests : ClientApplicationTestsBase
    {
        [Test]
        public async Task ShouldUpdateSolutionClientApplicationTypes()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateClientApplicationTypes(
                new HashSet<string> { "browser-based", "native-mobile" });

            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r => r.SolutionId == "Sln1"
                && JToken.Parse(r.ClientApplication)
                    .ReadStringArray("ClientApplicationTypes")
                    .ShouldContainOnly(new List<string> { "browser-based", "native-mobile" })
                    .Count() == 2;

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateSolutionClientApplicationTypesAndNothingElse()
        {
            const string clientApplicationJson = "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], "
                + "'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true }";

            SetUpMockSolutionRepositoryGetByIdAsync(clientApplicationJson);
            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest request, CancellationToken token)
            {
                calledBack = true;
                var json = JToken.Parse(request.ClientApplication);

                json.ReadStringArray("ClientApplicationTypes")
                    .ShouldContainOnly(new List<string> { "native-mobile", "native-desktop" })
                    .ShouldNotContain("browser-based");

                json.ReadStringArray("BrowsersSupported").ShouldContainOnly(new List<string> { "Chrome", "Edge" });
                json.SelectToken("MobileResponsive")?
                    .Value<bool>()
                    .Should()
                    .BeTrue();
            }

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateClientApplicationTypes(
                new HashSet<string> { "native-desktop", "native-mobile" });

            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldFailValidationUpdateEmptySolutionClientApplicationTypes()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateClientApplicationTypes(new HashSet<string>());
            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["client-application-types"].Should().Be("required");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());

            Expression<Func<ISolutionDetailRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(expression, Times.Never());
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypes()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateClientApplicationTypes(new HashSet<string>
            {
                "browser-based",
                "curry",
                "native-mobile",
                "native-desktop",
                "elephant",
                "anteater",
                "blue",
                null,
                string.Empty,
            });

            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r => r.SolutionId == "Sln1" && JToken
                .Parse(r.ClientApplication)
                .ReadStringArray("ClientApplicationTypes")
                .ShouldContainOnly(new List<string> { "browser-based", "native-mobile", "native-desktop" })
                .Count() == 3;

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypesAndNotChangeAnythingElse()
        {
            const string clientApplicationJson = "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], "
                + "'BrowsersSupported' : [ 'Chrome', 'Edge' ], 'MobileResponsive': true }";

            SetUpMockSolutionRepositoryGetByIdAsync(clientApplicationJson);

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest request, CancellationToken token)
            {
                calledBack = true;
                var json = JToken.Parse(request.ClientApplication);

                json.ReadStringArray("ClientApplicationTypes").ShouldContainOnly(
                    new List<string> { "native-mobile", "native-desktop", "browser-based" });

                json.ReadStringArray("BrowsersSupported").ShouldContainOnly(new List<string> { "Chrome", "Edge" });
                json.SelectToken("MobileResponsive")?.Value<bool>().Should().BeTrue();
            }

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateClientApplicationTypes(new HashSet<string>
            {
                "browser-based",
                "curry",
                "native-mobile",
                "native-desktop",
                "elephant",
                "anteater",
                "blue",
                null,
                string.Empty,
            });

            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldIgnoreUnknownClientApplicationTypesAndBrowsersSupportedRemainEmpty()
        {
            const string clientApplicationTypesBrowserBasedNativeMobileBrowsersSupported =
                "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], "
                + "'BrowsersSupported' : [ ]}";

            SetUpMockSolutionRepositoryGetByIdAsync(clientApplicationTypesBrowserBasedNativeMobileBrowsersSupported);
            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest request, CancellationToken token)
            {
                calledBack = true;
                var json = JToken.Parse(request.ClientApplication);

                var clientAppTypes = json.ReadStringArray("ClientApplicationTypes").ToList();

                clientAppTypes.Should().Contain(new List<string> { "native-mobile", "native-desktop", "browser-based" });
                clientAppTypes.Should().NotContain(new List<string> { "curry", "elephant", "anteater", "blue", string.Empty });

                json.ReadStringArray("BrowsersSupported").Should().BeEmpty();
                json.SelectToken("MobileResponsive").Should().BeNullOrEmpty();
            }

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateClientApplicationTypes(new HashSet<string>
            {
                "browser-based",
                "curry",
                "native-mobile",
                "native-desktop",
                "elephant",
                "anteater",
                "blue",
                null,
                string.Empty,
            });

            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateClientApplicationTypes(
                new HashSet<string> { "browser-based", "native-mobile" }));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));

            Expression<Func<ISolutionDetailRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(expression, Times.Never());
        }

        private async Task<ISimpleResult> UpdateClientApplicationTypes(HashSet<string> clientApplicationTypes)
        {
            var command = new UpdateSolutionClientApplicationTypesCommand(
                "Sln1",
                new UpdateSolutionClientApplicationTypesViewModel(clientApplicationTypes));

            return await Context.UpdateSolutionClientApplicationTypesHandler.Handle(
                command,
                CancellationToken.None);
        }
    }
}
