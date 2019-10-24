using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class GetSupportedBrowserSteps
    {
        private readonly ScenarioContext _context;

        private readonly Response _response;

        public GetSupportedBrowserSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [Then(@"the supported-browsers element contains")]
        public async Task ThenTheSupported_BrowsersElementContains(Table table)
        {
            var context = await _response.ReadBody();
            context.SelectToken("supported-browsers")
                .Select(s => s.ToString())
                .Should().BeEquivalentTo(table.CreateSet<SupportedBrowsersTable>().Select(s => s.BrowsersSupported));
        }

        private class SupportedBrowsersTable
        {
            public string BrowsersSupported { get; set; }
        }

    }
}
