using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileHardwareRequirements;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.ClientApplications.NativeMobile
{
    [TestFixture]
    internal sealed class SolutionUpdateNativeMobileHardwareRequirementsTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateSolutionNativeMobileHardwareRequirements()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeMobileHardwareRequirements("New hardware");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("NativeMobileHardwareRequirements").Value<string>() == "New hardware";

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldUpdateSolutionNativeMobileHardwareRequirementsToNull()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ }");

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest request, CancellationToken token)
            {
                calledBack = true;
                var json = JToken.Parse(request.ClientApplication);

                json.SelectToken("NativeMobileHardwareRequirements").Should().BeNullOrEmpty();
            }

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateNativeMobileHardwareRequirements(null);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldUpdateSolutionNativeMobileHardwareRequirementAndNothingElse()
        {
            const string clientApplicationJson = "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile' ], "
                + "'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], "
                + "'MobileResponsive': false, "
                + "'Plugins' : {'Required' : true, 'AdditionalInformation': 'lorem ipsum' } }";

            SetUpMockSolutionRepositoryGetByIdAsync(clientApplicationJson);

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest request, CancellationToken token)
            {
                calledBack = true;
                var json = JToken.Parse(request.ClientApplication);

                json.SelectToken("NativeMobileHardwareRequirements")?
                    .Value<string>()
                    .Should()
                    .Be("Updated hardware");
            }

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(
                    It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateNativeMobileHardwareRequirements("Updated hardware");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotUpdateInvalidNativeMobileHardwareRequirements()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeMobileHardwareRequirements(new string('a', 501));
            validationResult.IsValid.Should().BeFalse();
            validationResult.ToDictionary()["hardware-requirements"].Should().Be("maxLength");
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateNativeMobileHardwareRequirements("New hardware"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<ISolutionDetailRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(expression, Times.Never());
        }

        private async Task<ISimpleResult> UpdateNativeMobileHardwareRequirements(string hardwareRequirements)
        {
            return await Context.UpdateSolutionNativeMobileHardwareRequirementsHandler.Handle(
                new UpdateSolutionNativeMobileHardwareRequirementsCommand(SolutionId, hardwareRequirements),
                CancellationToken.None);
        }
    }
}
