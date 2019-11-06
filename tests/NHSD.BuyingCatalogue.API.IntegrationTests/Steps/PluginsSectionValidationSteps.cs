using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
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

        [Given(@"additional-information is a string of (.*) characters")]
        public void GivenAdditionalInfoIsAStringOfCharacters(int length)
        {
            _context["required"] = new string('a', length);
        }

        [Given(@"plug-ins is a string of (yes|no|null)")]
        public void GivenPluginsIsNull(string field)
        {
            _context["additionalInfo"] = string.IsNullOrWhiteSpace(field) ? null : field;
        }

        [When(@"a PUT request is made to update solution (.*) plug-ins section")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionSlnPlug_InsSection(string solutionId)
        {
            var content = new
            {
                AdditionalInformation = _context["required"],
                Required = _context["additionalInfo"]
            };

            _response.Result = await Client.PutAsJsonAsync(string.Format(PluginsUrl, solutionId), content);
        }

        [Then(@"the plug-ins required field contains (.*)")]
        public async Task ThenThePlug_InsRequiredFieldContainsRequired(string field)
        {
            var context = await _response.ReadBody();
            context.SelectToken("required").ToString().Should().Contain(field);
        }

        [Then(@"the plug-ins maxLength field contains (.*)")]
        public async Task ThenThePlug_InsMaxLengthFieldContainsRequired(string field)
        {
            var context = await _response.ReadBody();
            context.SelectToken("maxLength").ToString().Should().Contain(field);
        }
    }
}
