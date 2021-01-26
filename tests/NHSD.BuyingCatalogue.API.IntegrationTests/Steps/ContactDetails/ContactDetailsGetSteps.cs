using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.ContactDetails
{
    [Binding]
    internal sealed class ContactDetailsGetSteps
    {
        private readonly Response response;

        public ContactDetailsGetSteps(Response response)
        {
            this.response = response;
        }

        [Then(@"the contact-detail (contact-1|contact-2) has details")]
        public async Task ThenTheContact_DetailContactHasDetails(string contact, Table table)
        {
            var expected = table.CreateSet<ContactDetailsResultTable>().Single();
            var content = await response.ReadBody();
            var contactDetails = content.SelectToken($"{contact}");

            Assert.NotNull(contactDetails);
            contactDetails.SelectToken("department-name")?.ToString().Should().BeEquivalentTo(expected.DepartmentName);
            contactDetails.SelectToken("first-name")?.ToString().Should().BeEquivalentTo(expected.FirstName);
            contactDetails.SelectToken("last-name")?.ToString().Should().BeEquivalentTo(expected.LastName);
            contactDetails.SelectToken("phone-number")?.ToString().Should().BeEquivalentTo(expected.PhoneNumber);
            contactDetails.SelectToken("email-address")?.ToString().Should().BeEquivalentTo(expected.EmailAddress);
        }

        [Then(@"there is no (contact-1|contact-2|contact-3) for the contact-detail")]
        public async Task ThenThereIsNoContactForTheContact_Detail(string contact)
        {
            var content = await response.ReadBody();
            var contactDetails = content.SelectToken($"{contact}");
            contactDetails.Should().BeNull();
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class ContactDetailsResultTable
        {
            public string DepartmentName { get; init; }

            public string FirstName { get; init; }

            public string LastName { get; init; }

            public string PhoneNumber { get; init; }

            public string EmailAddress { get; init; }
        }
    }
}
