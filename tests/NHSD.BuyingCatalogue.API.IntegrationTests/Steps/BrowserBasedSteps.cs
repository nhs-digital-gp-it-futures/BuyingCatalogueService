using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;


namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    internal sealed class BrowserBasedSteps
    {
        private const string BrowserBasedUrl = "http://localhost:8080/api/v1/solutions/{0}/sections/browser-based";

        private readonly ScenarioContext _context;

        private readonly Response _response;

        public BrowserBasedSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [When(@"a GET request is made to display solution (.*) browser-based sections")]
        public async Task WhenGETRequestIsMadeToDisplaySolutionBrowserBasedSections(string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(BrowserBasedUrl, solutionId));
        }

        [When(@"a GET request is made to display solution browser-based sections with no solution id")]
        public async Task WhenGETRequestIsMadeToDisplaySolutionBrowserBasedSectionsWithNoSolutionId()
        {
            await WhenGETRequestIsMadeToDisplaySolutionBrowserBasedSections(@" ");
        }


        [Then(@"Solutions browser-based section contains all BrowserBased Sections")]
        public async Task SolutionBrowserBasedSectionContainsAllBrowserBasedSections(Table table)
        {
            var content = await _response.ReadBody();

            var featureDataSet = table.CreateSet<BrowserBasedSection>();

            content.SelectToken("sections").Select(s => s.SelectToken("id").ToString()).Should().BeEquivalentTo(featureDataSet.Select(section => section.Id));
            content.SelectToken("sections").Select(s => s.SelectToken("status").ToString()).Should().BeEquivalentTo(featureDataSet.Select(section => section.Status));
            content.SelectToken("sections").Select(s => s.SelectToken("requirement").ToString()).Should().BeEquivalentTo(featureDataSet.Select(section => section.Requirement));

        }

        private class BrowserBasedSection
        {
            public string Id { get; set; }

            public string Status { get; set; }

            public string Requirement { get; set; }
        }
    }
}
