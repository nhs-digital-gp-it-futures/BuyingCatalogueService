using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Public
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
            Required = clientApplicationPlugins?.Required.ToYesNoString();
            AdditionalInformation = clientApplicationPlugins?.AdditionalInformation;
        }
    }
}
