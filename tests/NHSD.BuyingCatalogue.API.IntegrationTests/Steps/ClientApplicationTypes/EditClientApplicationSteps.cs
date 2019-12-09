using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class EditClientApplicationSteps
    {
        private const string ClientApplicationTypeUrl = "http://localhost:8080/api/v1/solutions/{0}/sections/client-application-types";

        private readonly Response _response;

        public EditClientApplicationSteps(Response response)
        {
            _response = response;
        }

        [When(@"a PUT request is made to update solution (.*) client-application-types section")]
        public async Task WhenAPutRequestIsMadeToUpdateClientApplicationTypesSection(string solutionId, Table table)
        {
            var content = table.CreateInstance<ClientApplicationTypeTable>();
            _response.Result = await Client.PutAsJsonAsync(string.Format(CultureInfo.InvariantCulture, ClientApplicationTypeUrl, solutionId), new ClientApplicationTypesPayload { ClientApplicationTypes = content.ClientApplicationTypes }).ConfigureAwait(false);
        }

        private class ClientApplicationTypesPayload
        {
            [JsonProperty("client-application-types")]
            public List<string> ClientApplicationTypes { get; set; }
        }

        [When(@"a PUT request is made to update solution client-application-types section with no solution id")]
        public async Task WhenAPutRequestIsMadeToUpdateClientApplicationTypesSectionWithNoSolutionId(Table table)
        {
            await WhenAPutRequestIsMadeToUpdateClientApplicationTypesSection(" ", table).ConfigureAwait(false);
        }

        [Then(@"the client-application-types required field contains (.*)")]
        public async Task ThenTheClient_Application_TypesRequiredFieldContainsClient_Application_Types(string field)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("required").ToString().Should().Contain(field);
        }

        private class ClientApplicationTypeTable
        {
            public List<string> ClientApplicationTypes { get; set; }
        }
    }
}
