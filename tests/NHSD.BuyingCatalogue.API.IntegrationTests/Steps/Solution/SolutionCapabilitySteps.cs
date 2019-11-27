using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Solution
{
    [Binding]
    internal sealed class SolutionCapabilitySteps
    {
        private readonly Response _response;

        public SolutionCapabilitySteps(Response response)
        {
            _response = response;
        }

        [Then(@"the solution capabilities section contains Capabilities")]
        public async Task ThenTheSolutionCapabilitiesSectionContainsCapabilities(Table table)
        {
            var content = await _response.ReadBody();

            content.SelectToken("sections.capabilities.answers.capabilities-met")
                .Select(s => s.ToString()).Should().BeEquivalentTo(table.CreateSet<CapabilitiesTable>().Select(s => s.Capability));
        }

        [Then(@"the solution capabilities section contains no Capabilities")]
        public async Task ThenTheSolutionContainsNoCapabilities()
        {
            var content = await _response.ReadBody();
            content.SelectToken("sections.capabilities.answers.capabilities-met")
                .Should().BeNullOrEmpty();
        }

        private class CapabilitiesTable
        {
            public string Capability { get; set; }
        }
    }
}
