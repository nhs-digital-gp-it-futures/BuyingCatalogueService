using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class ClientApplicationTypeSectionSteps
    {
        private readonly ScenarioContext _context;

        private readonly Response _response;

        public ClientApplicationTypeSectionSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [Then(@"the client-application-types section contains (.*) subsections")]
        public async Task ThenTheClientApplicationTypesSectionContainsSubsections(int subsectionCount)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.client-application-types").Children()
                .Should().HaveCount(subsectionCount);
        }

        [Then(@"the client-application-types section is missing")]
        public async Task ThenTheClientApplicationTypesSectionIsMissing()
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.client-application-types").Should().BeNull();
        }

        [Then(@"the client-application-types section contains subsection (\S+)")]
        public async Task ThenTheClientApplicationTypesSectionContainsSubsectionBrowserBased(string subsection)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.client-application-types.sections.{subsection}")
                .Should().NotBeNull();
        }

        [Then(@"the solution client-application-types section contains Browsers")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsSupportedBrowsersOf(Table table)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.browsers-supported.answers.supported-browsers")
                .Select(s => s.ToString()).Should().BeEquivalentTo(table.CreateSet<SelectedBrowsersTable>().Select(s => s.Browser));
        }

        [Then(@"the solution client-application-types section does not contain Supported Browsers")]
        public async Task ThenTheSolutionClient_Application_TypesSectionDoesNotContainSupportedBrowsersOf()
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.browsers-supported.answers.supported-browsers")
                .Select(s => s.ToString()).Should().BeNull();
        }

        [Then(@"the solution client-application-types section contains mobile responsive with value (Yes|No)")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsMobileResponsive(string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.browsers-supported.answers.mobile-responsive").ToString().Should().Be(value);
        }

        [Then(@"the solution client-application-types section contains mobile responsive with value null")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsMobileResponsive()
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.browsers-supported.answers.mobile-responsive").Should().BeNull();
        }

        [Then(@"the solution client-application-types section contains plugin required with value (Yes|No)")]
        public async Task ThenTheSolutionClientApplicationTypesSectionContainsPluginRequiredWithValue(string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.plug-ins-or-extensions.answers.plugins-required")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution client-application-types section contains plugin detail with value (.*)")]
        public async Task ThenTheSolutionClientApplicationTypesSectionContainsPluginAdditionalInformationWithValue(string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.plug-ins-or-extensions.answers.plugins-detail")
                .ToString().Should().Be(value);
        }

        [Then(@"the client-application-types section is not returned")]
        public async Task ThenTheSolutionClientApplicationTypesSectionContains()
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types").Should().BeNullOrEmpty();
        }

        [Then(@"the solution client-application-types section is returned")]
        public async Task ThenTheSolutionClient_Application_TypesSectionIsReturnedAsync()
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types").Should().NotBeNullOrEmpty();
        }

        private class ClientApplicationTypesTable
        {
            public string ClientApplication { get; set; }
        }

        private class SelectedBrowsersTable
        {
            public string Browser { get; set; }
        }
    }
}
