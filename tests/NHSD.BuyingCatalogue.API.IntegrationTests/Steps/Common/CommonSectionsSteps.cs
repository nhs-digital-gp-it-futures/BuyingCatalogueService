using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common
{
    [Binding]
    internal sealed class CommonSectionsSteps
    {
        private const string RootSectionsUrl = "http://localhost:5200/api/v1/Solutions/{0}/sections/{1}";
        private const string RootDashboardUrl = "http://localhost:5200/api/v1/Solutions/{0}/dashboards/{1}";

        private readonly Response response;

        public CommonSectionsSteps(Response response)
        {
            this.response = response;
        }

        [Then(@"the solution ([^\s]+) section status is (COMPLETE|INCOMPLETE)")]
        public async Task ThenTheSolutionSectionStatusIs(string section, string status)
        {
            var content = await response.ReadBody();
            content.Should().NotBeNull();
            content.ToString().Should().Contain($"\"{section}\"");
            content.SelectToken($"sections.{section}.status")?.ToString().Should().Be(status);
        }

        [Then(@"the solution ([^\s]+) section requirement is (Mandatory|Optional)")]
        public async Task ThenTheSolutionSectionRequirementIsMandatory(string section, string requirement)
        {
            var content = await response.ReadBody();
            content.Should().NotBeNull();
            content.ToString().Should().Contain($"\"{section}\"");
            content.SelectToken($"sections.{section}.requirement")?.ToString().Should().Be(requirement);
        }

        [Then(@"the solution ([^\s]+) section ([^\s]+) subsection status is (INCOMPLETE|COMPLETE)")]
        public async Task ThenTheSolutionSubSectionStatusIs(string section, string subsection, string status)
        {
            var content = await response.ReadBody();
            content.SelectToken($"sections.{section}.sections.{subsection}.status")?.ToString().Should().Be(status);
        }

        [Then(@"the solution ([^\s]+) section ([^\s]+) subsection requirement is (Mandatory|Optional)")]
        public async Task ThenTheSolutionSubSectionRequirementIs(string section, string subsection, string requirement)
        {
            var content = await response.ReadBody();
            content.SelectToken($"sections.{section}.sections.{subsection}.requirement")?.ToString().Should().Be(requirement);
        }

        [When(@"a GET request is made for ([^\s]+) section with no solution id")]
        public async Task GetRequestSectionNoSolutionId(string section)
        {
            await GetSectionRequest(section, " ");
        }

        [When(@"a GET request is made for ([^\s]+) section dashboard with no solution id")]
        public async Task GetRequestDashboardNoSolutionId(string section)
        {
            await GetDashboardRequest(section, " ");
        }

        [When(@"a GET request is made for ([^\s]+) section for solution (.*)")]
        public async Task GetSectionRequest(string section, string solutionId)
        {
            response.Result = await Client.GetAsync(
                string.Format(CultureInfo.InvariantCulture, RootSectionsUrl, solutionId, section));
        }

        [When(@"a GET request is made for ([^\s]+) section dashboard for solution (.*)")]
        public async Task GetDashboardRequest(string section, string solutionId)
        {
            response.Result = await Client.GetAsync(
                string.Format(CultureInfo.InvariantCulture, RootDashboardUrl, solutionId, section));
        }

        [Then(@"Solutions section contains all items")]
        public async Task SolutionSectionContainsAllItems(Table table)
        {
            var content = await response.ReadBody();

            foreach (var section in table.CreateSet<SectionItems>())
            {
                content.SelectToken($"sections.{section.Id}.requirement")?.ToString().Should().Be(section.Requirement);
                content.SelectToken($"sections.{section.Id}.status")?.ToString().Should().Be(section.Status);
            }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class SectionItems
        {
            public string Id { get; init; }

            public string Status { get; init; }

            public string Requirement { get; init; }
        }
    }
}
