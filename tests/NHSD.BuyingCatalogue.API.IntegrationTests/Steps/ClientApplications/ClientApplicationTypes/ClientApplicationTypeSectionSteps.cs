using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.ClientApplications.ClientApplicationTypes
{
    [Binding]
    internal sealed class ClientApplicationTypeSectionSteps
    {
        private readonly Response response;

        public ClientApplicationTypeSectionSteps(Response response)
        {
            this.response = response;
        }

        [Then(@"the client-application-types section contains (.*) subsections")]
        public async Task ThenTheClientApplicationTypesSectionContainsSubsections(int subsectionCount)
        {
            var content = await response.ReadBody();
            content.SelectToken("sections.client-application-types")?.Children()
                .Should().HaveCount(subsectionCount);
        }

        [Then(@"the client-application-types section is missing")]
        public async Task ThenTheClientApplicationTypesSectionIsMissing()
        {
            var content = await response.ReadBody();
            content.SelectToken("sections.client-application-types").Should().BeNull();
        }

        [Then(@"the client-application-types section contains subsection (\S+)")]
        public async Task ThenTheClientApplicationTypesSectionContainsSubsectionBrowserBased(string subsection)
        {
            var content = await response.ReadBody();
            content.SelectToken($"sections.client-application-types.sections.{subsection}")
                .Should().NotBeNull();
        }

        [Then(@"the client-application-types section is not returned")]
        public async Task ThenTheSolutionClientApplicationTypesSectionContains()
        {
            var content = await response.ReadBody();
            content.SelectToken("sections.client-application-types").Should().BeNullOrEmpty();
        }

        [Then(@"the solution client-application-types section is returned")]
        public async Task ThenTheSolutionClient_Application_TypesSectionIsReturnedAsync()
        {
            var content = await response.ReadBody();
            content.SelectToken("sections.client-application-types").Should().NotBeNullOrEmpty();
        }
    }
}
