using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Preview
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

            Required = clientApplicationPlugins?.Required.ToYesNoString();
            AdditionalInformation = clientApplicationPlugins?.AdditionalInformation;
        }
    }
}
