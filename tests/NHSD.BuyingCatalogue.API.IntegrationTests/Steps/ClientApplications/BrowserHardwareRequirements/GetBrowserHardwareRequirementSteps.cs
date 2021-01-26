using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.ClientApplications.BrowserHardwareRequirements
{
    [Binding]
    internal sealed class GetBrowserHardwareRequirementSteps
    {
        private readonly Response response;

        public GetBrowserHardwareRequirementSteps(Response response)
        {
            this.response = response;
        }

        [Then(@"there are no browser-hardware-requirements")]
        public async Task ThenTheBrowserHardwareRequirementsValueIsNull()
        {
            var content = await response.ReadBody();
            content.ToString().Should().Be("{}");
        }
    }
}
