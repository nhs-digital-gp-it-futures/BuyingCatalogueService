using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileThirdParty;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.ClientApplications.NativeMobile
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

            var validationResult = await UpdateMobileThirdParty("component", "capabilities");
            validationResult.IsValid.Should().BeTrue();
            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken(ThirdPartyToken).Value<string>() == "component"
                && JToken.Parse(r.ClientApplication).SelectToken(DeviceToken).Value<string>() == "capabilities";

            Context.MockSolutionRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateNull()
        {
            const string clientApplicationJson = "{'ClientApplicationTypes' : [ 'native-mobile' ], "
                + "'MobileThirdParty': { 'ThirdPartyComponents': 'component', 'DeviceCapabilities': 'capabilities' } }";

            SetUpMockSolutionRepositoryGetByIdAsync(clientApplicationJson);

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest request, CancellationToken token)
            {
                calledBack = true;
                var json = JToken.Parse(request.ClientApplication);

                json.SelectToken(ThirdPartyToken).Should().BeNullOrEmpty();
                json.SelectToken(DeviceToken).Should().BeNullOrEmpty();
            }

            Context.MockSolutionRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateMobileThirdParty();
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotUpdateThirdPartyOverCharacterLimit()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateMobileThirdParty(new string('a', 501));
            validationResult.IsValid.Should().BeFalse();

            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["third-party-components"].Should().Be("maxLength");

            Context.MockSolutionRepository.Verify(
                r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()),
                Times.Never());

            Expression<Func<ISolutionRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionRepository.Verify(expression, Times.Never());
        }

        [Test]
        public async Task ShouldNotUpdateDeviceOverCharacterLimit()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateMobileThirdParty(null, new string('a', 501));
            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["device-capabilities"].Should().Be("maxLength");

            Context.MockSolutionRepository.Verify(
                r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()),
                Times.Never());

            Expression<Func<ISolutionRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionRepository.Verify(expression, Times.Never());
        }

        [Test]
        public async Task ShouldNotUpdateBothVariablesInvalid()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateMobileThirdParty(new string('a', 501), new string('a', 501));
            validationResult.IsValid.Should().BeFalse();

            var results = validationResult.ToDictionary();
            results.Count.Should().Be(2);
            results["third-party-components"].Should().Be("maxLength");
            results["device-capabilities"].Should().Be("maxLength");

            Context.MockSolutionRepository.Verify(
                r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()),
                Times.Never());

            Expression<Func<ISolutionRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionRepository.Verify(expression, Times.Never());
        }

        private async Task<ISimpleResult> UpdateMobileThirdParty(
            string thirdPartyComponents = null,
            string deviceCapabilities = null)
        {
            Expression<Func<IUpdateNativeMobileThirdPartyData, bool>> updateNativeMobileThirdPartyData = t =>
                t.ThirdPartyComponents == thirdPartyComponents
                && t.DeviceCapabilities == deviceCapabilities;

            var data = Mock.Of(updateNativeMobileThirdPartyData);

            return await Context.UpdateSolutionMobileThirdPartyHandler.Handle(
                new UpdateSolutionMobileThirdPartyCommand(SolutionId, data),
                CancellationToken.None);
        }
    }
}
