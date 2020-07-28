using System.Net;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common
{
    [Binding]
    internal sealed class ResponseSteps
    {
        private readonly Response _response;

        public ResponseSteps(Response response)
        {
            _response = response;
        }

        [Then(@"a successful response is returned")]
        public void ThenASuccessfulResponseIsReturned()
        {
            _response.Result.IsSuccessStatusCode.Should().BeTrue();
        }

        [Then(@"a response status of (.*) is returned")]
        public void ThenAResponseStatusIsReturned(HttpStatusCode httpStatusCode)
        {
            _response.Result.StatusCode.Should().Be(httpStatusCode);
        }
    }
}
