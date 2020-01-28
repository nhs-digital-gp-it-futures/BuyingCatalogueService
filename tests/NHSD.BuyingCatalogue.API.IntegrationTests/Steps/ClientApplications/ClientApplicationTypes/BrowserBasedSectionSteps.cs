using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.ClientApplications.ClientApplicationTypes
{
    [Binding]
    internal sealed class BrowserBasedSectionSteps
    {
        private readonly Response _response;

        public BrowserBasedSectionSteps(Response response)
        {
            _response = response;
        }

        [Then(@"the solution client-application-types section contains Browsers")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsSupportedBrowsersOf(Table table)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.browser-browsers-supported.answers.supported-browsers")
                .Select(s => s.ToString()).Should().BeEquivalentTo(table.CreateSet<SelectedBrowsersTable>().Select(s => s.Browser));
        }

        [Then(@"the solution client-application-types section does not contain Supported Browsers")]
        public async Task ThenTheSolutionClient_Application_TypesSectionDoesNotContainSupportedBrowsersOf()
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.browsers-supported.answers.supported-browsers").Should().BeNull();
        }

        [Then(@"the solution client-application-types section contains mobile responsive with value (Yes|No)")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsMobileResponsive(string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.browser-browsers-supported.answers.mobile-responsive").ToString().Should().Be(value);
        }

        [Then(@"the solution client-application-types section contains mobile responsive with value null")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsMobileResponsive()
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.browser-browsers-supported.answers.mobile-responsive").Should().BeNull();
        }

        [Then(@"the solution client-application-types section contains plugin required with value (Yes|No)")]
        public async Task ThenTheSolutionClientApplicationTypesSectionContainsPluginRequiredWithValue(string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.browser-plug-ins-or-extensions.answers.plugins-required")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution client-application-types section contains plugin detail with value (.*)")]
        public async Task ThenTheSolutionClientApplicationTypesSectionContainsPluginAdditionalInformationWithValue(string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.browser-plug-ins-or-extensions.answers.plugins-detail")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution client-application-types section contains hardware-requirements with value (.*)")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsHardware_RequirementsWithValueSomeNewHardware(string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.browser-hardware-requirements.answers.hardware-requirements-description")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution client-application-types section contains minimum-connection-speed with value (.*)")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsMinimumConnectionSpeedWithValue(string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.browser-connectivity-and-resolution.answers.minimum-connection-speed")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution client-application-types section contains minimum-desktop-resolution with value (.*)")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsMinimumDesktopResolutionWithValue(string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.browser-connectivity-and-resolution.answers.minimum-desktop-resolution")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution client-application-types section contains additional-information with value (.*)")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsAdditional_InformationWithValueSomeAdditionalInfo(string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.browser-additional-information.answers.additional-information")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution client-application-types section contains browser-mobile-first with value (Yes|No)")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsBrowser_Mobile_FirstWithValueYes(string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.browser-mobile-first.answers.mobile-first-design")
                .ToString().Should().Be(value);
        }

        private class SelectedBrowsersTable
        {
            public string Browser { get; set; }
        }
    }
}
