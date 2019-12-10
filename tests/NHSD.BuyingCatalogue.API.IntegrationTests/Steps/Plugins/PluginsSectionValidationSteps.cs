using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Plugins
{
    [Binding]
    internal sealed class PluginsSectionValidationSteps
    {
        private const string PluginsUrl = "http://localhost:8080/api/v1/solutions/{0}/sections/plug-ins-or-extensions";

        private readonly ScenarioContext _context;
        private readonly Response _response;

        public PluginsSectionValidationSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [Given(@"plug-ins is a string of (Yes|No)")]
        public void GivenPluginsIsBool(string field)
        {
            _context["required"] = field;
        }

        [Given(@"plug-ins is a string of null")]
        public void GivenPluginIsNull()
        {
            GivenPluginsIsBool(null);
        }

        [Given(@"additional-information is a string of (.*) characters")]
        public void GivenAdditionalInfoIsAStringOfCharacters(int length)
        {
            _context["additionalInfo"] = new string('a', length);
        }

        [When(@"a PUT request is made to update solution (.*) plug-ins section")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionSlnPlug_InsSection(string solutionId)
        {
            var required = _context["required"]?.ToString();
            var additionalInformation = _context["additionalInfo"].ToString();

            _response.Result = await Client.PutAsJsonAsync(string.Format(CultureInfo.InvariantCulture, PluginsUrl, solutionId), new PluginsPayload() { PluginsRequired = required, PluginsDetail = additionalInformation }).ConfigureAwait(false);
        }

        [Then(@"the plug-ins required field contains (.*)")]
        public async Task ThenThePlug_InsRequiredFieldContainsRequired(string field)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken("required").ToString().Should().Contain(field);
        }

        [Then(@"the plug-ins maxLength field contains (.*)")]
        public async Task ThenThePlug_InsMaxLengthFieldContainsRequired(string field)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken("maxLength").ToString().Should().Contain(field);
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
