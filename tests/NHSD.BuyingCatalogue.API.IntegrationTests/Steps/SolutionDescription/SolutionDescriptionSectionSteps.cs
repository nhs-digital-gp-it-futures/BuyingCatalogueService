using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class SolutionDescriptionSectionSteps
    {
        private readonly Response _response;

        public SolutionDescriptionSectionSteps(Response response)
        {
            _response = response;
        }

        [Then(@"the solution solution-description section contains (link|summary|description) of (.*)")]
        public async Task ThenTheSolutionContains(string field, string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.solution-description.answers.{field}").ToString().Should().Be(value);
        }

        [Then(@"the solution solution-description section does not contain (link|summary|description)")]
        public async Task ThenTheSolutionDoesNotContainLink(string field)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.solution-description.answers.{field}").Should().BeNull();
        }
    }
}
