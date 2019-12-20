using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common
{
    [Binding]
    internal sealed class EditSectionPutSteps
    {
        private readonly Response _response;

        private static readonly Dictionary<string, Type> PayloadTypes = new Dictionary<string, Type>
        {
            { "browsers-supported", typeof(SupportedBrowserPayload) },
            { "browser-additional-information", typeof(BrowserAdditionalInformationPayload) },
            { "browser-hardware-requirements", typeof(BrowserHardwareRequirementsPayload) },
            { "browser-mobile-first", typeof(BrowserMobileFirstPayload) },
            { "client-application-types", typeof(ClientApplicationTypesPayload) },
            { "plug-ins-or-extensions", typeof(PluginsPayload) },
            { "solution-description", typeof(SolutionDescriptionPayload) }, 
            { "connectivity-and-resolution", typeof(ConnectivityAndResolutionPayload) },
            { "mobile-operating-systems", typeof(MobileOperatingSystemsPayload) },
            { "mobile-connection-details", typeof(MobileConnectionDetailsPayload) },
            { "mobile-memory-and-storage", typeof(MemoryAndStoragePayload) }
        };

        public EditSectionPutSteps(Response response)
        {
            _response = response;
        }

        [When(@"a PUT request is made to update the (browsers-supported|browser-additional-information|browser-hardware-requirements|browser-mobile-first|client-application-types|plug-ins-or-extensions|solution-description|connectivity-and-resolution|mobile-operating-systems|mobile-connection-details|mobile-memory-and-storage) section for solution (.*)")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionSlnBrowsers_SupportedSection(string section, string solutionId, Table table)
        {
            if (!PayloadTypes.ContainsKey(section))
            {
                Assert.Fail($"There is no Payload registered for section '{section}'. Please visit the EditSectionPutSteps class, and add a Payload class to PayloadTypes.");
            }

            var obj = Activator.CreateInstance(PayloadTypes[section]);
            table.FillInstance(obj);
            _response.Result = await Client.PutAsJsonAsync($"http://localhost:8080/api/v1/solutions/{solutionId}/sections/{section}", obj)
                .ConfigureAwait(false);
        }

        [When(@"a PUT request is made to update the (browsers-supported|browser-additional-information|browser-hardware-requirements|browser-mobile-first|client-application-types|plug-ins-or-extensions|solution-description|connectivity-and-resolution|mobile-operating-systems|mobile-connection-details|mobile-memory-and-storage) section with no solution id")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionBrowsers_SupportedSectionWithNoSolutionId(string section, Table table)
        {
            await WhenAPUTRequestIsMadeToUpdateSolutionSlnBrowsers_SupportedSection(section, " ", table).ConfigureAwait(false);
        }

        private class SupportedBrowserPayload
        {
            [JsonProperty("supported-browsers")]
            public List<string> BrowsersSupported { get; set; }

            [JsonProperty("mobile-responsive")]
            public string MobileResponsive { get; set; }
        }

        private class BrowserAdditionalInformationPayload
        {
            [JsonProperty("additional-information")]
            public string AdditionalInformation { get; set; }
        }

        private class BrowserHardwareRequirementsPayload
        {
            [JsonProperty("hardware-requirements-description")]
            public string HardwareRequirements { get; set; }
        }

        private class ClientApplicationTypesPayload
        {
            [JsonProperty("client-application-types")]
            public List<string> ClientApplicationTypes { get; set; }
        }

        private class PluginsPayload
        {
            [JsonProperty("plugins-required")]
            public string PluginsRequired { get; set; }

            [JsonProperty("plugins-detail")]
            public string PluginsDetail { get; set; }
        }

        private class SolutionDescriptionPayload
        {
            [JsonProperty("summary")]
            public string Summary { get; set; }
            [JsonProperty("description")]
            public string Description { get; set; }
            [JsonProperty("link")]
            public string Link { get; set; }
        }

        private class ConnectivityAndResolutionPayload
        {
            [JsonProperty("minimum-connection-speed")]
            public string MinimumConnectionSpeed { get; set; }

            [JsonProperty("minimum-desktop-resolution")]
            public string MinimumDesktopResolution { get; set; }
        }

        private class BrowserMobileFirstPayload
        {
            [JsonProperty("mobile-first-design")]
            public string MobileFirstDesign { get; set; }
        }

        private class MobileConnectionDetailsPayload
        {
            [JsonProperty("minimum-connection-speed")]
            public string MinimumConnectionSpeed { get; set; }

            [JsonProperty("connection-requirements-description")]
            public string ConnectionRequirementsDescription { get; set; }

            [JsonProperty("connection-type")]
            public List<string> ConnectionType { get; set; }
        }

        private class MobileOperatingSystemsPayload
        {
            [JsonProperty("operating-systems")]
            public List<string> OperatingSystems { get; set; }

            [JsonProperty("operating-systems-description")]
            public string OperatingSystemsDescription { get; set; }
        }

        private class MemoryAndStoragePayload
        {
            [JsonProperty("minimum-memory-requirement")]
            public string MinimumMemoryRequirement { get; set; }

            [JsonProperty("storage-requirement-description")]
            public string StorageRequirementsDescription { get; set; }
        }
    }
}
