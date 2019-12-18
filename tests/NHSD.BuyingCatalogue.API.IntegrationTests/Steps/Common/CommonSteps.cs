using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

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

        [Then(@"the (.*) element contains")]
        public async Task ThenTheSupportedBrowsersElementContains(string token, Table table)
        {
            var content = table.CreateInstance<ElementContainsStringTable>();
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken(token)
                .Select(s => s.ToString())
                .Should().BeEquivalentTo(content.Elements);
        }

        private class ElementContainsStringTable
        {
            public List<string> Elements { get; set; }
        }
    }
}
