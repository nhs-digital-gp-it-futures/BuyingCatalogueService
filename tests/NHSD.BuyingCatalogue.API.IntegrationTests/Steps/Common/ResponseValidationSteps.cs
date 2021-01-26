using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common
{
    [Binding]
    internal sealed class ResponseValidationSteps
    {
        private readonly Response response;

        public ResponseValidationSteps(Response response) => this.response = response;

        [Then(@"the (.*) field value is the validation failure (required|maxLength|capabilityInvalid|epicsInvalid)")]
        public async Task ThenTheFieldContainsValidationResult(string token, string validationError)
        {
            var content = await response.ReadBody();
            JToken selectedToken = content.SelectToken(token);

            Assert.NotNull(selectedToken);
            selectedToken.ToString().Should().Be(validationError);
        }
    }
}
