using System.Globalization;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Solution
{
    [Binding]
    internal sealed class FoundationSolutionSteps
    {
        private const string foundationSolutionUrl = "http://localhost:5200/api/v1/Solutions/foundation";

        private readonly Response _response;

        public FoundationSolutionSteps(Response response)
        {
            _response = response;
        }

        [When(@"a GET request is made for foundation solutions")]
        public async Task WhenAGetRequestIsMadeForFoundation_Solutions()
        {
            _response.Result = await Client.GetAsync(string.Format(CultureInfo.InvariantCulture, foundationSolutionUrl)).ConfigureAwait(false);
        }
    }

}
