using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.BrowserAdditionalInformation
{
    [Binding]
    internal sealed class BrowserAdditionalInformationValidationSteps
    {
        private const string BrowserAdditionalInformationUrl = "http://localhost:8080/api/v1/solutions/{0}/sections/browser-additional-information";

        private readonly Response _response;

        public BrowserAdditionalInformationValidationSteps(Response response)
        {
            _response = response;
        }

        [When(@"a PUT request is made to update solution (.*) additional-information section")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionSlnAdditional_InformationSection(string solutionId, Table table)
        {
            var content = table.CreateInstance<BrowserAdditionalInformationPayload>();
            _response.Result = await Client
                .PutAsJsonAsync(
                    string.Format(CultureInfo.InvariantCulture, BrowserAdditionalInformationUrl, solutionId), content)
                .ConfigureAwait(false);
        }

        [Then(@"the additional-information maxLength field contains (.*)")]
        public async Task ThenTheAdditional_InformationMaxLengthFieldContainsAdditional_Information(string field)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken("maxLength").ToString().Should().Contain(field);
        }
        
        private class BrowserAdditionalInformationPayload
        {
            [JsonProperty("additional-information")]
            public string AdditionalInformation { get; set; }
        }
    }
}
