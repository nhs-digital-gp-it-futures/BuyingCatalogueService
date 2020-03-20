using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    internal sealed class MarketingContactEntitySteps
    {
        [Given(@"MarketingContacts exist")]
        public async Task GivenMarketingContactsExist(Table table)
        {
            foreach (var contact in table.CreateSet<MarketingContactEntity>())
            {
                if (contact.LastUpdated == DateTime.MinValue)
                {
                    contact.LastUpdated = DateTime.UtcNow;
                }
                await contact.InsertAsync().ConfigureAwait(false);
            }
        }

        [Given(@"No contacts exist for solution (.*)")]
        [Then(@"No contacts exist for solution (.*)")]
        public async Task NoContactsExist(string solutionId)
        {
            var contacts = await MarketingContactEntity.FetchForSolutionAsync(solutionId).ConfigureAwait(false);
            contacts.Should().BeEmpty();
        }

        [Then(@"Last Updated has updated on the MarketingContact for solution (.*)")]
        public async Task LastUpdatedHasUpdatedOnMarketingContact(string solutionId)
        {
            var contacts = (await MarketingContactEntity.FetchForSolutionAsync(solutionId).ConfigureAwait(false)).ToList();
            
            contacts.ForEach(async x => (await x.LastUpdated.SecondsFromNow().ConfigureAwait(false)).Should().BeLessOrEqualTo(5));
        }

        [Then(@"MarketingContacts exist for solution (.*)")]
        public async Task ThenMarketingContactsExist(string solutionId, Table table)
        {
            var expected = table.CreateSet<MarketingContactEntity>().ToList();
            var contacts = await MarketingContactEntity.FetchForSolutionAsync(solutionId).ConfigureAwait(false);

            IEnumerable<MarketingContactEntity> contactList = contacts.ToList();
            contactList.Count().Should().Be(expected.Count);
            contactList.Should().BeEquivalentTo(expected, config => config.Excluding(c => c.LastUpdated).Excluding(c => c.LastUpdatedBy).Excluding(c => c.SolutionId));
        }
    }
}
