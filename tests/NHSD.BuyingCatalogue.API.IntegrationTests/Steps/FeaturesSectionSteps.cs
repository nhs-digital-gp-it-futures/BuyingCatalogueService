using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class FeaturesSectionSteps
    {
        private const string ByIdSolutionsUrl = "http://localhost:8080/api/v1/Solutions/{0}";

        private const string FeaturesUrl = "http://localhost:8080/api/v1/Solutions/{0}/sections/features";

        private readonly ScenarioContext _context;

        private readonly Response _response;

        public FeaturesSectionSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [When(@"a GET request is made for solution (.*)")]
        public async Task WhenAGETRequestIsMadeForSolutionSln(string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(ByIdSolutionsUrl, solutionId));
        }

        [When(@"a PUT request is made to update solution (.*) features section")]
        public async Task GivenAPUTRequestIsMadeToUpdateSolutionSlnFeatures(string solutionId, Table table)
        {
            var content = table.CreateInstance<FeaturesPostTable>();

            _response.Result = await Client.PutAsJsonAsync(string.Format(FeaturesUrl, solutionId), new { Listing = content.Features });
        }

        [Then(@"the solution features section contains Features")]
        public async Task ThenTheSolutionContainsFeatures(Table table)
        {
            var content = await _response.ReadBody();
            content.SelectToken("solution.marketingData.sections[?(@.id == 'features')].data.listing")
                .Select(s => s.ToString()).Should().BeEquivalentTo(table.CreateSet<FeaturesTable>().Select(s => s.Feature));
        }

        [Then(@"the solution features section contains no features")]
        public async Task ThenTheSolutionContainsNoFeatures()
        {
            var content = await _response.ReadBody();
            content.SelectToken("solution.marketingData.sections[?(@.id == 'features')].data.listing")
                .Should().BeNullOrEmpty();
        }

        private class FeaturesPostTable
        {
            public List<string> Features { get; set; }
        }

        private class FeaturesTable
        {
            public string Feature { get; set; }
        }
    }
}
