using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.ClientApplicationTypes
{
    [Binding]
    internal sealed class NativeMobileSectionSteps
    {
        private readonly Response _response;

        private const string Token = "sections.client-application-types.sections.native-mobile.sections";

        public NativeMobileSectionSteps(Response response)
        {
            _response = response;
        }

        [Then(@"the solution native-mobile mobile-connection-details section contains connection-type")]
        public async Task ThenTheSolutionNativeMobileSectionContains(Table table)
        {
            var content = table.CreateInstance<ConnectionTypeTable>();
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken($"{Token}.mobile-connection-details.answers.connection-type")
                .Select(s => s.ToString())
                .Should().BeEquivalentTo(content.ConnectionTypes);
        }

        [Then(@"the solution native-mobile mobile-connection-details section contains (.*) with value (.*)")]
        public async Task ThenTheSolutionNativeMobileSectionContainsWithValue(string token, string value)
        {
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken($"{Token}.mobile-connection-details.answers.{token}")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution client-application-types section contains operating-systems")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsOperatingSystems(Table table)
        {
            var content = table.CreateInstance<OperatingSystemsTable>();
            var context = await _response.ReadBody().ConfigureAwait(false);
            context.SelectToken($"{Token}.mobile-operating-systems.answers.operating-systems")
                .Select(s => s.ToString())
                .Should().BeEquivalentTo(content.OperatingSystems);
        }

        [Then(@"the solution client-application-types section contains operating-systems-description with value (.*)")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsOperating_Systems_DescriptionWithValue(string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"{Token}.mobile-operating-systems.answers.operating-systems-description")
                .ToString().Should().Be(value);
        }

        [Then(@"the solution client-application-types section contains (minimum-memory-requirement|storage-requirements-description) with value (.*)")]
        public async Task ThenTheSolutionClient_Application_TypesSectionContainsMobileMemoryWithValue(string section, string value)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"{Token}.mobile-memory-and-storage.answers.{section}")
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
