using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Plugins
{
    [Binding]
    internal sealed class PluginsSectionValidationSteps
    {
        private const string PluginsUrl = "http://localhost:8080/api/v1/solutions/{0}/sections/plug-ins-or-extensions";

        private readonly Response _response;

        public PluginsSectionValidationSteps(Response response)
        {
            _response = response;
        }

        [When(@"a PUT request is made to update solution (.*) plug-ins section")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionSlnPlug_InsSection(string solutionId, Table table)
        {
            var content = table.CreateInstance<PluginsPayload>();
            _response.Result = await Client.PutAsJsonAsync(string.Format(CultureInfo.InvariantCulture, PluginsUrl, solutionId), content).ConfigureAwait(false);
        }

        private class PluginsPayload
        {
            [JsonProperty("plugins-required")]
            public string PluginsRequired { get; set; }

            [JsonProperty("plugins-detail")]
            public string PluginsDetail { get; set; }

        }
    }
}
