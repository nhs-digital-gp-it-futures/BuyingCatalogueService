using System.Net;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common
{
    [Binding]
    internal sealed class ResponseSteps
    {
        private readonly Response response;

        public ResponseSteps(Response response)
        {
            this.response = response;
        }

        [Then(@"a successful response is returned")]
        public void ThenASuccessfulResponseIsReturned()
        {
            response.Result.IsSuccessStatusCode.Should().BeTrue();
        }

        [Then(@"a response status of (.*) is returned")]
        public void ThenAResponseStatusIsReturned(HttpStatusCode httpStatusCode)
        {
            response.Result.StatusCode.Should().Be(httpStatusCode);
        }
    }
}
