using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class SolutionDescriptionSectionSteps
    {
        private const string SolutionDescriptionUrl = "http://localhost:8080/api/v1/Solutions/{0}/sections/solution-description";

        private readonly ScenarioContext _context;

        private readonly Response _response;

        public SolutionDescriptionSectionSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [When(@"a PUT request is made to update solution (.*) description section")]
        public async Task WhenAPUTTRequestIsMadeToUpdateSolutionDescriptionSection(string solutionId, Table table)
        {
            var content = table.CreateInstance<SolutionDescriptionPostTable>();

            _response.Result = await Client.PutAsJsonAsync(string.Format(SolutionDescriptionUrl, solutionId), content);
        }

        private class SolutionDescriptionPostTable
        {
            public string Summary { get; set; }

            public string Description { get; set; }

            public string Link { get; set; }
        }
    }
}
