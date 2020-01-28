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


        [Then(@"the solutions ([\w-]*) section contains answer ([\w-]*) with value (.*)")]
        public async Task ThenTheSolutionContains(string section, string field, string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.{section}.answers.{field}").ToString().Should().Be(value);
        }

        [Then(@"the solutions ([\w-]*) section does not contain answer ([\w-]*)")]
        public async Task ThenTheSolutionDoesNotContainLink(string section, string field)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.{section}.answers.{field}").Should().BeNull();
        }

        [Then(@"the solutions ([\.\w-]*) section contains ([\w-]*) with value (.*)")]
        public async Task ThenTheSolutionsSectionContainsWithValue(string section, string contains, string value)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken($"sections.{section}.{contains}")
                .ToString().Should().Be(value);
        }

        [Then(@"the solutions ([\w-]*) section is returned")]
        public async Task ThenTheSolutionSectionIsReturnedAsync(string section)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.{section}").Should().NotBeNullOrEmpty();
        }

        [Then(@"the solutions ([\w-]*) section is not returned")]
        public async Task ThenTheSolutionSectionIsNotReturnedAsync(string section)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.{section}").Should().BeNull();
        }
    }
}
