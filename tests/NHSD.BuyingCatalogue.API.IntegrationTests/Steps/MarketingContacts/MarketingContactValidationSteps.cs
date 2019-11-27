using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.MarketingContacts
{
    [Binding]
    internal sealed class MarketingContactValidationSteps
    {
        private readonly Response _response;

        public MarketingContactValidationSteps(Response response)
        {
            _response = response;
        }

        [Then(@"the solutions (contact-1|contact-2) has details")]
        public async Task ThenTheSolutionContactHasDetails(string contact, Table table)
        {
            var expected = table.CreateSet<ContactDetailsTable>().Single();
            var content = await _response.ReadBody();
            var contactDetails = content.SelectToken($"sections.contact-details.answers.{contact}");
            contactDetails.SelectToken("contact-name")?.ToString().Should().BeEquivalentTo(expected.Name);
            contactDetails.SelectToken("email-address")?.ToString().Should().BeEquivalentTo(expected.Email);
            contactDetails.SelectToken("phone-number")?.ToString().Should().BeEquivalentTo(expected.PhoneNumber);
            contactDetails.SelectToken("department-name")?.ToString().Should().BeEquivalentTo(expected.Department);
        }

        [Then(@"there is no (contact-1|contact-2|contact-3) for the solution")]
        public async Task ThenThereIsNoContactForTheSolution(string contact)
        {
            var content = await _response.ReadBody();
            var contactDetails = content.SelectToken($"sections.contact-details.answers.{contact}");
            contactDetails.Should().BeNull();
        }

        private class ContactDetailsTable
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Department { get; set; }
        }
    }
}
