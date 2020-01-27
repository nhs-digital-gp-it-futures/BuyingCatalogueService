using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.BrowserBased;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased
{
    public sealed class UpdateBrowserBasedPluginsViewModel : IUpdateBrowserBasedPluginsData
    {
        [JsonProperty("plugins-required")]
        public string Required { get; set; }

        [JsonProperty("plugins-detail")]
        public string AdditionalInformation { get; set; }
    }
}
