using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionPlugins
{
    public sealed class UpdateSolutionPluginsViewModel
    {
        [JsonProperty("plugins-required")]
        public string Required { get; set; }

        [JsonProperty("plugins-detail")]
        public string AdditionalInformation { get; set; }
    }
}
