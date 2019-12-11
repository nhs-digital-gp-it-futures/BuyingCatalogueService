using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class CommonSectionsSteps
    {
        private const string RootSectionsUrl = "http://localhost:8080/api/v1/Solutions/{0}/sections/{1}";

        private readonly ScenarioContext _context;

        private readonly Response _response;

        public CommonSectionsSteps(ScenarioContext context, Response response)
        {
            _context = context;
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

        [When(@"a GET request is made for (client-application-types|features|solution-description|browsers-supported|plug-ins-or-extensions|contact-details|browser-hardware-requirements|connectivity-and-resolution) with no solution id")]
        public async Task GetRequestSectionNoSolutionId(string section)
        {
            await GetSectionRequest(section, " ").ConfigureAwait(false);
        }

        [When(@"a GET request is made for (client-application-types|features|solution-description|browsers-supported|plug-ins-or-extensions|browser-hardware-requirements|connectivity-and-resolution) for solution (.*)")]
        public async Task GetSectionRequest(string section, string solutionId)
        {
            _response.Result = await Client.GetAsync(string.Format(CultureInfo.InvariantCulture, RootSectionsUrl, solutionId, section)).ConfigureAwait(false);
        }

        private class MandatoryTable
        {
            public string Mandatory { get; set; }
        }
    }
}
