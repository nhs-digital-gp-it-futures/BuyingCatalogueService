using System;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.MarketingContacts
{
    [Binding]
    internal sealed class MarketingContactSteps
    {
        [Given(@"MarketingContacts exist")]
        public async Task GivenMarketingContactsExist(Table table)
        {
            foreach (var contact in table.CreateSet<MarketingContactTable>())
            {
                await MarketingContactEntityBuilder.Create()
                    .WithSolutionId(contact.SolutionId)
                    .WithFirstName(contact.FirstName)
                    .WithLastName(contact.LastName)
                    .WithEmail(contact.Email)
                    .WithPhoneNumber(contact.PhoneNumber)
                    .WithDepartment(contact.Department)
                    .WithLastUpdated(DateTime.Now)
                    .WithLastUpdatedBy(Guid.NewGuid())
                    .Build()
                    .InsertAsync();
            }
        }

        [Given(@"No contacts exist for solution (.*)")]
        public async Task GivenNoContactsExist(string solutionId)
        {
            var contacts = await MarketingContactEntity.FetchForSolutionAsync(solutionId);
            contacts.Should().BeEmpty();
        }

        private class MarketingContactTable
        {
            public string SolutionId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Department { get; set; }
        }
    }
}
