using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.ContactDetails
{
    [Binding]
    internal class ContactDetailsUpdateSteps
    {
        private const string ContactDetailsUrl = "http://localhost:8080/api/v1/solutions/{0}/sections/contact-details";

        private readonly Response _response;

        public ContactDetailsUpdateSteps(Response response)
        {
            _response = response;
        }

        [When(@"a PUT request is made for solution (.*) contact details")]
        public async Task WhenGetRequestIsMadeToDisplaySolutionContactDetailsSections(string solutionId, Table table)
        {
            var contacts = table.CreateSet<ContactDetailsRequestTable>();
            _response.Result = await Client.PutAsJsonAsync(string.Format(CultureInfo.InvariantCulture, ContactDetailsUrl, solutionId), new ContactDetailsPayload { Contact1 = contacts.FirstOrDefault(), Contact2 = contacts.Skip(1).FirstOrDefault() }).ConfigureAwait(false);
        }

        [When(@"a PUT request is made for empty solution (.*) contact details")]
        public async Task WhenGetRequestIsMadeToDisplaySolutionContactDetailsSections(string solutionId)
        {
            _response.Result = await Client.PutAsJsonAsync(string.Format(CultureInfo.InvariantCulture, ContactDetailsUrl, solutionId), new ContactDetailsPayload { Contact1 = null, Contact2 = null }).ConfigureAwait(false);
        }

        [When(@"a PUT request is made to update solution contact details with no solution id")]
        public async Task WhenAPutRequestIsMadeToUpdateContactDetailsWithNoSolutionId(Table table)
        {
            await WhenGetRequestIsMadeToDisplaySolutionContactDetailsSections(" ", table).ConfigureAwait(false);
        }

        private class ContactDetailsPayload
        {
            [JsonProperty("contact-1")]
            public ContactDetailsRequestTable Contact1 { get; set; }

            [JsonProperty("contact-2")]
            public ContactDetailsRequestTable Contact2 { get; set; }
        }

        private class ContactDetailsRequestTable
        {
            [JsonProperty("department-name")]
            public string Department { get; set; }

            [JsonProperty("first-name")]
            public string FirstName { get; set; }

            [JsonProperty("last-name")]
            public string LastName { get; set; }

            [JsonProperty("phone-number")]
            public string PhoneNumber { get; set; }

            [JsonProperty("email-address")]
            public string Email { get; set; }

        }
    }
}
