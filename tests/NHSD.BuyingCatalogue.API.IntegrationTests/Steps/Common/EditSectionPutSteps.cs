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

        private static readonly Dictionary<string, Type> PayloadTypes = new()
        {
            { "client-application-types", typeof(ClientApplicationTypesPayload) },
            { "solution-description", typeof(SolutionDescriptionPayload) },
            { "browser-browsers-supported", typeof(SupportedBrowserPayload) },
            { "browser-additional-information", typeof(BrowserAdditionalInformationPayload) },
            { "browser-hardware-requirements", typeof(BrowserHardwareRequirementsPayload) },
            { "native-mobile-hardware-requirements", typeof(NativeMobileHardwareRequirementsPayload) },
            { "native-desktop-hardware-requirements", typeof(NativeDesktopHardwareRequirementsPayload) },
            { "browser-mobile-first", typeof(BrowserMobileFirstPayload) },
            { "browser-plug-ins-or-extensions", typeof(PluginsPayload) },
            { "browser-connectivity-and-resolution", typeof(ConnectivityAndResolutionPayload) },
            { "native-mobile-operating-systems", typeof(MobileOperatingSystemsPayload) },
            { "native-mobile-connection-details", typeof(MobileConnectionDetailsPayload) },
            { "native-mobile-first", typeof(NativeMobileFirstPayload) },
			{ "native-mobile-memory-and-storage", typeof(MemoryAndStoragePayload) },
			{ "native-mobile-third-party", typeof(MobileThirdPartyPayload) },
			{ "native-mobile-additional-information", typeof(NativeMobileAdditionalInformationPayload) },
			{ "native-desktop-connection-details", typeof(NativeDesktopConnectivityDetails) },
            { "native-desktop-operating-systems", typeof(NativeDesktopOperatingSystemsPayload) },
            { "native-desktop-memory-and-storage", typeof(NativeDesktopMemoryAndStoragePayload) },
            { "native-desktop-third-party", typeof(NativeDesktopThirdParty) },
            { "native-desktop-additional-information", typeof(NativeDesktopAdditionalInformationPayload) },
            { "hosting-type-public-cloud", typeof(PublicCloudPayload) },
            { "hosting-type-private-cloud", typeof(HostingPrivateCloudPayload) },
            { "hosting-type-on-premise", typeof(HostingOnPremisePayload) },
            { "hosting-type-hybrid", typeof(HostingHybridHostingTypePayload) },
            { "roadmap", typeof(RoadmapPayload) },
            { "about-supplier", typeof(SupplierPayload) },
            { "integrations", typeof(IntegrationsPayload) },
            { "implementation-timescales", typeof(ImplementationTimescalesPayload )},
            { "capabilities", typeof(CapabilitiesPayload) }
        };

        public EditSectionPutSteps(Response response)
        {
            _response = response;
        }

        [When(@"a PUT request is made to update the (.*) section for solution (.*)")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionSlnBrowsers_SupportedSection(string section, string solutionId, Table table)
        {
            if (!PayloadTypes.ContainsKey(section))
            {
                Assert.Fail($"There is no Payload registered for section '{section}'. Please visit the EditSectionPutSteps class, and add a Payload class to PayloadTypes.");
            }

            var obj = Activator.CreateInstance(PayloadTypes[section]);
            table.FillInstance(obj);
            _response.Result = await Client.PutAsJsonAsync($"http://localhost:5200/api/v1/solutions/{solutionId}/sections/{section}", obj)
                .ConfigureAwait(false);
        }

        [When(@"a PUT request is made to update the (.*) section with no solution id")]
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

        private class NativeMobileHardwareRequirementsPayload
        {
            [JsonProperty("hardware-requirements")]
            public string HardwareRequirements { get; set; }
        }

        private class NativeDesktopHardwareRequirementsPayload
        {
            [JsonProperty("hardware-requirements")]
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

            [JsonProperty("connection-types")]
            public List<string> ConnectionType { get; set; }
        }

        private class MobileOperatingSystemsPayload
        {
            [JsonProperty("operating-systems")]
            public List<string> OperatingSystems { get; set; }

            [JsonProperty("operating-systems-description")]
            public string OperatingSystemsDescription { get; set; }
        }

        private class NativeMobileFirstPayload
        {
            [JsonProperty("mobile-first-design")]
            public string MobileFirstDesign { get; set; }
        }

        private class MemoryAndStoragePayload
        {
            [JsonProperty("minimum-memory-requirement")]
            public string MinimumMemoryRequirement { get; set; }

            [JsonProperty("storage-requirements-description")]
            public string Description { get; set; }
        }

        private class MobileThirdPartyPayload
        {
            [JsonProperty("third-party-components")]
            public string ThirdPartyComponents { get; set; }

            [JsonProperty("device-capabilities")]
            public string DeviceCapabilities { get; set; }
        }

        private class NativeDesktopOperatingSystemsPayload
        {
            [JsonProperty("operating-systems-description")]
            public string NativeDesktopOperatingSystemsDescription { get; set; }
        }

        private class NativeMobileAdditionalInformationPayload
        {
            [JsonProperty("additional-information")]
            public string AdditionalInformation { get; set; }
        }

        private class NativeDesktopMemoryAndStoragePayload
        {
            [JsonProperty("minimum-memory-requirement")]
            public string MinimumMemoryRequirement { get; set; }

            [JsonProperty("storage-requirements-description")]
            public string StorageRequirementsDescription { get; set; }

            [JsonProperty("minimum-cpu")]
            public string MinimumCpu { get; set; }

            [JsonProperty("recommended-resolution")]
            public string RecommendedResolution { get; set; }
        }

        private class NativeDesktopConnectivityDetails
        {
            [JsonProperty("minimum-connection-speed")]
            public string NativeDesktopMinimumConnectionSpeed { get; set; }
        }

        private class NativeDesktopThirdParty
        {
            [JsonProperty("third-party-components")]
            public string ThirdPartyComponents { get; set; }

            [JsonProperty("device-capabilities")]
            public string DeviceCapabilities { get; set; }
        }

        private class NativeDesktopAdditionalInformationPayload
        {
            [JsonProperty("additional-information")]
            public string AdditionalInformation { get; set; }
        }

        private class PublicCloudPayload
        {
            [JsonProperty("summary")]
            public string Summary { get; set; }

            [JsonProperty("link")]
            public string Link { get; set; }

            [JsonProperty("requires-hscn")]
            public List<string> RequiresHSCN { get; set; }
        }

        private class HostingPrivateCloudPayload
        {
            [JsonProperty("summary")]
            public string Summary { get; set; }

            [JsonProperty("link")]
            public string Link { get; set; }

            [JsonProperty("hosting-model")]
            public string HostingModel { get; set; }

            [JsonProperty("requires-hscn")]
            public List<string> RequiresHSCN { get; set; }
        }

        private class HostingOnPremisePayload
        {
            [JsonProperty("summary")]
            public string Summary { get; set; }

            [JsonProperty("link")]
            public string Link { get; set; }

            [JsonProperty("hosting-model")]
            public string HostingModel { get; set; }

            [JsonProperty("requires-hscn")]
            public List<string> RequiresHSCN { get; set; }
        }

        private class HostingHybridHostingTypePayload
        {
            [JsonProperty("summary")]
            public string Summary { get; set; }

            [JsonProperty("link")]
            public string Link { get; set; }

            [JsonProperty("hosting-model")]
            public string HostingModel { get; set; }

            [JsonProperty("requires-hscn")]
            public List<string> RequiresHSCN { get; set; }
        }

        private class RoadmapPayload
        {
            [JsonProperty("summary")]
            public string Summary { get; set; }
        }

        private class SupplierPayload
        {
            [JsonProperty("description")]
            public string Summary { get; set; }

            [JsonProperty("Link")]
            public string SupplierUrl { get; set; }
        }

        private class IntegrationsPayload
        {
            [JsonProperty("link")]
            public string IntegrationsUrl { get; set; }
        }

        private class ImplementationTimescalesPayload
        {
            [JsonProperty("description")]
            public string ImplementationTimescales { get; set; }
        }
        
        private class CapabilitiesPayload
        {
            [JsonProperty("capabilities")]
            public List<string> CapabilityRefs { get; set; }
        }
    }
}
