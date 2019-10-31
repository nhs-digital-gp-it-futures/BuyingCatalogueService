using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class ClientApplicationTypeSectionSteps
    {
        private readonly ScenarioContext _context;

        private readonly Response _response;

        public ClientApplicationTypeSectionSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [Then(@"the client-application-types section contains (.*) subsections")]
        public async Task ThenTheClientApplicationTypesSectionContainsSubsections(int subsectionCount)
        {
            var content = await _response.ReadBody();
            content.SelectToken($"sections.client-application-types").Children()
                .Should().HaveCount(subsectionCount);
        }

        [Then(@"the client-application-types section is missing")]
        public async Task ThenTheClientApplicationTypesSectionIsMissing()
        {
            var content = await _response.ReadBody();
            content.SelectToken($"sections.client-application-types").Should().BeNull();
        }

        [Then(@"the client-application-types section contains subsection (\S+)")]
        public async Task ThenTheClientApplicationTypesSectionContainsSubsectionBrowserBased(string subsection)
        {
            var content = await _response.ReadBody();
            content.SelectToken($"sections.client-application-types.sections.{subsection}")
                .Should().NotBeNull();
        }

        private class ClientApplicationTypesTable
        {
            public string ClientApplication { get; set; }
        }
    }
}
