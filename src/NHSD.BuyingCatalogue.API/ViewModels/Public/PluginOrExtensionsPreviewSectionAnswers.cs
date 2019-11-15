using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts;

namespace NHSD.BuyingCatalogue.API.ViewModels.Public
{
    public class PluginOrExtensionsPublicSectionAnswers
    {
        [JsonProperty("plugins-required")]
        public string Required { get; }

        [JsonProperty("plugins-detail")]
        public string AdditionalInformation { get; }

        [JsonIgnore]
        public bool HasData => Required != null;

        /// <summary>
        /// Initialises a new instance of the <see cref="PluginOrExtensionsPublicSectionAnswers"/> class.
        /// </summary>
        public PluginOrExtensionsPublicSectionAnswers(IPlugins clientApplicationPlugins)
        {
            bool? pluginRequirement = clientApplicationPlugins?.Required;

            Required = pluginRequirement.HasValue ? pluginRequirement.Value ? "yes" : "no" : null;
            AdditionalInformation = clientApplicationPlugins?.AdditionalInformation;
        }
    }
}
