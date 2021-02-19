using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.ClientApplications.NativeDesktop
{
    internal sealed class UpdateNativeDesktopAdditionalInformationTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateNativeDesktopAdditionInformation()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeDesktopAdditionalInformation("Some info");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("NativeDesktopAdditionalInformation").Value<string>() == "Some info";

            Context.MockSolutionRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateNativeDesktopAdditionalInformationToNull()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ 'NativeDesktopAdditionalInformation': 'some additional info' }");

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest request, CancellationToken token)
            {
                calledBack = true;
                var json = JToken.Parse(request.ClientApplication);

                json.SelectToken("NativeDesktopAdditionalInformation").Should().BeNullOrEmpty();
            }

            Context.MockSolutionRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateNativeDesktopAdditionalInformation();
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdateNativeDesktopAdditionalInformationAndNothingElse()
        {
            var clientApplication = new Application.Domain.ClientApplication
            {
                NativeDesktopAdditionalInformation = "Some old info",
            };

            var clientJson = JsonConvert.SerializeObject(clientApplication);

            SetUpMockSolutionRepositoryGetByIdAsync(clientJson);

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest request, CancellationToken token)
            {
                calledBack = true;
                var json = JToken.Parse(request.ClientApplication);
                var newClientApplication = JsonConvert.DeserializeObject<Application.Domain.ClientApplication>(
                    json.ToString());

                clientApplication.Should().BeEquivalentTo(
                    newClientApplication,
                    c => c.Excluding(m => m.NativeDesktopAdditionalInformation));

                newClientApplication.NativeDesktopAdditionalInformation.Should().Be("Some new info");
            }

            Context.MockSolutionRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateNativeDesktopAdditionalInformation("Some new info");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotUpdateInvalidNativeDesktopAdditionalInformation()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeDesktopAdditionalInformation(new string('a', 501));
            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["additional-information"].Should().Be("maxLength");
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateNativeDesktopAdditionalInformation("New Additional Info"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<ISolutionRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionRepository.Verify(expression, Times.Never());
        }

        private async Task<ISimpleResult> UpdateNativeDesktopAdditionalInformation(string additionalInformation = null)
        {
            return await Context.UpdateNativeDesktopAdditionalInformationHandler.Handle(
                new UpdateNativeDesktopAdditionalInformationCommand(SolutionId, additionalInformation),
                CancellationToken.None);
        }
    }
}
