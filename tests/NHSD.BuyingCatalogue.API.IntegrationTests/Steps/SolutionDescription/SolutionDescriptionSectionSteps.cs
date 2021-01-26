using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.SolutionDescription
{
    [Binding]
    internal sealed class SolutionDescriptionSectionSteps
    {
        private readonly Response response;

        public SolutionDescriptionSectionSteps(Response response)
        {
            this.response = response;
        }

        [Then(@"the solution solution-description section does not contain (link|summary|description)")]
        public async Task ThenTheSolutionDoesNotContainLink(string field)
        {
            var content = await response.ReadBody();
            content.SelectToken($"sections.solution-description.answers.{field}").Should().BeNull();
        }
    }
}
