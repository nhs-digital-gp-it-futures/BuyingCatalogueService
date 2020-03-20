using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using TechTalk.SpecFlow;

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

        [Then(@"the solution client-application-types section does not contain Supported Browsers")]
        public async Task ThenTheSolutionClient_Application_TypesSectionDoesNotContainSupportedBrowsersOf()
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.browsers-supported.answers.supported-browsers").Should().BeNull();
        }

        [Then(@"the solution client-application-types section contains mobile responsive with value null")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsMobileResponsive()
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.client-application-types.sections.browser-based.sections.browser-browsers-supported.answers.mobile-responsive").Should().BeNull();
        }
    }
}
