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

        [Then(@"the solution (features|solution-description|client-application-types|contact-details|public-cloud|private-cloud|hybrid) section status is (COMPLETE|INCOMPLETE)")]
        public async Task ThenTheSolutionSectionStatusIs(string section, string status)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.{section}.status").ToString().Should().Be(status);
        }

        [Then(@"the solution (features|solution-description|client-application-types|contact-details|public-cloud|private-cloud|hybrid) section requirement is (Mandatory|Optional)")]
        public async Task ThenTheSolutionSectionRequirementIsMandatory(string section, string requirement)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.{section}.requirement").ToString().Should().Be(requirement);
        }

        [Then(@"the solution (browser-based|native-desktop|native-mobile) section requirement is (Mandatory|Optional)")]
        public async Task ThenTheSolutionSubSectionRequirementIsMandatory(string section, string requirement)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.client-application-types.sections.{section}.requirement").ToString().Should().Be(requirement);
        }

        [When(@"a GET request is made for (client-application-types|features|solution-description|browser-browsers-supported|browser-plug-ins-or-extensions|contact-details|browser-hardware-requirements|browser-connectivity-and-resolution|browser-additional-information|browser-mobile-first|native-mobile-operating-systems|native-mobile-connection-details|native-mobile-memory-and-storage|native-mobile-first|native-mobile-third-party|native-mobile-hardware-requirements|native-mobile-additional-information|native-desktop-hardware-requirements|native-desktop-connection-details|native-desktop-operating-systems|native-desktop-third-party|native-desktop-memory-and-storage|native-desktop-additional-information) with no solution id")]
        public async Task GetRequestSectionNoSolutionId(string section)
        {
            await GetSectionRequest(section, " ").ConfigureAwait(false);
        }

        [When(@"a GET request is made for (browser-based|native-mobile|native-desktop) dashboard with no solution id")]
        public async Task GetRequestDashboardNoSolutionId(string section)
        {
            await GetDashboardRequest(section, " ").ConfigureAwait(false);
        }

        [When(@"a GET request is made for (client-application-types|features|solution-description|browser-browsers-supported|browser-plug-ins-or-extensions|contact-details|browser-hardware-requirements|browser-connectivity-and-resolution|browser-additional-information|browser-mobile-first|native-mobile-operating-systems|native-mobile-connection-details|native-mobile-memory-and-storage|native-mobile-first|native-mobile-third-party|native-mobile-hardware-requirements|native-mobile-additional-information|native-desktop-hardware-requirements|native-desktop-connection-details|native-desktop-operating-systems|native-desktop-third-party|native-desktop-memory-and-storage|native-desktop-additional-information) for solution (.*)")]
        public async Task GetSectionRequest(string section, string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(CultureInfo.InvariantCulture, RootSectionsUrl, solutionId, section)).ConfigureAwait(false);
        }

        [When(@"a GET request is made for (browser-based|native-mobile|native-desktop) dashboard for solution (.*)")]
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

        [Then(@"the status of the (browser-browsers-supported|browser-plug-ins-or-extensions|browser-hardware-requirements|browser-connectivity-and-resolution|browser-additional-information|browser-mobile-first|native-mobile-first|native-mobile-operating-systems|native-mobile-connection-details|native-mobile-memory-and-storage|native-mobile-hardware-requirements|native-mobile-third-party|native-mobile-additional-information|native-desktop-operating-systems|native-desktop-hardware-requirements|native-desktop-connection-details|native-desktop-third-party|native-desktop-memory-and-storage|native-desktop-additional-information) section is (COMPLETE|INCOMPLETE)")]
        public async Task StatusOfSectionIs(string section, string status)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken("sections." + section + ".status").ToString().Should().BeEquivalentTo(status);
        }

        private class SectionItems
        {
            public string Id { get; set; }

            public string Status { get; set; }

            public string Requirement { get; set; }
        }
    }
}
