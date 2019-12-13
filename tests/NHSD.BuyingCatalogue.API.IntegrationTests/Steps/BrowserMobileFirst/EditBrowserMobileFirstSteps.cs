using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.BrowserMobileFirst
{
    [Binding]
    internal sealed class EditBrowserMobileFirstSteps
    {
        private const string BrowserMobileFirstUrl = "http://localhost:8080/api/v1/solutions/{0}/sections/browser-mobile-first";

        private readonly Response _response;

        public EditBrowserMobileFirstSteps(Response response)
        {
            _response = response;
        }

        [When(@"a PUT request is made to update solution (.*) browser-mobile-first section")]
        public async Task WhenAputRequestIsMadeToUpdateSolutionSlnBrowserMobileFirstSection(string solutionId, Table table)
        {
            var content = table.CreateInstance<BrowserMobileFirstPayload>();

            _response.Result = await Client
                .PutAsJsonAsync(string.Format(CultureInfo.InvariantCulture, BrowserMobileFirstUrl, solutionId), content)
                .ConfigureAwait(false);
        }

        [When(@"a PUT request is made to update solution browser-mobile-first section with no solution id")]
        public async Task WhenAputRequestIsMadeToUpdateSolutionBrowserMobileFirstSectionWithNoSolutionId(Table table)
        {
            await WhenAputRequestIsMadeToUpdateSolutionSlnBrowserMobileFirstSection(" ", table).ConfigureAwait(false);
        }

        private class BrowserMobileFirstPayload
        {
            [JsonProperty("mobile-first-design")]
            public string MobileFirstDesign { get; set; }
        }
    }
}
