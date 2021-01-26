using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.ContactDetails
{
    [Binding]
    internal class ContactDetailsUpdateSteps
    {
        private const string ContactDetailsUrl = "http://localhost:5200/api/v1/solutions/{0}/sections/contact-details";

        private readonly Response response;

        public ContactDetailsUpdateSteps(Response response)
        {
            this.response = response;
        }

        [When(@"a PUT request is made for solution (.*) contact details")]
        public async Task WhenGetRequestIsMadeToDisplaySolutionContactDetailsSections(string solutionId, Table table)
        {
            var contacts = table.CreateSet<ContactDetailsRequestTable>().ToList();

            response.Result = await Client.PutAsJsonAsync(
                string.Format(CultureInfo.InvariantCulture, ContactDetailsUrl, solutionId),
                new ContactDetailsPayload
                {
                    Contact1 = contacts.FirstOrDefault(),
                    Contact2 = contacts.Skip(1).FirstOrDefault(),
                });
        }

        [When(@"a PUT request is made for empty solution (.*) contact details")]
        public async Task WhenGetRequestIsMadeToDisplaySolutionContactDetailsSections(string solutionId)
        {
            response.Result = await Client.PutAsJsonAsync(
                string.Format(CultureInfo.InvariantCulture, ContactDetailsUrl, solutionId),
                new ContactDetailsPayload { Contact1 = null, Contact2 = null });
        }

        [When(@"a PUT request is made to update solution contact details with no solution id")]
        public async Task WhenAPutRequestIsMadeToUpdateContactDetailsWithNoSolutionId(Table table)
        {
            await WhenGetRequestIsMadeToDisplaySolutionContactDetailsSections(" ", table);
        }

        private sealed class ContactDetailsPayload
        {
            [JsonProperty("contact-1")]
            public ContactDetailsRequestTable Contact1 { get; init; }

            [JsonProperty("contact-2")]
            public ContactDetailsRequestTable Contact2 { get; init; }
        }

        private sealed class ContactDetailsRequestTable
        {
            [JsonProperty("department-name")]
            public string Department { get; init; }

            [JsonProperty("first-name")]
            public string FirstName { get; init; }

            [JsonProperty("last-name")]
            public string LastName { get; init; }

            [JsonProperty("phone-number")]
            public string PhoneNumber { get; init; }

            [JsonProperty("email-address")]
            public string Email { get; init; }
        }
    }
}
