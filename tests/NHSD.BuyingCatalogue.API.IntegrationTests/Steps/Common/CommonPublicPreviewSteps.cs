using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common
{
    [Binding]
    internal sealed class CommonPublicPreviewSteps
    {
        private readonly Response _response;

        public CommonPublicPreviewSteps(Response response)
        {
            _response = response;
        }

        [Then(@"the solutions (.*) section contains (.*) with value (.*)")]
        public async Task ThenTheSolutionsSectionContainsWithValue(string section, string contains, string value)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken($"sections.{section}.{contains}")
                .ToString().Should().Be(value);
        }

        [Then(@"the solutions (.*) section is returned")]
        public async Task ThenTheSolutionSectionIsReturnedAsync(string section)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.{section}").Should().NotBeNullOrEmpty();
        }
    }
}
