using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.BrowserHardwareRequirements
{
    [Binding]
    internal sealed class BrowserHardwareRequirementSectionValidationSteps
    {
        private const string BrowserHardwareRequirementsUrl = "http://localhost:8080/api/v1/solutions/{0}/sections/browser-hardware-requirements";

        private readonly ScenarioContext _context;
        private readonly Response _response;

        public BrowserHardwareRequirementSectionValidationSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }
        
        [Given(@"hardware-requirements-description is a string of (.*) characters")]
        public void GiveHardwareRequirementsDescriptionIsAStringOfCharacters(int length)
        {
            _context["hardwareRequirements"] = new string('a', length);
        }

        [Given(@"hardware-requirements-description is null")]
        public void GivenHardware_Requirements_DescriptionIsNull()
        {
            _context["hardwareRequirements"] = null;
        }

        [When(@"a PUT request is made to update solution (.*) hardware-requirements-description section")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionSlnBrowserHardwareRequirementsSection(string solutionId)
        {
            var hardwareRequirements = _context["hardwareRequirements"]?.ToString();

            _response.Result = await Client.PutAsJsonAsync(string.Format(CultureInfo.InvariantCulture, BrowserHardwareRequirementsUrl, solutionId), new BrowserHardwareRequirementsPayload() { HardwareRequirements = hardwareRequirements }).ConfigureAwait(false);
        }

        [Then(@"the browser-hardware-requirements maxLength field contains (.*)")]
        public async Task ThenTheBrowserHardwareRequirementsMaxLengthFieldContainsRequired(string field)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken("maxLength").ToString().Should().Contain(field);
        }

        private class BrowserHardwareRequirementsPayload
        {
            [JsonProperty("hardware-requirements-description")]
            public string HardwareRequirements { get; set; }
        }
    }
}
