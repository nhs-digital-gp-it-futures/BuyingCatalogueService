using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.ClientApplications.ClientApplicationTypes
{
    [Binding]
    internal sealed class NativeMobileSectionSteps
    {
        private readonly Response _response;

        private const string Token = "sections.client-application-types.sections.native-mobile.sections";
        private const string MobileFirstDesign = "native-mobile-first.answers.mobile-first-design";
        private const string AdditionalInformationToken = "native-mobile-additional-information.answers.additional-information";

        public NativeMobileSectionSteps(Response response)
        {
            _response = response;
        }

        [Then(@"the solution native-mobile native-mobile-first section does not contain mobile-first-design")]
        public async Task ThenTheSolutionNativeMobileSectionDoesNotContainMobileFirstDesign()
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"{Token}.{MobileFirstDesign}").Should().BeNull();
        }

        [Then(@"the solution native-mobile native-mobile-additional-information section does not contain additional-information")]
        public async Task ThenTheSolutionNativeMobileSectionDoesNotContainAdditionalInformation()
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"{Token}.{AdditionalInformationToken}").Should().BeNull();
        }
    }
}
