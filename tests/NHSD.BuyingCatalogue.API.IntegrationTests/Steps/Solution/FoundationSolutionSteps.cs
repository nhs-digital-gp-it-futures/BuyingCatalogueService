using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Solution
{
    [Binding]
    internal sealed class FoundationSolutionSteps
    {
        private const string foundationSolutionUrl = "http://localhost:8080/api/v1/Solutions/{0}/foundation";

        private readonly Response _response;

        public FoundationSolutionSteps(Response response)
        {
            _response = response;
        }

        [When(@"a get request is made for foundation-solution for (.*)")]
        public async Task WhenAGetRequestIsMadeForFoundation_SolutionForSln(string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(foundationSolutionUrl, solutionId));
        }
    }

}
