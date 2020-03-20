using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.ClientApplications.BrowserHardwareRequirements
{
    [Binding]
    internal sealed class GetBrowserHardwareRequirementSteps
    {
        private readonly Response _response;

        public GetBrowserHardwareRequirementSteps(Response response)
        {
            _response = response;
        }

        [Then(@"there are no browser-hardware-requirements")]
        public async Task ThenTheBrowserHardwareRequirementsValueIsNull()
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            string.IsNullOrEmpty(content.ToString());
        }

    }
}
