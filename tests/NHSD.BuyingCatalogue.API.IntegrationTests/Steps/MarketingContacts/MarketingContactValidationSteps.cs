using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.MarketingContacts
{
    [Binding]
    internal sealed class MarketingContactValidationSteps
    {
        private readonly Response response;

        public MarketingContactValidationSteps(Response response)
        {
            this.response = response;
        }

        [Then(@"the solutions (contact-1|contact-2) has details")]
        public async Task ThenTheSolutionContactHasDetails(string contact, Table table)
        {
            var expected = table.CreateSet<ContactDetailsTable>().Single();
            var content = await response.ReadBody();
            var contactDetails = content.SelectToken($"sections.contact-details.answers.{contact}");

            Assert.NotNull(contactDetails);
            contactDetails.SelectToken("contact-name")?.ToString().Should().BeEquivalentTo(expected.Name);
            contactDetails.SelectToken("email-address")?.ToString().Should().BeEquivalentTo(expected.Email);
            contactDetails.SelectToken("phone-number")?.ToString().Should().BeEquivalentTo(expected.PhoneNumber);
            contactDetails.SelectToken("department-name")?.ToString().Should().BeEquivalentTo(expected.Department);
        }

        [Then(@"there is no (contact-1|contact-2|contact-3) for the solution")]
        public async Task ThenThereIsNoContactForTheSolution(string contact)
        {
            var content = await response.ReadBody();
            var contactDetails = content.SelectToken($"sections.contact-details.answers.{contact}");
            contactDetails.Should().BeNull();
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class ContactDetailsTable
        {
            public string Name { get; init; }

            public string Email { get; init; }

            public string PhoneNumber { get; init; }

            public string Department { get; init; }
        }
    }
}
