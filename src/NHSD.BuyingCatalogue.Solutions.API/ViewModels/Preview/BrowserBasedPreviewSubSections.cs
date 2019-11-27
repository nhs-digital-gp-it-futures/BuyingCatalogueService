using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Preview
{
    public class BrowserBasedPreviewSubSections
    {
        [JsonProperty("browsers-supported")]
        public BrowsersSupportedPreviewSection BrowsersSupported { get; }

        [JsonProperty("plug-ins-or-extensions")]
        public PluginOrExtensionsPreviewSection PluginOrExtensionsSection { get; }

        [JsonIgnore]
        public bool HasData => BrowsersSupported.Answers.HasData || PluginOrExtensionsSection.Answers.HasData;

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowserBasedPreviewSubSections"/> class.
        /// </summary>
        public BrowserBasedPreviewSubSections(IClientApplication clientApplication)
        {
            BrowsersSupported = new BrowsersSupportedPreviewSection(clientApplication);
            PluginOrExtensionsSection = new PluginOrExtensionsPreviewSection(clientApplication);
        }
    }
}
