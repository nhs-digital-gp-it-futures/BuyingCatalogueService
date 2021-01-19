using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased
{
    public sealed class GetConnectivityAndResolutionResult
    {
        public GetConnectivityAndResolutionResult(string minimumConnectionSpeed, string minimumDesktopResolution)
        {
            MinimumConnectionSpeed = minimumConnectionSpeed;
            MinimumDesktopResolution = minimumDesktopResolution;
        }

        [JsonProperty("minimum-connection-speed")]
        public string MinimumConnectionSpeed { get; }

        [JsonProperty("minimum-desktop-resolution")]
        public string MinimumDesktopResolution { get; }
    }
}
