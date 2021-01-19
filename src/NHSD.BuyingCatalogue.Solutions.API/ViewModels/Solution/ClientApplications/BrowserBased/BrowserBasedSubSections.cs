﻿using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.BrowserBased
{
    public sealed class BrowserBasedSubSections
    {
        internal BrowserBasedSubSections(IClientApplication clientApplication)
        {
            BrowsersSupported = new BrowsersSupportedSection(clientApplication);
            PluginOrExtensionsSection = new PluginOrExtensionsSection(clientApplication);
            BrowserHardwareRequirementsSection = new BrowserHardwareRequirementsSection(clientApplication);
            BrowserAdditionalInformationSection = new BrowserAdditionalInformationSection(clientApplication);
            BrowserConnectivityAndResolutionSection = new BrowserConnectivityAndResolutionSection(clientApplication);
            BrowserMobileFirstSection = new BrowserMobileFirstSection(clientApplication);
        }

        [JsonProperty("browser-browsers-supported")]
        public BrowsersSupportedSection BrowsersSupported { get; }

        [JsonProperty("browser-plug-ins-or-extensions")]
        public PluginOrExtensionsSection PluginOrExtensionsSection { get; }

        [JsonProperty("browser-hardware-requirements")]
        public BrowserHardwareRequirementsSection BrowserHardwareRequirementsSection { get; }

        [JsonProperty("browser-additional-information")]
        public BrowserAdditionalInformationSection BrowserAdditionalInformationSection { get; }

        [JsonProperty("browser-connectivity-and-resolution")]
        public BrowserConnectivityAndResolutionSection BrowserConnectivityAndResolutionSection { get; }

        [JsonProperty("browser-mobile-first")]
        public BrowserMobileFirstSection BrowserMobileFirstSection { get; }

        [JsonIgnore]
        public bool HasData => BrowsersSupported.Answers.HasData
            || PluginOrExtensionsSection.Answers.HasData
            || BrowserHardwareRequirementsSection.Answers.HasData
            || BrowserAdditionalInformationSection.Answers.HasData
            || BrowserConnectivityAndResolutionSection.Answers.HasData
            || BrowserMobileFirstSection.Answers.HasData;
    }
}
