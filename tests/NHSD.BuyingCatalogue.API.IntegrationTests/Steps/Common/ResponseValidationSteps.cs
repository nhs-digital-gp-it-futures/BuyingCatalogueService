using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common
{
    [Binding]
    internal sealed class ResponseValidationSteps
    {
        private readonly Response _response;

        public ResponseValidationSteps(Response response) => _response = response;

        [Then(@"the (.*) field value is the validation failure (required|maxLength|capabilityInvalid)")]
        public async Task ThenTheFieldContainsValidationResult(string token, string validationError)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            JToken selectedToken = content.SelectToken(token);

            selectedToken.Should().NotBeNull();
            selectedToken.ToString().Should().Be(validationError);
        }
    }
}
