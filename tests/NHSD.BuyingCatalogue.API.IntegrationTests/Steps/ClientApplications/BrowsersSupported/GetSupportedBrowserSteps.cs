using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.ClientApplications.BrowsersSupported
{
    [Binding]
    internal sealed class GetSupportedBrowserSteps
    {
        private readonly Response response;

        public GetSupportedBrowserSteps(Response response)
        {
            this.response = response;
        }

        [Then(@"the mobile-responsive element is (Yes|No)")]
        public async Task ThenTheMobile_ResponsiveElementContains(string mobileResponsive)
        {
            var context = await response.ReadBody();
            context.SelectToken("mobile-responsive")?.Value<string>().Should().Be(mobileResponsive);
        }

        [Then(@"the mobile-responsive element is null")]
        public async Task ThenTheMobile_ResponsiveElementIsNull()
        {
            var context = await response.ReadBody();
            context.SelectToken("mobile-responsive").Should().BeNull();
        }
    }
}
