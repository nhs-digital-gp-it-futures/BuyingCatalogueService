using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common
{
    [Binding]
    internal sealed class CommonSectionsSteps
    {
        private const string RootSectionsUrl = "http://localhost:8080/api/v1/Solutions/{0}/sections/{1}";
        private const string RootDashboardUrl = "http://localhost:8080/api/v1/Solutions/{0}/dashboards/{1}";

        private readonly Response _response;

        public CommonSectionsSteps(Response response)
        {
            _response = response;
        }

        [Then(@"the solution ([^\s]+) section status is (COMPLETE|INCOMPLETE)")]
        public async Task ThenTheSolutionSectionStatusIs(string section, string status)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.{section}.status").ToString().Should().Be(status);
        }

        [Then(@"the solution ([^\s]+) section requirement is (Mandatory|Optional)")]
        public async Task ThenTheSolutionSectionRequirementIsMandatory(string section, string requirement)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.{section}.requirement").ToString().Should().Be(requirement);
        }

        [Then(@"the solution ([^\s]+) section ([^\s]+) subsection status is (INCOMPLETE|COMPLETE)")]
        public async Task ThenTheSolutionSubSectionStatusIs(string section, string subsection, string status)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.{section}.sections.{subsection}.status").ToString().Should().Be(status);
        }

        [Then(@"the solution ([^\s]+) section ([^\s]+) subsection requirement is (Mandatory|Optional)")]
        public async Task ThenTheSolutionSubSectionRequirementIs(string section, string subsection, string requirement)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.{section}.sections.{subsection}.requirement").ToString().Should().Be(requirement);
        }

        [When(@"a GET request is made for ([^\s]+) section with no solution id")]
        public async Task GetRequestSectionNoSolutionId(string section)
        {
            await GetSectionRequest(section, " ").ConfigureAwait(false);
        }

        [When(@"a GET request is made for ([^\s]+) section dashboard with no solution id")]
        public async Task GetRequestDashboardNoSolutionId(string section)
        {
            await GetDashboardRequest(section, " ").ConfigureAwait(false);
        }

        [When(@"a GET request is made for ([^\s]+) section for solution (.*)")]
        public async Task GetSectionRequest(string section, string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(CultureInfo.InvariantCulture, RootSectionsUrl, solutionId, section)).ConfigureAwait(false);
        }

        [When(@"a GET request is made for ([^\s]+) section dashboard for solution (.*)")]
        public async Task GetDashboardRequest(string section, string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(CultureInfo.InvariantCulture, RootDashboardUrl, solutionId, section)).ConfigureAwait(false);
        }

        [Then(@"Solutions section contains all items")]
        public async Task SolutionSectionContainsAllItems(Table table)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);

            foreach (var section in table.CreateSet<SectionItems>())
            {
                content.SelectToken($"sections.{section.Id}.requirement").ToString().Should().Be(section.Requirement);
                content.SelectToken($"sections.{section.Id}.status").ToString().Should().Be(section.Status);
            }
        }

        private class SectionItems
        {
            public string Id { get; set; }

            public string Status { get; set; }

            public string Requirement { get; set; }
        }
    }
}
