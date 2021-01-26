using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.SolutionDescription
{
    [Binding]
    internal sealed class GetSolutionDescriptionSteps
    {
        private readonly Response response;

        public GetSolutionDescriptionSteps(Response response)
        {
            this.response = response;
        }

        [Then(@"the solution (link|summary|description) is (.*)")]
        public async Task ThenTheSolutionSolution_DescriptionSectionLinkIsUrlSln(string element, string link)
        {
            var content = await response.ReadBody();
            content.SelectToken(element)?.ToString().Should().Be(link);
        }

        [Then(@"the solution does not contain (link|summary|description)")]
        public async Task ThenTheSolutionDoesNotContainLink(string element)
        {
            var content = await response.ReadBody();
            content.SelectToken(element).Should().BeNull();
        }
    }
}
