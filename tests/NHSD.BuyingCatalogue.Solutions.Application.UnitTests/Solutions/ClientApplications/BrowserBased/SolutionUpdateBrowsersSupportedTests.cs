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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionBrowsersSupported;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.ClientApplications.BrowserBased
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

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r => r.SolutionId == "Sln1"
                && JToken.Parse(r.ClientApplication)
                    .ReadStringArray("BrowsersSupported")
                    .ShouldContainOnly(new List<string> { "Edge", "Google Chrome" })
                    .Count() == 2
                && JToken.Parse(r.ClientApplication).SelectToken("MobileResponsive").Value<bool>();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));
            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateSolutionBrowsersSupportedAndNothingElse()
        {
            const string clientApplicationJson = "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], "
                + "'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], "
                + "'MobileResponsive': false }";

            SetUpMockSolutionRepositoryGetByIdAsync(clientApplicationJson);

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest request, CancellationToken token)
            {
                calledBack = true;
                var json = JToken.Parse(request.ClientApplication);

                json.ReadStringArray("ClientApplicationTypes").ShouldContainOnly(new List<string> { "native-mobile", "browser-based" });
                json.ReadStringArray("BrowsersSupported").ShouldContainOnly(new List<string> { "Google Chrome", "Edge" });
                json.SelectToken("MobileResponsive")?.Value<bool>().Should().BeTrue();
            }

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateBrowsersSupported(new HashSet<string> { "Edge", "Google Chrome" }, "yes");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotUpdateEmptySolutionBrowsersSupported()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateBrowsersSupported(new HashSet<string>());
            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(2);
            results["supported-browsers"].Should().Be("required");
            results["mobile-responsive"].Should().Be("required");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());

            Expression<Func<ISolutionDetailRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(expression, Times.Never());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateBrowsersSupported(new HashSet<string> { "Edge", "Google Chrome" }, "yes"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));

            Expression<Func<ISolutionDetailRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(expression, Times.Never());
        }

        private async Task<ISimpleResult> UpdateBrowsersSupported(HashSet<string> browsersSupported, string mobileResponsive = null)
        {
            var data = Mock.Of<IUpdateBrowserBasedBrowsersSupportedData>(
                t => t.BrowsersSupported == browsersSupported && t.MobileResponsive == mobileResponsive);

            return await Context.UpdateSolutionBrowsersSupportedHandler.Handle(
                new UpdateSolutionBrowsersSupportedCommand("Sln1", data),
                CancellationToken.None);
        }
    }
}
