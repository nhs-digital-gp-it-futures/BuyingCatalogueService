using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.SolutionDescription
{
    [Binding]
    internal sealed class GetSolutionDescriptionSteps
    {
        private readonly ScenarioContext _context;

        private readonly Response _response;

        public GetSolutionDescriptionSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [Then(@"the solution (link|summary|description) is (.*)")]
        public async Task ThenTheSolutionSolution_DescriptionSectionLinkIsUrlSln(string element, string link)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken(element).ToString().Should().Be(link);
        }

        [Then(@"the solution does not contain (link|summary|description)")]
        public async Task ThenTheSolutionDoesNotContainLink(string element)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken(element).Should().BeNull();
        }
    }
}
