using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.BrowserHardwareRequirements
{
    [Binding]
    internal sealed class EditBrowserHardwareRequirementSteps
    {
        private const string BrowserHardwareRequirementsUrl = "http://localhost:8080/api/v1/solutions/{0}/sections/browser-hardware-requirements";

        private readonly Response _response;

        public EditBrowserHardwareRequirementSteps(Response response)
        {
            _response = response;
        }

        [When(@"a PUT request is made to update solution (.*) browser-hardware-requirements section")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionSlnBrowser_Hardware_RequirementsSection(string solutionId, Table table)
        {
            var content = table.CreateInstance<BrowserHardwareRequirementsSection>();

            _response.Result = await Client
                .PutAsJsonAsync(string.Format(CultureInfo.InvariantCulture, BrowserHardwareRequirementsUrl, solutionId),
                    new BrowserHardwareRequirementsPayload {HardwareRequirements = content.HardwareRequirements})
                .ConfigureAwait(false);
        }

        [When(@"a PUT request is made to update solution browser-hardware-requirements section with no solution id")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionBrowser_Hardware_RequirementsSectionWithNoSolutionId(Table table)
        {
            await WhenAPUTRequestIsMadeToUpdateSolutionSlnBrowser_Hardware_RequirementsSection(" ", table).ConfigureAwait(false);
        }

        private class BrowserHardwareRequirementsPayload
        {
            [JsonProperty("hardware-requirements-description")]
            public string HardwareRequirements { get; set; }
        }
        
        private class BrowserHardwareRequirementsSection
        {
            public string HardwareRequirements { get; set; }
        }
    }
}
