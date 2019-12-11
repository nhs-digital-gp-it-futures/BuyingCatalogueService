using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;


namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
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
            _response.Result = await Client.GetAsync(string.Format(CultureInfo.InvariantCulture, BrowserBasedUrl, solutionId)).ConfigureAwait(false);
        }

        [When(@"a GET request is made to display solution browser-based sections with no solution id")]
        public async Task WhenGETRequestIsMadeToDisplaySolutionBrowserBasedSectionsWithNoSolutionId()
        {
            await WhenGETRequestIsMadeToDisplaySolutionBrowserBasedSections(@" ").ConfigureAwait(false);
        }

        [Then(@"Solutions browser-based section contains all BrowserBased Sections")]
        public async Task SolutionBrowserBasedSectionContainsAllBrowserBasedSections(Table table)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);

            foreach (var section in table.CreateSet<BrowserBasedSection>())
            {
                content.SelectToken($"sections.{section.Id}.requirement").ToString().Should().Be(section.Requirement);
                content.SelectToken($"sections.{section.Id}.status").ToString().Should().Be(section.Status);
            }
        }

        [Then(@"the status of the (browsers-supported|plug-ins-or-extensions|browser-hardware-requirements) section is (COMPLETE|INCOMPLETE)")]
        public async Task StatusOfPluginsSectionIs(string section, string status)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections." + section + ".status").ToString().Should().BeEquivalentTo(status);
        }

        private class BrowserBasedSection
        {
            public string Id { get; set; }

            public string Status { get; set; }

            public string Requirement { get; set; }
        }
    }
}
