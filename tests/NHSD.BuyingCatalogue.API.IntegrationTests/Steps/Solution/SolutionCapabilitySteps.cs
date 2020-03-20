using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using TechTalk.SpecFlow;

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

        [Then(@"the solution capabilities section contains no Capabilities")]
        public async Task ThenTheSolutionContainsNoCapabilities()
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.capabilities.answers.capabilities-met")
                .Should().BeNullOrEmpty();
        }
    }
}
