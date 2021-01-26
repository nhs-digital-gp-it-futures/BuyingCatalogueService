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
        private readonly Response response;

        public EpicSteps(Response response)
        {
            this.response = response;
        }

        [When(@"a PUT request is made to update a epics section for solution (.*)")]
        public async Task WhenAPutRequestIsMadeToUpdateEpicsSection(string solutionId, Table table)
        {
            var obj = new { epics = table.CreateSet<EpicsPayload>() };

            response.Result = await Client.PutAsJsonAsync(
                $"http://localhost:5200/api/v1/solutions/{solutionId}/sections/epics",
                obj);
        }

        private sealed class EpicsPayload
        {
            [JsonProperty("epic-id")]
            public string EpicId { get; init; }

            [JsonProperty("assessment-result")]
            public string StatusName { get; init; }
        }
    }
}
