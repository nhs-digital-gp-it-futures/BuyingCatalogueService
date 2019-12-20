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

        private readonly Response _response;

        public CommonSectionsSteps(Response response)
        {
            _response = response;
        }

        [Then(@"the solution (features|solution-description|client-application-types|contact-details) section status is (COMPLETE|INCOMPLETE)")]
        public async Task ThenTheSolutionSectionStatusIs(string section, string status)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.{section}.status").ToString().Should().Be(status);
        }

        [Then(@"the solution (features|solution-description|client-application-types|contact-details) section requirement is (Mandatory|Optional)")]
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

        [When(@"a GET request is made for (client-application-types|features|solution-description|browsers-supported|plug-ins-or-extensions|contact-details|browser-hardware-requirements|connectivity-and-resolution|browser-additional-information|browser-mobile-first|browser-based|mobile-operating-systems|native-mobile|mobile-connection-details|mobile-memory-and-storage|mobile-first) with no solution id")]
        public async Task GetRequestSectionNoSolutionId(string section)
        {
            await GetSectionRequest(section, " ").ConfigureAwait(false);
        }

        [When(@"a GET request is made for (client-application-types|features|solution-description|browsers-supported|plug-ins-or-extensions|contact-details|browser-hardware-requirements|connectivity-and-resolution|browser-additional-information|browser-mobile-first|browser-based|mobile-operating-systems|native-mobile|mobile-connection-details|mobile-memory-and-storage|mobile-first) for solution (.*)")]
        public async Task GetSectionRequest(string section, string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(CultureInfo.InvariantCulture, RootSectionsUrl, solutionId, section)).ConfigureAwait(false);
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

        [Then(@"the status of the (browsers-supported|plug-ins-or-extensions|browser-hardware-requirements|connectivity-and-resolution|browser-additional-information|browser-mobile-first|mobile-operating-systems|mobile-connection-details|mobile-memory-and-storage) section is (COMPLETE|INCOMPLETE)")]
        public async Task StatusOfPluginsSectionIs(string section, string status)
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
