using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.BrowserAdditionalInformation
{
    [Binding]
    internal sealed class BrowserAdditionalInformationValidationSteps
    {
        private const string BrowserAdditionalInformationUrl = "http://localhost:8080/api/v1/solutions/{0}/sections/browser-additional-information";

        private readonly ScenarioContext _context;
        private readonly Response _response;

        public BrowserAdditionalInformationValidationSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [Given(@"additional-information on BrowserAdditionalInformation is a string of (.*) characters")]
        public void GivenAdditional_InformationOnBrowserAdditionalInformationIsAStringOfCharacters(int length)
        {
            _context["browserAdditionalInformation"] = new string('a', length);
        }

        [Given(@"additional-information on BrowserAdditionalInformation is null")]
        public void GivenAdditional_InformationOnBrowserAdditionalInformationIsNull()
        {
            _context["browserAdditionalInformation"] = null;
        }

        [When(@"a PUT request is made to update solution (.*) additional-information section")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionSlnAdditional_InformationSection(string solutionId)
        {
            var browserAdditionalInformation = _context["browserAdditionalInformation"]?.ToString();

            _response.Result = await Client
                .PutAsJsonAsync(
                    string.Format(CultureInfo.InvariantCulture, BrowserAdditionalInformationUrl, solutionId),
                    new BrowserAdditionalInformationPayload() {AdditionalInformation = browserAdditionalInformation})
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
