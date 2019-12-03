using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class PluginOrExtensionsSectionAnswers
    {
        [JsonProperty("plugins-required")]
        public string Required { get; }

        [JsonProperty("plugins-detail")]
        public string AdditionalInformation { get; }

        [JsonIgnore]
        public bool HasData => Required != null;

        /// <summary>
        /// Initialises a new instance of the <see cref="PluginOrExtensionsSectionAnswers"/> class.
        /// </summary>
        public PluginOrExtensionsSectionAnswers(IPlugins clientApplicationPlugins)
        {
            Required = clientApplicationPlugins?.Required.ToYesNoString();
            AdditionalInformation = clientApplicationPlugins?.AdditionalInformation;
        }
    }
}
