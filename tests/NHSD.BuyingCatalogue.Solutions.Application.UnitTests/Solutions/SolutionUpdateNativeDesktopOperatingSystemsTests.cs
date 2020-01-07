using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionNativeDesktopOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionUpdateNativeDesktopOperatingSystemsTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateSolutionNativeDesktopOperatingSystems()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");
            var validationResult = await UpdateNativeDesktopOperatingSystems("New Description").ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("NativeDesktopOperatingSystemsDescription").Value<string>() == "New Description"
            ), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task ShouldNotUpdateSolutionNativeDesktopOperatingSystemsToNull()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile', 'native-desktop' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false, 'Plugins' : {'Required' : true, 'AdditionalInformation': 'lorem ipsum' }, 'NativeDesktopOperatingSystemsDescription': 'old description' }");

            var validationResult = await UpdateNativeDesktopOperatingSystems(null).ConfigureAwait(false);
            validationResult.IsValid.Should().BeFalse();

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).SelectToken("NativeMobileHardwareRequirements").Value<string>() == "old description"
            ), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public async Task ShouldUpdateSolutionNativeDesktopOperatingSystemsAndNothingElse()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{ 'ClientApplicationTypes' : [ 'browser-based', 'native-mobile', 'native-desktop' ], 'BrowsersSupported' : [ 'Mozilla Firefox', 'Edge' ], 'MobileResponsive': false, 'Plugins' : {'Required' : true, 'AdditionalInformation': 'lorem ipsum' }, 'NativeDesktopOperatingSystemsDescription': 'old description' }");

            var calledBack = false;

            Context.MockSolutionDetailRepository
                .Setup(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback((IUpdateSolutionClientApplicationRequest updateSolutionClientApplicationRequest,
                    CancellationToken cancellationToken) =>
                {
                    calledBack = true;
                    var json = JToken.Parse(updateSolutionClientApplicationRequest.ClientApplication);

                    json.SelectToken("NativeDesktopOperatingSystemsDescription").Value<string>().Should()
                        .Be("New Description");
                });

            var validationResult = await UpdateNativeDesktopOperatingSystems("New Description").ConfigureAwait(false);
            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            calledBack.Should().BeTrue();
        }

        [TestCase("")]
        [TestCase("                             ")]
        [TestCase(null)]
        public async Task ShouldNotUpdateInvalidRequiredNativeDesktopOperatingSystems(string description)
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeDesktopOperatingSystems(description).ConfigureAwait(false);
            validationResult.IsValid.Should().BeFalse();
            validationResult.ToDictionary()["operating-systems-description"].Should().Be("required");
        }
        [Test]
        public async Task ShouldNotUpdateInvalidMaxLengthNativeDesktopOperatingSystems()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateNativeDesktopOperatingSystems(new string(c: 'd', count: 1001)).ConfigureAwait(false);
            validationResult.IsValid.Should().BeFalse();
            validationResult.ToDictionary()["operating-systems-description"].Should().Be("maxLength");
        }

        [Test]
        public void ShouldThrowWhenSolutionNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateNativeDesktopOperatingSystems("New description"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once());

            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Test]
        public void CommandShouldTrimStrings()
        {
            var originalDescription = "    some description   ";
            var command = new UpdateSolutionNativeDesktopOperatingSystemsCommand(SolutionId, originalDescription);
            command.NativeDesktopOperatingSystemsDescription.Should().Be("some description");
        }

        private async Task<ISimpleResult> UpdateNativeDesktopOperatingSystems(
            string description)
        {
            return await Context.UpdateSolutionNativeDesktopOperatingSystemsHandler.Handle(
                 new UpdateSolutionNativeDesktopOperatingSystemsCommand(SolutionId, description),
                 new CancellationToken()).ConfigureAwait(false);
        }

    }
}
