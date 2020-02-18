using System.Threading.Tasks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Epics
{
    [Binding]
    internal sealed class EpicSteps
    {
        private readonly Response _response;

        public EpicSteps(Response response)
        {
            _response = response;
        }
        
        [When(@"a PUT request is made to update a epics section for solution (.*)")]
        public async Task WhenAPUTRequestIsMadeToUpdateEpicsSection(string solutionId, Table table)
        {
            var obj = new { epics = table.CreateSet<EpicsPayload>() };

            _response.Result = await Client.PutAsJsonAsync($"http://localhost:8080/api/v1/solutions/{solutionId}/sections/epics", obj)
                .ConfigureAwait(false);
        }
        
        private class EpicsPayload
        {
            [JsonProperty("epic-id")]
            public string EpicId { get; set; }

            [JsonProperty("assessment-result")]
            public string StatusName { get; set; }
        }
    }
}
