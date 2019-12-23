using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class GetSolutionConnectivityAndResolutionResult
    {
        [JsonProperty("minimum-connection-speed")]
        public string MinimumConnectionSpeed { get; }

        [JsonProperty("minimum-desktop-resolution")]
        public string MinimumDesktopResolution { get; }

        public GetSolutionConnectivityAndResolutionResult(string minimumConnectionSpeed, string minimumDesktopResolution)
        {
            MinimumConnectionSpeed = minimumConnectionSpeed;
            MinimumDesktopResolution = minimumDesktopResolution;
        }
    }
}
