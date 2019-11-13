using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class SolutionDescriptionSectionSteps
    {
        private const string SolutionDescriptionUrl = "http://localhost:8080/api/v1/Solutions/{0}/sections/solution-description";

        private readonly ScenarioContext _context;

        private readonly Response _response;

        public SolutionDescriptionSectionSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [When(@"a PUT request is made to update solution solution-description section with no solution id")]
        public async Task WhenARequestIsMadeToSubmitForReviewWithNoSolutionId(Table table)
        {
            await WhenAPUTRequestIsMadeToUpdateSolutionDescriptionSection(" ", table);
        }

        [When(@"a PUT request is made to update solution (.*) solution-description section")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionDescriptionSection(string solutionId, Table table)
        {
            var content = table.CreateInstance<SolutionDescriptionPostTable>();

            _response.Result = await Client.PutAsJsonAsync(string.Format(SolutionDescriptionUrl, solutionId), content);
        }

        [Then(@"the solution solution-description section contains (link|summary|description) of (.*)")]
        public async Task ThenTheSolutionContains(string field, string value)
        {
            var content = await _response.ReadBody();
            content.SelectToken($"sections.solution-description.answers.{field}").ToString().Should().Be(value);
        }

        [Then(@"the solution solution-description section does not contain (link|summary|description)")]
        public async Task ThenTheSolutionDoesNotContainLink(string field)
        {
            var content = await _response.ReadBody();
            content.SelectToken($"sections.solution-description.answers.{field}").Should().BeNull();
        }

        private class SolutionDescriptionPostTable
        {
            public string Summary { get; set; }

            public string Description { get; set; }

            public string Link { get; set; }
        }
    }
}
