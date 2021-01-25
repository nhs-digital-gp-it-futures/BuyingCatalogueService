using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionNativeDesktopThirdParty;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.ClientApplications.NativeDesktop
{
    internal sealed class SolutionUpdateNativeDesktopThirdPartyTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateSolutionNativeDesktopThirdParty()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeDesktopThirdParty("New Component", "New Capability");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication)
                    .SelectToken("NativeDesktopThirdParty.ThirdPartyComponents")
                    .Value<string>() == "New Component"
                && JToken.Parse(r.ClientApplication)
                    .SelectToken("NativeDesktopThirdParty.DeviceCapabilities")
                    .Value<string>() == "New Capability";

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateSolutionNativeDesktopThirdPartyToNull()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ }");

            var validationResult = await UpdateNativeDesktopThirdParty();
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("NativeDesktopThirdParty.ThirdPartyComponents") == null
                && JToken.Parse(r.ClientApplication).SelectToken("NativeDesktopThirdParty.DeviceCapabilities") == null;

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateSolutionNativeDesktopThirdPartyAndNothingElse()
        {
            var clientApplication = new Application.Domain.ClientApplication
            {
                NativeDesktopThirdParty = new NativeDesktopThirdParty
                {
                    ThirdPartyComponents = "Component",
                    DeviceCapabilities = "Capabilities",
                }
            };

            var clientJson = JsonConvert.SerializeObject(clientApplication);

            SetUpMockSolutionRepositoryGetByIdAsync(clientJson);

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken _)
            {
                calledBack = true;
                var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);
                var newClientApplication = JsonConvert.DeserializeObject<Application.Domain.ClientApplication>(json.ToString());
                clientApplication.Should().BeEquivalentTo(newClientApplication, c => c.Excluding(m => m.NativeDesktopThirdParty));

                newClientApplication.NativeDesktopThirdParty.ThirdPartyComponents.Should().Be("New Component");
                newClientApplication.NativeDesktopThirdParty.DeviceCapabilities.Should().Be("New Capability");
            }

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateNativeDesktopThirdParty("New Component", "New Capability");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotUpdateInvalidThirdParty()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeDesktopThirdParty(new string('a', 501), new string('a', 501));
            validationResult.IsValid.Should().Be(false);

            var maxLengthResult = validationResult as MaxLengthResult;

            Assert.NotNull(maxLengthResult);
            maxLengthResult.MaxLength.Should().BeEquivalentTo("third-party-components", "device-capabilities");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Never());

            Expression<Func<ISolutionDetailRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(expression, Times.Never());
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateNativeDesktopThirdParty("Component", "Capability"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<ISolutionDetailRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(expression, Times.Never());
        }

        private async Task<ISimpleResult> UpdateNativeDesktopThirdParty(string components = null, string capabilities = null)
        {
            var data = Mock.Of<IUpdateNativeDesktopThirdPartyData>(t =>
                t.ThirdPartyComponents == components && t.DeviceCapabilities == capabilities);

            return await Context.UpdateSolutionNativeDesktopThirdPartyHandler.Handle(
                new UpdateSolutionNativeDesktopThirdPartyCommand(SolutionId, data),
                CancellationToken.None);
        }
    }
}
