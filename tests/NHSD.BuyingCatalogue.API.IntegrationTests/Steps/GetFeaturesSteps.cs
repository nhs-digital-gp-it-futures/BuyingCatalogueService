using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class GetFeaturesSteps
    {
        private readonly ScenarioContext _context;

        private readonly Response _response;

        public GetFeaturesSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [Then(@"the features element contains")]
        public async Task ThenTheFeaturesElementContains(Table table)
        {
            var content = await _response.ReadBody();
            content.SelectToken("listing")
                .Select(s => s.ToString())
                .Should().BeEquivalentTo(table.CreateSet<FeaturesTable>().Select(s => s.Features));
        }

        private class FeaturesTable
        {
            public string Features { get; set; }
        }
    }
}
