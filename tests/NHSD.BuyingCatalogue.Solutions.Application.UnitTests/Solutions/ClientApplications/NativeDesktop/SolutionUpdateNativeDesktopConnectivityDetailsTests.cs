using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionConnectivityDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.ClientApplications.NativeDesktop
{
    internal sealed class SolutionUpdateNativeDesktopConnectivityDetailsTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateSolutionNativeConnectivityDetails()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeDesktopConnectivityDetails("2 Mbps");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r =>
                r.SolutionId == SolutionId && JToken.Parse(r.ClientApplication)
                    .SelectToken("NativeDesktopMinimumConnectionSpeed")
                    .Value<string>() == "2 Mbps";

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldNotUpdateSolutionNativeMobileConnectivityDetailsToNull()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ }");

            var validationResult = await UpdateNativeDesktopConnectivityDetails();
            validationResult.IsValid.Should().BeFalse();
            validationResult.ToDictionary()["minimum-connection-speed"].Should().Be("required");
        }

        [Test]
        public async Task ShouldUpdateSolutionNativeMobileConnectivityDetailsAndNothingElse()
        {
            var clientApplication = new Application.Domain.ClientApplication { NativeDesktopMinimumConnectionSpeed = "3Mbps" };
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
                    c => c.Excluding(m => m.NativeDesktopMinimumConnectionSpeed));

                newClientApplication.NativeDesktopMinimumConnectionSpeed.Should().Be("6Mbps");
            }

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateNativeDesktopConnectivityDetails("6Mbps");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateNativeDesktopConnectivityDetails("3Mbps"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<ISolutionDetailRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(expression, Times.Never());
        }

        private async Task<ISimpleResult> UpdateNativeDesktopConnectivityDetails(string connectivityDetails = null)
        {
            return await Context.UpdateSolutionNativeDesktopConnectivityDetailsHandler.Handle(
                new UpdateSolutionNativeDesktopConnectivityDetailsCommand(SolutionId, connectivityDetails),
                CancellationToken.None);
        }
    }
}
