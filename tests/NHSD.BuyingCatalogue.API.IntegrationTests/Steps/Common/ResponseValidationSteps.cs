using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common
{
    [Binding]
    internal sealed class ResponseValidationSteps
    {
        private readonly Response _response;

        public ResponseValidationSteps(Response response)
        {
            _response = response;
        }

        //This step will eventually be obsolete and replaced by the steps below 
        [Then(@"the (required|maxLength) field only contains (.*)")]
        public async Task ThenTheFieldOnlyContains(string token, List<string> listing)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken(token).Select(x => x.ToString()).Should().BeEquivalentTo(listing);
        }

        //This step will eventually be obsolete and replaced by the steps below 
        [Then(@"the (required|maxLength) field contains (.*)")]
        public async Task ThenTheFieldContains(string token, List<string> listing)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken(token).Select(x => x.ToString()).Should().Contain(listing);
        }

        //This step will eventually be obsolete and replaced by the steps below 
        [Then(@"the (required|maxLength) field does not contain (.*)")]
        public async Task ThenTheRequiredFieldDoesNotContain(string token, string field)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken(token).Select(x => x.ToString()).Should().NotContain(field);
        }

        [Then(@"the (.*) field value is the validation failure (required|maxLength)")]
        public async Task ThenTheFieldContainsValidationResult(string token, string validationError)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken(token).ToString().Should().Be(validationError);
        }
    }
}
