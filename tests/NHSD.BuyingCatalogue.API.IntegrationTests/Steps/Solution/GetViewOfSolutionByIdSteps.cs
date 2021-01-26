using System;
using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Solution
{
    [Binding]
    internal sealed class GetViewOfSolutionByIdSteps
    {
        private const string ByIdSolutionsUrl = "http://localhost:5200/api/v1/Solutions/{0}/{1}";

        private readonly Response response;

        public GetViewOfSolutionByIdSteps(Response response)
        {
            this.response = response;
        }

        [When(@"a GET request is made for (dashboard|preview|public) with no solution id")]
        public async Task WhenAGetRequestIsMadeForSolutionSlnNoId(string view)
        {
            await WhenAGetRequestIsMadeForSolution(view, " ");
        }

        [When(@"a GET request is made for solution (dashboard|preview|public) (.*)")]
        public async Task WhenAGetRequestIsMadeForSolution(string view, string solutionId)
        {
            response.Result = await Client.GetAsync(
                string.Format(CultureInfo.InvariantCulture, ByIdSolutionsUrl, solutionId, view));
        }

        [When(@"a GET request is made for solution authority dashboard (.*)")]
        public async Task WhenAGetRequestIsMadeForSolutionSln(string solutionId)
        {
            await WhenAGetRequestIsMadeForSolution("dashboard/authority", solutionId);
        }

        [Then(@"the last updated date in the solution is (.*)")]
        public async Task ThenTheLastUpdatedDateInTheSolutionIs(DateTime lastUpdated)
        {
            var content = await response.ReadBody();
            var contentLastUpdated = (DateTime)content.SelectToken("lastUpdated");

            contentLastUpdated.Should().Be(lastUpdated);
        }
    }
}
