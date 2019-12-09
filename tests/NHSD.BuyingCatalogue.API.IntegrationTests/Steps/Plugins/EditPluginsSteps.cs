using System.Threading.Tasks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class EditPluginsSteps
    {
        private const string PluginsUrl = "http://localhost:8080/api/v1/solutions/{0}/sections/plug-ins-or-extensions";

        private readonly Response _response;

        public EditPluginsSteps(Response response)
        {
            _response = response;
        }

        [When(@"a PUT request is to update solution (.*) plug-ins section")]
        public async Task WhenAPUTRequestIsToUpdateSolutionSlnPlug_InsSection(string solutionId, Table table)
        {
            var content = table.CreateInstance<PluginsTable>();

            _response.Result = await Client.PutAsJsonAsync(string.Format(PluginsUrl, solutionId),
                new PluginsPayload()
                {
                    PluginsRequired = content.Required,
                    PluginsDetail = content.AdditionalInformation
                }).ConfigureAwait(false);
        }

        [When(@"a PUT request is made to update plug-ins section with no solution id")]
        public async Task WhenAPUTRequestIsMadeToUpdatePlug_InsSectionWithNoSolutionId(Table table)
        {
            await WhenAPUTRequestIsToUpdateSolutionSlnPlug_InsSection(" ", table).ConfigureAwait(false);
        }


        private class PluginsTable
        {
            public string Required { get; set; }

            public string AdditionalInformation { get; set; }
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
