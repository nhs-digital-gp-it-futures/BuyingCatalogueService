using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class SolutionDescriptionSectionStepsValidation
    {
        private const string SolutionDescriptionUrl = "http://localhost:8080/api/v1/Solutions/{0}/sections/solution-description";

        private readonly Response _response;

        public SolutionDescriptionSectionStepsValidation(Response response)
        {
            _response = response;
        }

        [When(@"the update solution description request is made for (.*)")]
        public async Task WhenTheRequestIsMade(string solutionId, Table table)
        {
            var content = table.CreateInstance<SolutionDescriptionPayload>();
            _response.Result = await Client.PutAsJsonAsync(string.Format(CultureInfo.InvariantCulture, SolutionDescriptionUrl, solutionId), content).ConfigureAwait(false);
        }

        private class SolutionDescriptionPayload
        {
            [JsonProperty("summary")]
            public string Summary { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("link")]
            public string Link { get; set; }
        }
    }
}
