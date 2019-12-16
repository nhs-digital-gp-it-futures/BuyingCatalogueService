using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;


namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class BrowserBasedSteps
    {
        private readonly ScenarioContext _context;

        private readonly Response _response;

        public BrowserBasedSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
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

        [Then(@"the status of the (browsers-supported|plug-ins-or-extensions|browser-hardware-requirements|connectivity-and-resolution|browser-additional-information|browser-mobile-first) section is (COMPLETE|INCOMPLETE)")]
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
