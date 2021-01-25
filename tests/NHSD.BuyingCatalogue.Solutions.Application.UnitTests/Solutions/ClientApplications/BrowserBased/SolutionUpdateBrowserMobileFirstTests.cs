using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionBrowserMobileFirst;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.ClientApplications.BrowserBased
{
    [TestFixture]
    internal sealed class SolutionUpdateBrowserMobileFirstTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateBrowserMobileFirst()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateBrowserMobileFirst("yes");
            validationResult.IsValid.Should().BeTrue();

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("MobileFirstDesign").Value<bool>();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));
            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldNotUpdateEmptyMobileFirst()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateBrowserMobileFirst();
            validationResult.IsValid.Should().BeFalse();
            validationResult.ToDictionary()["mobile-first-design"].Should().Be("required");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());

            Expression<Func<ISolutionDetailRepository, Task>> expression = r =>
                r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(expression, Times.Never());
        }

        [Test]
        public async Task ShouldUpdateSolutionBrowserMobileFirstAndNothingElse()
        {
            const string clientApplicationJson = "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], "
                + "'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], "
                + "'MobileResponsive': false, "
                + "'Plugins' : {'Required' : true, 'AdditionalInformation': 'lorem ipsum' }, "
                + "'HardwareRequirements': 'New Hardware', "
                + "'AdditionalInformation': 'New Additional Info', "
                + "'MobileFirstDesign': true }";

            SetUpMockSolutionRepositoryGetByIdAsync(clientApplicationJson);

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken _)
            {
                calledBack = true;
                var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                json.SelectToken("MobileFirstDesign")?.Value<bool>().Should().BeFalse();

                json.ReadStringArray("ClientApplicationTypes")
                    .ShouldContainOnly(new List<string> { "browser-based", "native-mobile" });

                json.ReadStringArray("BrowsersSupported")
                    .ShouldContainOnly(new List<string> { "Mozilla Firefox", "Edge" });

                json.SelectToken("MobileResponsive")?.Value<bool>().Should().BeFalse();
                json.SelectToken("Plugins.Required")?.Value<bool>().Should().BeTrue();
                json.SelectToken("Plugins.AdditionalInformation")?.Value<string>().Should().Be("lorem ipsum");
                json.SelectToken("HardwareRequirements")?.Value<string>().Should().Be("New Hardware");
                json.SelectToken("AdditionalInformation")?.Value<string>().Should().Be("New Additional Info");
            }

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateBrowserMobileFirst("no");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateBrowserMobileFirst("yes"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        private async Task<ISimpleResult> UpdateBrowserMobileFirst(string mobileFirstDesign = null)
        {
            return await Context.UpdateSolutionBrowserMobileFirstHandler.Handle(
                new UpdateSolutionBrowserMobileFirstCommand(SolutionId, mobileFirstDesign),
                CancellationToken.None);
        }
    }
}
