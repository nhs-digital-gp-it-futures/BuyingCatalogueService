using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionNativeMobileFirst;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.NativeMobile
{
    [TestFixture]
    internal sealed class SolutionUpdateNativeMobileFirstTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateNativeMobileFirst()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeMobileFirst("yes").ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());
            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("NativeMobileFirstDesign").Value<bool>() == true
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldNotUpdateEmptyMobileFirst()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeMobileFirst().ConfigureAwait(false);
            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["mobile-first-design"].Should().Be("required");
            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Never());

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public async Task ShouldUpdateSolutionNativeMobileFirstAndNothingElse()
        {
            SetUpMockSolutionRepositoryGetByIdAsync(
                "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false, 'Plugins' : {'Required' : true, 'AdditionalInformation': 'orem ipsum' }, 'HardwareRequirements': 'New Hardware', 'AdditionalInformation': 'New Additional Info', 'MobileFirstDesign': true, 'NativeMobileFirstDesign': true }");

            var calledBack = false;

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest,
                    CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                    json.ReadStringArray("ClientApplicationTypes").ShouldContainOnly(new List<string> { "browser-based", "native-mobile" });
                    json.ReadStringArray("BrowsersSupported").ShouldContainOnly(new List<string> { "Mozilla Firefox", "Edge" });
                    json.SelectToken("MobileResponsive").Value<bool>().Should().BeFalse();
                    json.SelectToken("Plugins.Required").Value<bool>().Should().BeTrue();
                    json.SelectToken("Plugins.AdditionalInformation").Value<string>().Should().Be("orem ipsum");
                    json.SelectToken("HardwareRequirements").Value<string>().Should().Be("New Hardware");
                    json.SelectToken("AdditionalInformation").Value<string>().Should().Be("New Additional Info");
                    json.SelectToken("MobileFirstDesign").Value<bool>().Should().BeTrue();
                    json.SelectToken("NativeMobileFirstDesign").Value<bool>().Should().BeFalse();
                });

            var validationResult = await UpdateNativeMobileFirst("no").ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateNativeMobileFirst("yes"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        private async Task<ISimpleResult> UpdateNativeMobileFirst(
            string mobileFirstDesign = null)
        {
            return await Context.UpdateSolutionNativeMobileFirstHandler
                .Handle(
                    new UpdateSolutionNativeMobileFirstCommand(SolutionId,
                        new UpdateSolutionNativeMobileFirstViewModel { MobileFirstDesign = mobileFirstDesign }),
                    new CancellationToken()).ConfigureAwait(false);
        }
    }
}
