using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionConnectivityDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.NativeDesktop
{
    internal sealed class SolutionUpdateNativeDesktopConnectivityDetailsTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateSolutionNativeConnectivityDetails()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeDesktopConnectivityDetails("2 Mbps").ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("NativeDesktopMinimumConnectionSpeed").Value<string>() == "2 Mbps"
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldNotUpdateSolutionNativeMobileConnectivityDetailsToNull()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ }");

            var validationResult = await UpdateNativeDesktopConnectivityDetails().ConfigureAwait(false);
            validationResult.IsValid.Should().BeFalse();
            validationResult.ToDictionary()["minimum-connection-speed"].Should().Be("required");
        }

        [Test]
        public async Task ShouldUpdateSolutionNativeMobileConnectivityDetailsAndNothingElse()
        {
            var clientApplication = new ClientApplication { NativeDesktopMinimumConnectionSpeed = "3Mbps" };
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
                        c.Excluding(m => m.NativeDesktopMinimumConnectionSpeed));

                    newClientApplication.NativeDesktopMinimumConnectionSpeed.Should().Be("6Mbps");
                });
            var validationResult = await UpdateNativeDesktopConnectivityDetails("6Mbps").ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateNativeDesktopConnectivityDetails("3Mbps"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public void CommandShouldTrimStrings()
        {
            var command = new UpdateSolutionNativeDesktopConnectivityDetailsCommand("Sln1", "    6Mbps    ");
            command.NativeDesktopMinimumConnectionSpeed.Should().Be("6Mbps");
        }

        private async Task<ISimpleResult> UpdateNativeDesktopConnectivityDetails(string connectivityDetails = null)
        {
            return await Context.UpdateSolutionNativeDesktopConnectivityDetailsHandler.Handle(
                new UpdateSolutionNativeDesktopConnectivityDetailsCommand(SolutionId, connectivityDetails),
                new CancellationToken()).ConfigureAwait(false);
        }

    }
}
