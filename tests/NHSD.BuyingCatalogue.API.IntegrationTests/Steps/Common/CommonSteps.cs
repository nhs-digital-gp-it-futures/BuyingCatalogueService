using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common
{
    [Binding]
    internal sealed class CommonSteps
    {
        private readonly Response _response;

        public CommonSteps(Response response)
        {
            _response = response;
        }

        [Then(@"the string value of element (.*) is (.*)")]
        public async Task ThenTheStringIs(string token, string requirement)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.Value<string>(token).Should().Be(requirement);
        }

        [Then(@"the (.*) string does not exist")]
        public async Task ThenTheBrowserHardwareRequirementsValueIsNull(string token)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.Value<string>(token).Should().Be(null);
        }
    }
}
