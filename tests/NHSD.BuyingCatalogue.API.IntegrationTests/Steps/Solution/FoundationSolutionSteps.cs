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
        private const string FoundationSolutionUrl = "http://localhost:5200/api/v1/Solutions/foundation";

        private readonly Response response;

        public FoundationSolutionSteps(Response response)
        {
            this.response = response;
        }

        [When(@"a GET request is made for foundation solutions")]
        public async Task WhenAGetRequestIsMadeForFoundation_Solutions()
        {
            response.Result = await Client.GetAsync(string.Format(CultureInfo.InvariantCulture, FoundationSolutionUrl));
        }
    }
}
