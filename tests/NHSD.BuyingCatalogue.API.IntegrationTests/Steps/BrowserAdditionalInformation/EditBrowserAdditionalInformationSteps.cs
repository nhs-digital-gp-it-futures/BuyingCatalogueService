using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.BrowserAdditionalInformation
{
    [Binding]
    internal sealed class EditBrowserAdditionalInformationSteps
    {
        private const string BrowserAdditionalInformationUrl = "http://localhost:8080/api/v1/solutions/{0}/sections/browser-additional-information";

        private readonly Response _response;

        public EditBrowserAdditionalInformationSteps(Response response)
        {
            _response = response;
        }

        [When(@"a PUT request is made to update solution (.*) browser-additional-information section")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionSlnBrowser_Additional_InformationSection(string solutionId, Table table)
        {
            var content = table.CreateInstance<BrowserAdditionalInformationSection>();

            _response.Result = await Client
                .PutAsJsonAsync(
                    string.Format(CultureInfo.InvariantCulture, BrowserAdditionalInformationUrl, solutionId),
                    new BrowserAdditionalInformationPayload() {AdditionalInformation = content.AdditionalInformation})
                .ConfigureAwait(false);
        }

        [When(@"a PUT request is made to update solution browser-additional-information section with no solution id")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionBrowser_Additional_InformationSectionWithNoSolutionId(Table table)
        {
            await WhenAPUTRequestIsMadeToUpdateSolutionSlnBrowser_Additional_InformationSection(" ", table).ConfigureAwait(false);
        }
        
        private class BrowserAdditionalInformationPayload
        {
            [JsonProperty("additional-information")]
            public string AdditionalInformation { get; set; }
        }

        private class BrowserAdditionalInformationSection
        {
            public string AdditionalInformation { get; set; }
        }
    }
}
