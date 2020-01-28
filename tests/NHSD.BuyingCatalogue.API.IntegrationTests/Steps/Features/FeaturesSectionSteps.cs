using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Features
{
    [Binding]
    internal sealed class FeaturesSectionSteps
    {
        private const string FeaturesUrl = "http://localhost:8080/api/v1/Solutions/{0}/sections/features";

        private readonly ScenarioContext _context;

        private readonly Response _response;

        public FeaturesSectionSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [When(@"a PUT request is made to update solution features section with no solution id")]
        public async Task WhenARequestIsMadeToSubmitForReviewWithNoSolutionId(Table table)
        {
            await WhenAPUTRequestIsMadeToUpdateSolutionSlnFeatures(" ", table).ConfigureAwait(false);
        }

        [When(@"a PUT request is made to update solution (.*) features section")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionSlnFeatures(string solutionId, Table table)
        {
            var content = table.CreateInstance<FeaturesPostTable>();

            _response.Result = await Client.PutAsJsonAsync(string.Format(CultureInfo.InvariantCulture, FeaturesUrl, solutionId), new { Listing = content.Features }).ConfigureAwait(false);
        }

        [Then(@"the solution features section contains no features")]
        public async Task ThenTheSolutionContainsNoFeatures()
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections.features.answers.listing")
                .Should().BeNullOrEmpty();
        }

        private class FeaturesPostTable
        {
            public List<string> Features { get; set; }
        }
    }
}
