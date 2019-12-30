using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common
{
    [Binding]
    internal sealed class ResponseValidationSteps
    {
        private readonly Response _response;

        public ResponseValidationSteps(Response response) => _response = response;

        [Then(@"the (.*) field value is the validation failure (required|maxLength)")]
        public async Task ThenTheFieldContainsValidationResult(string token, string validationError)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken(token).ToString().Should().Be(validationError);
        }
    }
}
