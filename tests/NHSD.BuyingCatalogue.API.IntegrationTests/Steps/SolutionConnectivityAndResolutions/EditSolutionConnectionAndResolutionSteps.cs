using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.SolutionConnectivityAndResolutions
{
    [Binding]
    internal sealed class EditSolutionConnectionAndResolutionSteps
    {
        private const string Url = "http://localhost:8080/api/v1/solutions/{0}/sections/connectivity-and-resolution";

        private readonly Response _response;

        public EditSolutionConnectionAndResolutionSteps(Response response)
        {
            _response = response;
        }

        [When(@"a PUT request is made to update solution (.*) connectivity-and-resolution section")]
        public async Task WhenAPutRequestIsMadeToUpdateSection(string solutionId, Table table)
        {
            var content = table.CreateInstance<StepTable>();
            _response.Result = await Client.PutAsJsonAsync(string.Format(CultureInfo.InvariantCulture, Url, solutionId), content).ConfigureAwait(false);
        }

        [When(@"a PUT request is made to update solution connectivity-and-resolution section with no solution id")]
        public async Task WhenAPutRequestIsMadeWithNoSolutionId(Table table)
        {
            await WhenAPutRequestIsMadeToUpdateSection(" ", table).ConfigureAwait(false);
        }

        [Then(@"the (minimum-connection-speed|minimum-desktop-resolution) field equals (.*)")]
        public async Task ThenTheFieldEquals(string token, string field)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken(token).ToString().Should().Be(field);
        }

        private class StepTable
        {
            [JsonProperty("minimum-connection-speed")]
            public string MinimumConnectionSpeed { get; set; }

            [JsonProperty("minimum-desktop-resolution")]
            public string MinimumDesktopResolution { get; set; }
        }
    }
}
