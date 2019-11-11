using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts;

namespace NHSD.BuyingCatalogue.API.ViewModels.Preview
{
    public class PluginOrExtensionsPreviewSectionAnswers
    {
        [JsonProperty("plugins-required")]
        public string Required { get; }

        [JsonProperty("plugins-detail")]
        public string AdditionalInformation { get; }

        [JsonIgnore]
        public bool HasData => Required != null;

        /// <summary>
        /// Initialises a new instance of the <see cref="PluginOrExtensionsPreviewSectionAnswers"/> class.
        /// </summary>
        public PluginOrExtensionsPreviewSectionAnswers(IPlugins clientApplicationPlugins)
        {
            bool? pluginRequirement = clientApplicationPlugins?.Required;

            Required = pluginRequirement.HasValue ? pluginRequirement.Value ? "yes" : "no" : null;
            AdditionalInformation = clientApplicationPlugins?.AdditionalInformation;
        }
    }
}
