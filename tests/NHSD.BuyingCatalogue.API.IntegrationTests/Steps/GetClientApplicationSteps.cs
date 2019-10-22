using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class GetClientApplicationSteps
    {
        private const string ClientApplicationTypesUrl = "http://localhost:8080/api/v1/Solutions/{0}/sections/client-application-types";

        private readonly ScenarioContext _context;

        private readonly Response _response;

        public GetClientApplicationSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [When(@"a GET request is made for client-application-types with no solution id")]
        public async Task GetRequestClientApplicationNoSolutionId()
        {
            await GetRequestClientApplication(" ");
        }
        
        [When(@"a GET request is made for client-application-types for solution (.*)")]
        public async Task GetRequestClientApplication(string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(ClientApplicationTypesUrl, solutionId));
        }

        [Then(@"the client-application-types element contains")]
        public async Task ClientApplicationContains(Table table)
        {
            var content = await _response.ReadBody();
            content.SelectToken("client-application-types")
                .Select(s => s.ToString())
                .Should().BeEquivalentTo(table.CreateSet<ClientApplicationTypesTable>().Select(s => s.ClientApplicationTypes));
        }

        private class ClientApplicationTypesTable
        {
            public string ClientApplicationTypes { get; set; }
        }
    }
}
