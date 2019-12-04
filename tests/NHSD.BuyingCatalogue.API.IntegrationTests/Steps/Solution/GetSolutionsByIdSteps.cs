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
            await WhenAGETRequestIsMadeForSolutionSln(view, " ");
        }

        [When(@"a GET request is made for solution (dashboard|preview|public) (.*)")]
        public async Task WhenAGETRequestIsMadeForSolutionSln(string view, string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(ByIdSolutionsUrl, solutionId, view));
        }

        [Then(@"the solution IsFoundation is (true|false)")]
        public async Task ThenTheSolutionIsFoundationIsBool(bool response)
        {
            var content = await _response.ReadBody();
            content.SelectToken("isFoundation").Value<bool>().Should().Be(response);
        }

        [Then(@"the solution organisationName is (.*)")]
        public async Task ThenTheSolutionOrganisationNameIs(string name)
        {
            var content = await _response.ReadBody();
            content.SelectToken("organisationName").Value<string>().Should().Be(name);
        }

        [Then(@"the solution lastUpdated is (.*)")]
        public async Task LastUpdatedGet(string date)
        {
            var content = await _response.ReadBody();
            content.SelectToken("lastUpdated").Value<string>().Should().Be(date);
        }

        [Then(@"the last updated date in the solution is (.*)")]
        public async Task ThenTheLastUpdatedDateInTheSolutionIs(string lastUpdated)
        {
            var dateTimeLastUpdated = DateTime.ParseExact(lastUpdated, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            var content = await _response.ReadBody();
            var contentLastUpdated = (DateTime)content.SelectToken("lastUpdated");
            contentLastUpdated.Should().Be(dateTimeLastUpdated);
        }

    }
}
