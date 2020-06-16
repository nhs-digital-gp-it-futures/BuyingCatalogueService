using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.SubmitForReview
{
    [Binding]
    internal sealed class SubmitForReviewSteps
    {
        private const string SubmitForReviewSolutionsUrl = "http://localhost:5200/api/v1/Solutions/{0}/SubmitForReview";

        private readonly ScenarioContext _context;

        private readonly Response _response;

        public SubmitForReviewSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [When(@"a request is made to submit Solution (.*) for review")]
        public async Task WhenARequestIsMadeToSubmitSlnForReview(string solutionId)
        {
            _response.Result = await Client.PutAsync(string.Format(CultureInfo.InvariantCulture, SubmitForReviewSolutionsUrl, solutionId));
        }

        [When(@"a request is made to submit Solution for review with no solution id")]
        public async Task WhenARequestIsMadeToSubmitForReviewWithNoSolutionId()
        {
            _response.Result = await Client.PutAsync(string.Format(CultureInfo.InvariantCulture, SubmitForReviewSolutionsUrl, " "));
        }

        [Then(@"the response details of the submit Solution for review request are as follows")]
        public async Task ThenTheDetailsOfTheSolutionsReturnedAreAsFollows(Table table)
        {
            var response = await _response.ReadBody();

            var expectedSectionErrorTable = table.CreateSet<SubmitSolutionForReviewResponseTable>();
            foreach (var expectedSectionError in expectedSectionErrorTable)
            {
                var token = response.SelectToken(expectedSectionError.Property);
                token.Should().NotBeNull();
                token.Select(s => s.ToString()).Should().BeEquivalentTo(expectedSectionError.InvalidSections);
            }
        }

        private class SubmitSolutionForReviewResponseTable
        {
            public string Property { get; set; }

            public List<string> InvalidSections { get; set; }
        }
    }
}
