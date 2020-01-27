using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands.BrowserBased
{
    public interface  IUpdateBrowserBasedConnectivityAndResolutionData
    {
        [JsonProperty("minimum-connection-speed")]
        string MinimumConnectionSpeed { get; }

        [JsonProperty("minimum-desktop-resolution")]
        string MinimumDesktopResolution { get; }
    }
}
