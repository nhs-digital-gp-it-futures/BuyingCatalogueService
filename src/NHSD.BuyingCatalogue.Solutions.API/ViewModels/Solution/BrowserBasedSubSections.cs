using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class BrowserBasedSubSections
    {
        [JsonProperty("browsers-supported")]
        public BrowsersSupportedSection BrowsersSupported { get; }

        [JsonProperty("plug-ins-or-extensions")]
        public PluginOrExtensionsSection PluginOrExtensionsSection { get; }

        [JsonProperty("browser-hardware-requirements")]
        public BrowserHardwareRequirementsSection BrowserHardwareRequirementsSection { get; }

        [JsonProperty("browser-additional-information")]
        public BrowserAdditionalInformationSection BrowserAdditionalInformationSection { get; }

        [JsonProperty("connectivity-and-resolution")]
        public BrowserConnectivityAndResolutionSection BrowserConnectivityAndResolutionSection { get; }

        [JsonIgnore]
        public bool HasData => BrowsersSupported.Answers.HasData || PluginOrExtensionsSection.Answers.HasData ||
                               BrowserHardwareRequirementsSection.Answers.HasData ||
                               BrowserAdditionalInformationSection.Answers.HasData ||
                               BrowserConnectivityAndResolutionSection.Answers.HasData;
        
        /// <summary>
        /// Initialises a new instance of the <see cref="BrowserBasedSubSections"/> class.
        /// </summary>
        internal BrowserBasedSubSections(IClientApplication clientApplication)
        {
            BrowsersSupported = new BrowsersSupportedSection(clientApplication);
            PluginOrExtensionsSection = new PluginOrExtensionsSection(clientApplication);
            BrowserHardwareRequirementsSection = new BrowserHardwareRequirementsSection(clientApplication);
            BrowserAdditionalInformationSection = new BrowserAdditionalInformationSection(clientApplication);
            BrowserConnectivityAndResolutionSection = new BrowserConnectivityAndResolutionSection(clientApplication);
        }
    }
}
