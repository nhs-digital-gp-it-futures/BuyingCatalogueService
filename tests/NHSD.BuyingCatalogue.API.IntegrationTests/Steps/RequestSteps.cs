using System.Net.Http;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class RequestSteps
    {
        private readonly ScenarioContext _context;

        private readonly Response _response;

        public RequestSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [Then(@"a successful response is returned")]
        public void ThenASuccessfulResponseIsReturned()
        {
            _response.Result.IsSuccessStatusCode.Should().BeTrue();
        }
    }
}
