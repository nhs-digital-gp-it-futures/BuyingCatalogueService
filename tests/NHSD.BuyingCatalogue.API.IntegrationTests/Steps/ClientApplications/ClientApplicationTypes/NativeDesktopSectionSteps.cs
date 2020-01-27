using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.ClientApplications.ClientApplicationTypes
{
    [Binding]
    internal sealed class NativeDesktopSectionSteps
    {
        private readonly Response _response;

        private const string Token = "sections.client-application-types.sections.native-desktop.sections";

        public NativeDesktopSectionSteps(Response response)
        {
            _response = response;
        }

        [Then(@"the solution native-desktop native-desktop-hardware-requirements section contains hardware-requirements with value (.*)")]
        public async Task ThenTheSolutionNativeDesktopHardwareRequirementsSectionContainsWithValue(string value)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken($"{Token}.native-desktop-hardware-requirements.answers.hardware-requirements")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution native-desktop native-desktop-operating-systems section contains operating-systems-description with value (.*)")]
        public async Task ThenTheSolutionNativeDesktopOperatingSystemsDescriptionSectionContainsWithValue(string value)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken($"{Token}.native-desktop-operating-systems.answers.operating-systems-description")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution native-desktop native-desktop-connection-details section contains minimum-connection-speed with value (.*)")]
        public async Task ThenTheSolutionNativeDesktopConnectionDetailsSectionContainsWithValue(string value)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken($"{Token}.native-desktop-connection-details.answers.minimum-connection-speed")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution native-desktop native-desktop-third-party section contains (third-party-components|device-capabilities) with value (.*)")]
        public async Task ThenTheSolutionNativeDesktopThirdPartySectionContainsWithValue(string section, string value)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken($"{Token}.native-desktop-third-party.answers.{section}")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution native-desktop native-desktop-memory-and-storage section contains (minimum-memory-requirement|storage-requirements-description|minimum-cpu|recommended-resolution) with value (.*)")]
        public async Task ThenTheSolutionNativeDesktopMemoryAndStorageSectionContainsWithValue(string section, string value)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken($"{Token}.native-desktop-memory-and-storage.answers.{section}")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution native-desktop native-desktop-additional-information section contains additional-information with value (.*)")]
        public async Task ThenTheSolutionNativeDesktopAdditionalInformationSectionContainsWithValue(string value)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken($"{Token}.native-desktop-additional-information.answers.additional-information")
                .ToString().Should().Be(value);
        }
    }
}
