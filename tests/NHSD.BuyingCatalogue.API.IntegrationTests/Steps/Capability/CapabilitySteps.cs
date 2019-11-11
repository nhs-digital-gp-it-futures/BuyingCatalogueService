using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class CapabilitySteps
    {
        private const string ListCapabilitiesUrl = "http://localhost:8080/api/v1/Capabilities";

        private readonly ScenarioContext _context;

        private readonly Response _response;

        public CapabilitySteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [When(@"a GET request is made for the capability list")]
        public async Task WhenAGETRequestIsMadeForTheCapabilityList()
        {
            _response.Result = await Client.GetAsync(ListCapabilitiesUrl);
        }

        [Then(@"the capabilities are returned ordered by IsFoundation then Capability Name")]
        public async Task ThenTheCapabilitiesAreReturnedOrderedByIsFoundationThenCapabilityName(Table table)
        {
            var storedCapabilities = await CapabilityEntity.FetchAllAsync();

            var expectedCapabilities = table.CreateSet<CapabilityTable>().Select(t => new
            {
                Id = storedCapabilities.First(c => c.Name == t.CapabilityName).Id,
                Name = t.CapabilityName,
                IsFoundation = t.IsFoundation
            });

            var capabilities = (await _response.ReadBody())
                .SelectToken("capabilities")
                .Select(t => new
                {
                    Id = Guid.Parse(t.SelectToken("id").ToString()),
                    Name = t.SelectToken("name").ToString(),
                    IsFoundation = Convert.ToBoolean(t.SelectToken("isFoundation").ToString())
                });

            capabilities.Should().BeEquivalentTo(expectedCapabilities, options => options.WithStrictOrdering());
        }

        private class CapabilityTable
        {
            public string CapabilityName { get; set; }

            public bool IsFoundation { get; set; }
        }
    }
}
