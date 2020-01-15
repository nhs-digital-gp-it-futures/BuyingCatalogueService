using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateNativeDesktopAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.NativeDesktop
{
    internal sealed class UpdateNativeDesktopAdditionalInformationTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateNativeDesktopAdditionInformation()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeDesktopAdditionalInformation("Some info").ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("NativeDesktopAdditionalInformation").Value<string>() == "Some info"
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldUpdateNativeDesktopAdditionalInformationToNull()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ 'NativeDesktopAdditionalInformation': 'some additional info' }");

            var calledBack = false;

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                    json.SelectToken("NativeDesktopAdditionalInformation").Should().BeNullOrEmpty();
                });

            var validationResult = await UpdateNativeDesktopAdditionalInformation(null).ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdateNativeDesktopAdditionalInformationAndNothingElse()
        {
            var clientApplication = new ClientApplication { NativeDesktopAdditionalInformation = "Some old info" };
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
                        c.Excluding(m => m.NativeDesktopAdditionalInformation));

                    newClientApplication.NativeDesktopAdditionalInformation.Should().Be("Some new info");
                });
            var validationResult = await UpdateNativeDesktopAdditionalInformation("Some new info").ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotUpdateInvalidNativeDesktopAdditionalInformation()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeDesktopAdditionalInformation(new string('a', 501)).ConfigureAwait(false);
            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["additional-information"].Should().Be("maxLength");
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateNativeDesktopAdditionalInformation("New Additional Info"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public void CommandShouldTrimStrings()
        {
            var command = new UpdateNativeDesktopAdditionalInformationCommand("Sln1", "       hello   ");
            command.AdditionalInformation.Should().Be("hello");
        }

        private async Task<ISimpleResult> UpdateNativeDesktopAdditionalInformation(
            string additionalInformation = null)
        {
            return await Context.UpdateNativeDesktopAdditionalInformationHandler.Handle(
                new UpdateNativeDesktopAdditionalInformationCommand(SolutionId, additionalInformation),
                new CancellationToken()).ConfigureAwait(false);
        }
    }
}
