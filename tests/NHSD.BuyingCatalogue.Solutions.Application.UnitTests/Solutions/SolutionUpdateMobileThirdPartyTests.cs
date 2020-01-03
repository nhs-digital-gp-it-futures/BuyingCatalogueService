using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileThirdParty;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionUpdateMobileThirdPartyTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";
        private const string ThirdPartyToken = "MobileThirdParty.ThirdPartyComponents";
        private const string DeviceToken = "MobileThirdParty.DeviceCapabilities";

        [Test]
        public async Task ShouldUpdateMobileThirdParty()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateMobileThirdParty("component", "capabilities").ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();
            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()),
                Times.Once);
            Context.MockSolutionDetailRepository.Verify(r =>
                r.UpdateClientApplicationAsync(
                    It.Is<IUpdateSolutionClientApplicationRequest>(r => r.SolutionId == SolutionId
                                                                        && JToken.Parse(r.ClientApplication)
                                                                            .SelectToken(ThirdPartyToken)
                                                                            .Value<string>() == "component"
                                                                        && JToken.Parse(r.ClientApplication)
                                                                            .SelectToken(DeviceToken)
                                                                            .Value<string>() == "capabilities"),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateNull()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{'ClientApplicationTypes' : [ 'native-mobile' ], 'MobileThirdParty': { 'ThirdPartyComponents': 'component', 'DeviceCapabilities': 'capabilities' } }");

            var calledBack = false;

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                    json.SelectToken(ThirdPartyToken).Should().BeNullOrEmpty();
                    json.SelectToken(DeviceToken).Should().BeNullOrEmpty();
                });

            var validationResult = await UpdateMobileThirdParty().ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }


        public async Task ShouldNotUpdateThirdPartyOverCharacterLimit()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateMobileThirdParty(new string('a', 501)).ConfigureAwait(false);
            validationResult.IsValid.Should().Be(false);
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["third-party-components"].Should().Be("maxLength");
            
            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Never);
            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task ShouldNotUpdateDeviceOverCharacterLimit()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateMobileThirdParty(null, new string('a', 501)).ConfigureAwait(false);
            validationResult.IsValid.Should().Be(false);
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["device-capabilities"].Should().Be("maxLength");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Never);
            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task ShouldNotUpdateBothVariablesInvalid()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateMobileThirdParty(new string('a', 501), new string('a', 501)).ConfigureAwait(false);
            validationResult.IsValid.Should().Be(false);
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(2);
            results["third-party-components"].Should().Be("maxLength");
            results["device-capabilities"].Should().Be("maxLength");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Never);
            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }


        private async Task<ISimpleResult> UpdateMobileThirdParty(string thirdPartyComponents = null, string deviceCapabilities = null)
        {
            return await Context.UpdateSolutionMobileThirdPartyHandler.Handle(
                new UpdateSolutionMobileThirdPartyCommand(SolutionId,
                    new UpdateSolutionMobileThirdPartyViewModel()
                    {
                        ThirdPartyComponents = thirdPartyComponents, DeviceCapabilities = deviceCapabilities
                    }), CancellationToken.None).ConfigureAwait(false);
        }
    }
}
