using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class CommonSectionsSteps
    {
        private readonly ScenarioContext _context;

        private readonly Response _response;

        public CommonSectionsSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [Then(@"the solution (features|solution-description|client-application-types) section status is (COMPLETE|INCOMPLETE)")]
        public async Task ThenTheSolutionFeaturesSectionStatusIsCOMPLETE(string section, string status)
        {
            var content = await _response.ReadBody();
            content.SelectToken($"solution.marketingData.sections[?(@.id == '{section}')].status").ToString().Should().Be(status);
        }

        [Then(@"the solution (features|solution-description|client-application-types) section requirement is (Mandatory|Optional)")]
        public async Task ThenTheSolutionSectionRequirementIsMandatory(string section, string requirement)
        {
            var content = await _response.ReadBody();
            content.SelectToken($"solution.marketingData.sections[?(@.id == '{section}')].requirement").ToString().Should().Be(requirement);
        }

        [Then(@"the solution (features|solution-description) section Mandatory field list is")]
        public async Task ThenTheSolutionSolution_DescriptionSectionMandatoryFieldListIs(string section, Table table)
        {
            var content = await _response.ReadBody();
            content.SelectToken($"solution.marketingData.sections[?(@.id == '{section}')].mandatory")
                .Select(s => s.ToString()).Should().BeEquivalentTo(table.CreateSet<MandatoryTable>().Select(s => s.Mandatory));
        }

        [Then(@"the solution (features|solution-description) section Mandatory field list is empty")]
        public async Task ThenTheSolutionFeaturesSectionMandatoryFieldListIsEmpty(string section)
        {
            var content = await _response.ReadBody();
            content.SelectToken($"solution.marketingData.sections[?(@.id == '{section}')].mandatory")
                .Select(s => s.ToString()).Should().BeNullOrEmpty();
        }

        private class MandatoryTable
        {
            public string Mandatory { get; set; }
        }
    }
}
