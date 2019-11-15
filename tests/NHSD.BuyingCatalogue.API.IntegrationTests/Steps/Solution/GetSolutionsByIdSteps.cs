using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class GetSolutionsByIdSteps
    {
        private const string ByIdSolutionsUrl = "http://localhost:8080/api/v1/Solutions/{0}/{1}";

        private readonly ScenarioContext _context;

        private readonly Response _response;

        public GetSolutionsByIdSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [When(@"a GET request is made for (dashboard|preview|public) with no solution id")]
        public async Task WhenAGETRequestIsMadeForSolutionSlnNoId(string view)
        {
            await WhenAGETRequestIsMadeForSolutionSln(view, " ");
        }

        [When(@"a GET request is made for solution (dashboard|preview|public) (.*)")]
        public async Task WhenAGETRequestIsMadeForSolutionSln(string view, string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(ByIdSolutionsUrl, solutionId, view));
        }
    }
}
