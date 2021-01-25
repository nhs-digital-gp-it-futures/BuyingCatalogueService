using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.ClientApplications.NativeDesktop
{
    [TestFixture]
    internal sealed class UpdateNativeDesktopHardwareRequirementsTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateSolutionNativeMobileHardwareRequirements()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeDesktopHardwareRequirements("New hardware");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("NativeDesktopHardwareRequirements").Value<string>() == "New hardware";

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateSolutionNativeMobileHardwareRequirementsToNull()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ }");

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken _)
            {
                calledBack = true;
                var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                json.SelectToken("NativeDesktopHardwareRequirements").Should().BeNullOrEmpty();
            }

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateNativeDesktopHardwareRequirements(null);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdateSolutionNativeMobileHardwareRequirementAndNothingElse()
        {
            var clientApplication = new Application.Domain.ClientApplication
            {
                NativeDesktopHardwareRequirements = "initialHardwareRequirements",
            };

            var clientJson = JsonConvert.SerializeObject(clientApplication);

            SetUpMockSolutionRepositoryGetByIdAsync(clientJson);

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken _)
            {
                calledBack = true;
                var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);
                var newClientApplication = JsonConvert.DeserializeObject<Application.Domain.ClientApplication>(
                    json.ToString());

                clientApplication.Should().BeEquivalentTo(
                    newClientApplication,
                    c => c.Excluding(m => m.NativeDesktopHardwareRequirements));

                newClientApplication.NativeDesktopHardwareRequirements.Should().Be("Updated hardware");
            }

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateNativeDesktopHardwareRequirements("Updated hardware");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotUpdateInvalidNativeMobileHardwareRequirements()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeDesktopHardwareRequirements(new string('a', 501));
            validationResult.IsValid.Should().BeFalse();
            validationResult.ToDictionary()["hardware-requirements"].Should().Be("maxLength");
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateNativeDesktopHardwareRequirements("New hardware"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<ISolutionDetailRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(expression, Times.Never());
        }

        private async Task<ISimpleResult> UpdateNativeDesktopHardwareRequirements(string hardwareRequirements)
        {
            return await Context.UpdateNativeDesktopHardwareRequirementsHandler.Handle(
                new UpdateNativeDesktopHardwareRequirementsCommand(SolutionId, hardwareRequirements),
                CancellationToken.None);
        }
    }
}
