using System;
using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Solution
{
    [Binding]
    internal sealed class GetSolutionsByIdSteps
    {
        private const string ByIdSolutionsUrl = "http://localhost:8080/api/v1/Solutions/{0}/{1}";

        private readonly ScenarioContext _context;

        private readonly Response _response;

        public GetSolutionsByIdSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [When(@"a GET request is made for (dashboard|preview|public) with no solution id")]
        public async Task WhenAGETRequestIsMadeForSolutionSlnNoId(string view)
        {
            await WhenAGETRequestIsMadeForSolutionSln(view, " ").ConfigureAwait(false);
        }

        [When(@"a GET request is made for solution (dashboard|preview|public) (.*)")]
        public async Task WhenAGETRequestIsMadeForSolutionSln(string view, string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(CultureInfo.InvariantCulture, ByIdSolutionsUrl, solutionId, view)).ConfigureAwait(false);
        }

        [Then(@"the solution IsFoundation is (true|false)")]
        public async Task ThenTheSolutionIsFoundationIsBool(bool response)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("isFoundation").Value<bool>().Should().Be(response);
        }

        [Then(@"the solution Name is (.*)")]
        public async Task ThenTheSolutionNameIs(string name)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("name").Value<string>().Should().Be(name);
        }

        [Then(@"the solution organisationName is (.*)")]
        public async Task ThenTheSolutionOrganisationNameIs(string name)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("organisationName").Value<string>().Should().Be(name);
        }

        [Then(@"the last updated date in the solution is (.*)")]
        public async Task ThenTheLastUpdatedDateInTheSolutionIs(DateTime lastUpdated)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            var contentLastUpdated = (DateTime)content.SelectToken("lastUpdated");

            contentLastUpdated.Should().Be(lastUpdated);
        }

    }
}
