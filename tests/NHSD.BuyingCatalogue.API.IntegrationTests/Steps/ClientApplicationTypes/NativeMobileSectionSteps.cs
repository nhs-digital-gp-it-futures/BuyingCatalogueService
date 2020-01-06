using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.ClientApplicationTypes
{
    [Binding]
    internal sealed class NativeMobileSectionSteps
    {
        private readonly Response _response;

        private const string Token = "sections.client-application-types.sections.native-mobile.sections";
        private const string MobileFirstDesign = "native-mobile-first.answers.mobile-first-design";

        public NativeMobileSectionSteps(Response response)
        {
            _response = response;
        }

        [Then(@"the solution native-mobile native-mobile-connection-details section contains connection-types")]
        public async Task ThenTheSolutionNativeMobileSectionContains(Table table)
        {
            var content = table.CreateInstance<ConnectionTypeTable>();
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken($"{Token}.native-mobile-connection-details.answers.connection-types")
                .Select(s => s.ToString())
                .Should().BeEquivalentTo(content.ConnectionTypes);
        }

        [Then(@"the solution native-mobile native-mobile-connection-details section contains (.*) with value (.*)")]
        public async Task ThenTheSolutionNativeMobileSectionContainsWithValue(string token, string value)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken($"{Token}.native-mobile-connection-details.answers.{token}")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution native-mobile native-mobile-hardware-requirements section contains (.*) with value (.*)")]
        public async Task ThenTheSolutionNativeMobileHardwareRequirementsSectionContainsWithValue(string token, string value)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken($"{Token}.native-mobile-hardware-requirements.answers.{token}")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution native-mobile native-mobile-third-party section contains (.*) with value (.*)")]
        public async Task ThenTheSolutionNativeMobileThirdPartySectionContainsWithValue(string token, string value)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken($"{Token}.native-mobile-third-party.answers.{token}")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution client-application-types section contains operating-systems")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsOperatingSystems(Table table)
        {
            var content = table.CreateInstance<OperatingSystemsTable>();
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken($"{Token}.native-mobile-operating-systems.answers.operating-systems")
                .Select(s => s.ToString())
                .Should().BeEquivalentTo(content.OperatingSystems);
        }

        [Then(@"the solution client-application-types section contains operating-systems-description with value (.*)")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsOperating_Systems_DescriptionWithValue(string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"{Token}.native-mobile-operating-systems.answers.operating-systems-description")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution native-mobile native-mobile-first section contains mobile-first-design with value (Yes|No)")]
        public async Task ThenTheSolutionNativeMobileSectionContainsMobileFirstDesign(string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"{Token}.{MobileFirstDesign}").ToString().Should().Be(value);
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
            content.SelectToken($"{Token}.native-mobile-additional-information.answers.additional-information").Should().BeNull();
        }

        [Then(@"the solution client-application-types section contains (minimum-memory-requirement|storage-requirements-description) with value (.*)")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsMobileMemoryWithValue(string section, string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"{Token}.native-mobile-memory-and-storage.answers.{section}")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution client-application-types section contains native-mobile-additional-information with value (.*)")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsAdditional_InformationWithValueAdditionalInfo(string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"{Token}.native-mobile-additional-information.answers.additional-information")
                .ToString().Should().Be(value);
        }


        private class OperatingSystemsTable
        {
            public List<string> OperatingSystems { get; set; }
        }

        private class ConnectionTypeTable
        {
            public List<string> ConnectionTypes { get; set; }
        }
    }
}
