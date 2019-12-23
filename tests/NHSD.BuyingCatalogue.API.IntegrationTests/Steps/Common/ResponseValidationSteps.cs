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

        [Then(@"the (required|maxLength) field only contains (.*)")]
        public async Task ThenTheFieldOnlyContains(string token, List<string> listing)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken(token).Select(x => x.ToString()).Should().BeEquivalentTo(listing);
        }

        [Then(@"the (required|maxLength) field contains (.*)")]
        public async Task ThenTheFieldContains(string token, List<string> listing)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken(token).Select(x => x.ToString()).Should().Contain(listing);
        }

        [Then(@"the (required|maxLength) field does not contain (.*)")]
        public async Task ThenTheRequiredFieldDoesNotContain(string token, string field)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken(token).Select(x => x.ToString()).Should().NotContain(field);
        }
    }
}
