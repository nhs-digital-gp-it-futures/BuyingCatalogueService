using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class GetConnectivityAndResolutionResult
    {
        [JsonProperty("minimum-connection-speed")]
        public string MinimumConnectionSpeed { get; set; }

        [JsonProperty("minimum-desktop-resolution")]
        public string MinimumDesktopResolution { get; set; }
    }
}
