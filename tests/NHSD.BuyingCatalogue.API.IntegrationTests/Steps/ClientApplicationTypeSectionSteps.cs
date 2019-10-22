using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class ClientApplicationTypeSectionSteps
    {
        private readonly ScenarioContext _context;

        private readonly Response _response;

        public ClientApplicationTypeSectionSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [Then(@"the solution client-application-types section contains clientapplication")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsClientApplication(Table table)
        {
            var content = await _response.ReadBody();
            content.SelectToken("solution.marketingData.sections[?(@.id == 'client-application-types')].sections")
                .Select(s => s.SelectToken("id").ToString()).Should().BeEquivalentTo(table.CreateSet<ClientApplicationTypesTable>().Select(s => s.ClientApplication));
        }

        private class ClientApplicationTypesTable
        {
            public string ClientApplication { get; set; }
        }
    }
}
