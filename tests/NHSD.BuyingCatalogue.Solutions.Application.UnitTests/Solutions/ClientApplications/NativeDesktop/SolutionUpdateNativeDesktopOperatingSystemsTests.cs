using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionNativeDesktopOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.ClientApplications.NativeDesktop
{
    [TestFixture]
    internal sealed class SolutionUpdateNativeDesktopOperatingSystemsTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateSolutionNativeDesktopOperatingSystems()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");
            var validationResult = await UpdateNativeDesktopOperatingSystems("New Description");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication)
                    .SelectToken("NativeDesktopOperatingSystemsDescription")
                    .Value<string>() == "New Description";

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldNotUpdateSolutionNativeDesktopOperatingSystemsToNull()
        {
            const string clientApplicationJson =
                "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile', 'native-desktop' ], "
                + "'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], "
                + "'MobileResponsive': false, "
                + "'Plugins' : {'Required' : true, 'AdditionalInformation': 'lorem ipsum' }, "
                + "'NativeDesktopOperatingSystemsDescription': 'old description' }";

            SetUpMockSolutionRepositoryGetByIdAsync(clientApplicationJson);

            var validationResult = await UpdateNativeDesktopOperatingSystems(null);
            validationResult.IsValid.Should().BeFalse();

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("NativeMobileHardwareRequirements").Value<string>() == "old description";

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()),
                Times.Never());
        }

        [Test]
        public async Task ShouldUpdateSolutionNativeDesktopOperatingSystemsAndNothingElse()
        {
            const string clientApplicationJson =
                "{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile', 'native-desktop' ], "
                + "'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], "
                + "'MobileResponsive': false, "
                + "'Plugins' : {'Required' : true, 'AdditionalInformation': 'lorem ipsum' }, "
                + "'NativeDesktopOperatingSystemsDescription': 'old description' }";

            SetUpMockSolutionRepositoryGetByIdAsync(clientApplicationJson);

            var calledBack = false;

            void Action(IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest, CancellationToken _)
            {
                calledBack = true;
                var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                json.SelectToken("NativeDesktopOperatingSystemsDescription")?
                    .Value<string>()
                    .Should()
                    .Be("New Description");
            }

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback<IUpdateSolutionClientApplicationRequest, CancellationToken>(Action);

            var validationResult = await UpdateNativeDesktopOperatingSystems("New Description");
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            calledBack.Should().BeTrue();
        }

        [TestCase("")]
        [TestCase("                             ")]
        [TestCase(null)]
        public async Task ShouldNotUpdateInvalidRequiredNativeDesktopOperatingSystems(string description)
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeDesktopOperatingSystems(description);
            validationResult.IsValid.Should().BeFalse();
            validationResult.ToDictionary()["operating-systems-description"].Should().Be("required");
        }

        [Test]
        public async Task ShouldNotUpdateInvalidMaxLengthNativeDesktopOperatingSystems()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeDesktopOperatingSystems(new string('d', 1001));
            validationResult.IsValid.Should().BeFalse();
            validationResult.ToDictionary()["operating-systems-description"].Should().Be("maxLength");
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateNativeDesktopOperatingSystems("New description"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<ISolutionDetailRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(expression, Times.Never());
        }

        private async Task<ISimpleResult> UpdateNativeDesktopOperatingSystems(string description)
        {
            return await Context.UpdateSolutionNativeDesktopOperatingSystemsHandler.Handle(
                 new UpdateSolutionNativeDesktopOperatingSystemsCommand(SolutionId, description),
                 CancellationToken.None);
        }
    }
}
