using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class GetSupportedBrowserSteps
    {
        private readonly Response _response;

        public GetSupportedBrowserSteps(Response response)
        {
            _response = response;
        }

        [Then(@"the mobile-responsive element is (Yes|No)")]
        public async Task ThenTheMobile_ResponsiveElementContains(string mobileResponsive)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken("mobile-responsive").Value<string>()
                .Should().Be(mobileResponsive);
        }

        [Then(@"the mobile-responsive element is null")]
        public async Task ThenTheMobile_ResponsiveElementIsNull()
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken("mobile-responsive")
                .Should().BeNull();
        }
    }
}
