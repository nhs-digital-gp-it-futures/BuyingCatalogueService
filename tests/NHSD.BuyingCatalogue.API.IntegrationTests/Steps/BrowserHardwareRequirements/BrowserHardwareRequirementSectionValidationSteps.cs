using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.BrowserHardwareRequirements
{
    [Binding]
    internal sealed class BrowserHardwareRequirementSectionValidationSteps
    {
        private const string BrowserHardwareRequirementsUrl = "http://localhost:8080/api/v1/solutions/{0}/sections/browser-hardware-requirements";

        private readonly Response _response;

        public BrowserHardwareRequirementSectionValidationSteps(Response response)
        {
            _response = response;
        }

        [When(@"a PUT request is made to update solution (.*) hardware-requirements-description section")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionSlnBrowserHardwareRequirementsSection(string solutionId, Table table)
        {
            var content = table.CreateInstance<BrowserHardwareRequirementsPayload>();
            _response.Result = await Client.PutAsJsonAsync(string.Format(CultureInfo.InvariantCulture, BrowserHardwareRequirementsUrl, solutionId), content).ConfigureAwait(false);
        }

        [Then(@"the browser-hardware-requirements maxLength field contains (.*)")]
        public async Task ThenTheBrowserHardwareRequirementsMaxLengthFieldContainsRequired(string field)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken("maxLength").ToString().Should().Contain(field);
        }

        private class BrowserHardwareRequirementsPayload
        {
            [JsonProperty("hardware-requirements-description")]
            public string HardwareRequirements { get; set; }
        }
    }
}
