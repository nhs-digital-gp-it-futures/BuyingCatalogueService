using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.BrowserHardwareRequirements
{
    [Binding]
    internal sealed class GetBrowserHardwareRequirementSteps
    {
        private readonly Response _response;

        public GetBrowserHardwareRequirementSteps(Response response)
        {
            _response = response;
        }

        [Then(@"the browser-hardware-requirements string is (.*)")]
        public async Task ThenTheBrowserHardwareRequirementsStringIsTest(string requirement)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("hardware-requirements-description").Value<string>().Should().Be(requirement);
        }

        [Then(@"there are no browser-hardware-requirements")]
        public async Task ThenTheBrowserHardwareRequirementsValueIsNull()
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            string.IsNullOrEmpty(content.ToString());
        }

    }
}
