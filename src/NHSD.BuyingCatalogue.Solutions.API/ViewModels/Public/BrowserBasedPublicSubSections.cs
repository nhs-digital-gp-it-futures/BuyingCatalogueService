using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Public
{
    public class BrowserBasedPublicSubSections
    {
        [JsonProperty("browsers-supported")]
        public BrowsersSupportedPublicSection BrowsersSupported { get; }

        [JsonProperty("plug-ins-or-extensions")]
        public PluginOrExtensionsPublicSection PluginOrExtensionsSection { get; }

        [JsonIgnore]
        public bool HasData => BrowsersSupported.Answers.HasData || PluginOrExtensionsSection.Answers.HasData;

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowserBasedPublicSubSections"/> class.
        /// </summary>
        public BrowserBasedPublicSubSections(IClientApplication clientApplication)
        {
            BrowsersSupported = new BrowsersSupportedPublicSection(clientApplication);
            PluginOrExtensionsSection = new PluginOrExtensionsPublicSection(clientApplication);
        }
    }
}
