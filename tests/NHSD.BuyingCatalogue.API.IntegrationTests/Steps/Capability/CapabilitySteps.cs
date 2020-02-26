using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Capability
{
    [Binding]
    internal sealed class CapabilitySteps
    {
        private const string ListCapabilitiesUrl = "http://localhost:5200/api/v1/Capabilities";

        private readonly Response _response;

        public CapabilitySteps(Response response)
        {
            _response = response;
        }

        [When(@"a GET request is made for the capability list")]
        public async Task WhenAGETRequestIsMadeForTheCapabilityList()
        {
            _response.Result = await Client.GetAsync(ListCapabilitiesUrl).ConfigureAwait(false);
        }

        [Then(@"the capabilities are returned ordered by IsFoundation then Capability Name containing the values")]
        public async Task ThenTheCapabilitiesAreReturnedOrderedByIsFoundationThenCapabilityName(Table table)
        {
            var expectedCapabilities = table.CreateSet<CapabilityTable>().Select(t => new
            {
                CapabilityRef = t.CapabilityRef,
                Version = t.Version,
                Name = t.CapabilityName,
                IsFoundation = t.IsFoundation
            });

            var capabilities = (await _response.ReadBody().ConfigureAwait(false))
                .SelectToken("capabilities")
                .Select(t => new
                {
                    CapabilityRef = t.SelectToken("reference").ToString(),
                    Version = t.SelectToken("version").ToString(),
                    Name = t.SelectToken("name").ToString(),
                    IsFoundation = Convert.ToBoolean(t.SelectToken("isFoundation").ToString(), CultureInfo.InvariantCulture)
                });

            capabilities.Should().BeEquivalentTo(expectedCapabilities, options => options.WithStrictOrdering());
        }

        private class CapabilityTable
        {
            public string CapabilityRef { get; set; }

            public string Version { get; set; }

            public string CapabilityName { get; set; }

            public bool IsFoundation { get; set; }
        }
    }
}
