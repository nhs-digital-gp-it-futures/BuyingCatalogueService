using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowserMobileFirst;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.BrowserBased
{
    [TestFixture]
    internal sealed class SolutionUpdateBrowserMobileFirstTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateBrowserMobileFirst()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateBrowserMobileFirst("yes").ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("MobileFirstDesign").Value<bool>() == true
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldNotUpdateEmptyMobileFirst()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateBrowserMobileFirst().ConfigureAwait(false);
            validationResult.IsValid.Should().BeFalse();
            validationResult.ToDictionary()["mobile-first-design"].Should().Be("required");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public async Task ShouldUpdateSolutionBrowserMobileFirstAndNothingElse()
        {
            SetUpMockSolutionRepositoryGetByIdAsync(
                "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false, 'Plugins' : {'Required' : true, 'AdditionalInformation': 'orem ipsum' }, 'HardwareRequirements': 'New Hardware', 'AdditionalInformation': 'New Additional Info', 'MobileFirstDesign': true }");

            var calledBack = false;

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest,
                    CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                    json.SelectToken("MobileFirstDesign").Value<bool>().Should().BeFalse();

                    json.ReadStringArray("ClientApplicationTypes")
                        .ShouldContainOnly(new List<string> { "browser-based", "native-mobile" });
                    json.ReadStringArray("BrowsersSupported")
                        .ShouldContainOnly(new List<string> { "Mozilla Firefox", "Edge" });
                    json.SelectToken("MobileResponsive").Value<bool>()
                        .Should().BeFalse();
                    json.SelectToken("Plugins.Required").Value<bool>().Should().BeTrue();
                    json.SelectToken("Plugins.AdditionalInformation").Value<string>().Should().Be("orem ipsum");
                    json.SelectToken("HardwareRequirements").Value<string>().Should()
                        .Be("New Hardware");
                    json.SelectToken("AdditionalInformation").Value<string>().Should()
                        .Be("New Additional Info");
                });

            var validationResult = await UpdateBrowserMobileFirst("no").ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateBrowserMobileFirst("yes"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public void CommandShouldTrimStrings()
        {
            var originalViewModel = new UpdateBrowserBasedMobileFirstViewModel { MobileFirstDesign = "     yes     "};
            var command = new UpdateSolutionBrowserMobileFirstCommand("Sln1", originalViewModel.MobileFirstDesign);
            command.MobileFirstDesign.Should().Be("yes");
        }

        private async Task<ISimpleResult> UpdateBrowserMobileFirst(
            string mobileFirstDesign = null)
        {
            return await Context.UpdateSolutionBrowserMobileFirstHandler
                .Handle(
                    new UpdateSolutionBrowserMobileFirstCommand(SolutionId, mobileFirstDesign),
                    new CancellationToken()).ConfigureAwait(false);
        }
    }
}
