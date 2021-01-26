using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
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

        private readonly Response response;

        public CapabilitySteps(Response response)
        {
            this.response = response;
        }

        [When(@"a GET request is made for the capability list")]
        public async Task WhenAGetRequestIsMadeForTheCapabilityList()
        {
            response.Result = await Client.GetAsync(ListCapabilitiesUrl);
        }

        [Then(@"the capabilities are returned ordered by Capability Name containing the values")]
        public async Task ThenTheCapabilitiesAreReturnedOrderedByIsFoundationThenCapabilityName(Table table)
        {
            var expectedCapabilities = table.CreateSet<CapabilityTable>().Select(t => new
            {
                t.CapabilityRef,
                t.Version,
                Name = t.CapabilityName,
                t.IsFoundation,
            });

            var capabilities = (await response.ReadBody())
                .SelectToken("capabilities")?
                .Select(t => new
                {
                    CapabilityRef = t.SelectToken("reference")?.ToString(),
                    Version = t.SelectToken("version")?.ToString(),
                    Name = t.SelectToken("name")?.ToString(),
                    IsFoundation = Convert.ToBoolean(t.SelectToken("isFoundation")?.ToString(), CultureInfo.InvariantCulture),
                });

            capabilities.Should().BeEquivalentTo(expectedCapabilities, options => options.WithStrictOrdering());
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class CapabilityTable
        {
            public string CapabilityRef { get; init; }

            public string Version { get; init; }

            public string CapabilityName { get; init; }

            public bool IsFoundation { get; init; }
        }
    }
}
