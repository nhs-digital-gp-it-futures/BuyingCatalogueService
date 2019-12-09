using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionConnectivityAndResolution
{
    public class UpdateSolutionConnectivityAndResolutionViewModel
    {
        [JsonProperty("minimum-connection-speed")]
        public string MinimumConnectionSpeed { get; set; }

        [JsonProperty("minimum-desktop-resolution")]
        public string MinimumDesktopResolution { get; set; }
    }
}
