using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionNativeDesktopThirdParty;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.NativeDesktop
{
    internal sealed class SolutionUpdateNativeDesktopThirdPartyTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateSolutionNativeDesktopThirdParty()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeDesktopThirdParty("New Component", "New Capability").ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("NativeDesktopThirdParty.ThirdPartyComponents").Value<string>() == "New Component"
                && JToken.Parse(r.ClientApplication).SelectToken("NativeDesktopThirdParty.DeviceCapabilities").Value<string>() == "New Capability"
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldUpdateSolutionNativeDesktopThirdPartyToNull()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ }");

            var validationResult = await UpdateNativeDesktopThirdParty().ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("NativeDesktopThirdParty.ThirdPartyComponents").Value<string>() == null
                && JToken.Parse(r.ClientApplication).SelectToken("NativeDesktopThirdParty.DeviceCapabilities").Value<string>() == null
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldUpdateSolutionNativeDesktopThirdPartyAndNothingElse()
        {
            var clientApplication = new ClientApplication { NativeDesktopThirdParty = new NativeDesktopThirdParty()
            {
                ThirdPartyComponents = "Component",
                DeviceCapabilities = "Capabilities"
            }};
            var clientJson = JsonConvert.SerializeObject(clientApplication);

            SetUpMockSolutionRepositoryGetByIdAsync(clientJson);

            var calledBack = false;

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest,
                    CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);
                    var newClientApplication = JsonConvert.DeserializeObject<ClientApplication>(json.ToString());
                    clientApplication.Should().BeEquivalentTo(newClientApplication, c =>
                        c.Excluding(m => m.NativeDesktopThirdParty));

                    newClientApplication.NativeDesktopThirdParty.ThirdPartyComponents.Should().Be("New Component");
                    newClientApplication.NativeDesktopThirdParty.DeviceCapabilities.Should().Be("New Capability");
                });
            var validationResult = await UpdateNativeDesktopThirdParty("New Component", "New Capability").ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotUpdateInvalidThirdParty()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeDesktopThirdParty(new string('a', 501), new string('a', 501))
                .ConfigureAwait(false);
            validationResult.IsValid.Should().Be(false);

            var maxLengthResult = validationResult as MaxLengthResult;
            maxLengthResult.MaxLength.Should().BeEquivalentTo("third-party-components", "device-capabilities");

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Never);
            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateNativeDesktopThirdParty("Component", "Capability"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public void CommandShouldTrimStrings()
        {
            var viewModel = new Mock<IUpdateNativeDesktopThirdPartyData>();
            var trimmedViewModel = Mock.Of<IUpdateNativeDesktopThirdPartyData>();
            viewModel.Setup(x => x.Trim()).Returns(trimmedViewModel);

            var command = new UpdateSolutionNativeDesktopThirdPartyCommand("Sln1", viewModel.Object);
            viewModel.Verify(x => x.Trim(), Times.Once);
            command.Data.IsSameOrEqualTo(trimmedViewModel);
        }

        private async Task<ISimpleResult> UpdateNativeDesktopThirdParty(string components = null, string capabilities = null)
        {
            var trimmedData = Mock.Of<IUpdateNativeDesktopThirdPartyData>(t =>
                t.ThirdPartyComponents == components && t.DeviceCapabilities == capabilities);

            var data = new Mock<IUpdateNativeDesktopThirdPartyData>();
            data.Setup(s => s.Trim()).Returns(trimmedData);

            return await Context.UpdateSolutionNativeDesktopThirdPartyHandler.Handle(
                new UpdateSolutionNativeDesktopThirdPartyCommand(SolutionId, data.Object),
                new CancellationToken()).ConfigureAwait(false);
        }
    }
}
