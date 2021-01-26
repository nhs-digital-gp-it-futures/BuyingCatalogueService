using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Features
{
    [Binding]
    internal sealed class FeaturesSectionSteps
    {
        private const string FeaturesUrl = "http://localhost:5200/api/v1/Solutions/{0}/sections/features";

        private readonly Response response;

        public FeaturesSectionSteps(Response response)
        {
            this.response = response;
        }

        [When(@"a PUT request is made to update solution features section with no solution id")]
        public async Task WhenARequestIsMadeToSubmitForReviewWithNoSolutionId(Table table)
        {
            await WhenAPutRequestIsMadeToUpdateSolutionSlnFeatures(" ", table);
        }

        [When(@"a PUT request is made to update solution (.*) features section")]
        public async Task WhenAPutRequestIsMadeToUpdateSolutionSlnFeatures(string solutionId, Table table)
        {
            var content = table.CreateInstance<FeaturesPostTable>();

            response.Result = await Client.PutAsJsonAsync(
                string.Format(CultureInfo.InvariantCulture, FeaturesUrl, solutionId),
                new { Listing = content.Features });
        }

        [Then(@"the solution features section contains no features")]
        public async Task ThenTheSolutionContainsNoFeatures()
        {
            var content = await response.ReadBody();
            content.SelectToken("sections.features.answers.listing").Should().BeNullOrEmpty();
        }

        private sealed class FeaturesPostTable
        {
            [UsedImplicitly]
            public List<string> Features { get; init; }
        }
    }
}
