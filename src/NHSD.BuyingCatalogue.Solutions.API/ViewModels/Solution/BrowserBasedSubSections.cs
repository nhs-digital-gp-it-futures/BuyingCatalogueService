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

        [JsonIgnore]
        public bool HasData => BrowsersSupported.Answers.HasData || PluginOrExtensionsSection.Answers.HasData;

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowserBasedSubSections"/> class.
        /// </summary>
        public BrowserBasedSubSections(IClientApplication clientApplication)
        {
            BrowsersSupported = new BrowsersSupportedSection(clientApplication);
            PluginOrExtensionsSection = new PluginOrExtensionsSection(clientApplication);
        }
    }
}
