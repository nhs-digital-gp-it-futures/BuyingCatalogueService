using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Solution
{
    [Binding]
    internal sealed class SolutionCapabilitySteps
    {
        private readonly Response response;

        public SolutionCapabilitySteps(Response response)
        {
            this.response = response;
        }

        [Then(@"the solution capabilities section contains no Capabilities")]
        public async Task ThenTheSolutionContainsNoCapabilities()
        {
            var content = await response.ReadBody();
            content.SelectToken("sections.capabilities.answers.capabilities-met").Should().BeNullOrEmpty();
        }
    }
}
