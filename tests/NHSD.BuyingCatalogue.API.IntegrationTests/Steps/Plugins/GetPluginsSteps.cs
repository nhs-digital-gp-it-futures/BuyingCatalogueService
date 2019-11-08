using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class GetPluginsSteps
    {
        private const string PluginsUrl = "http://localhost:8080/api/v1/solutions/{0}/sections/plug-ins-or-extensions";

        private readonly Response _response;

        public GetPluginsSteps(Response response)
        {
            _response = response;
        }

        [Then(@"the required string is (yes|no)")]
        public async Task ThenTheRequiredElementContains(string required)
        {
            var content = await _response.ReadBody();
            content.SelectToken("plugins-required").Value<string>().Should().Be(required);
        }

        [Then(@"the required string is null")]
        public async Task ThenTheRequiredElementIsNull()
        {
            var content = await _response.ReadBody();
            content.SelectToken("plugins-required").Should().BeNull();
        }

        [Then(@"the addition-information string is (.*)")]
        public async Task ThenTheAdditionInformationElementIs(string additional_information)
        {
            var content = await _response.ReadBody();
            content.SelectToken("plugins-detail").Value<string>().Should().Be(additional_information);
        }

        [Then(@"the addition-information string value null")]
        public async Task ThenTheAdditionInformationElementIsNull()
        {
            var content = await _response.ReadBody();
            content.SelectToken("plugins-detail").Should().BeNull();
        }
    }
}
