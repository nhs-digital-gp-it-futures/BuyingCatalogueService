using System.Globalization;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Solution
{
    [Binding]
    internal sealed class GetSolutionSteps
    {
        private const string GetSolutionUrl = "http://localhost:5200/api/v1/Solutions/{0}";

        private readonly Response _response;

        public GetSolutionSteps(Response response)
        {
            _response = response;
        }

        [When(@"a GET request is made to retrieve a solution by ID '(.*)'")]
        public async Task WhenAGetRequestIsMadeToRetrieveASolutionById(string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(CultureInfo.InvariantCulture, GetSolutionUrl, solutionId)).ConfigureAwait(false);
        }
    }
}
