using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.BrowserBased;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.BrowserBased
{
    public sealed class UpdateBrowserBasedConnectivityAndResolutionViewModel : IUpdateBrowserBasedConnectivityAndResolutionData
    {
        [JsonProperty("minimum-connection-speed")]
        public string MinimumConnectionSpeed { get; set; }

        [JsonProperty("minimum-desktop-resolution")]
        public string MinimumDesktopResolution { get; set; }

        public IUpdateBrowserBasedConnectivityAndResolutionData Trim()
        {
            return new UpdateBrowserBasedConnectivityAndResolutionViewModel()
            {
                MinimumConnectionSpeed = MinimumConnectionSpeed?.Trim(),
                MinimumDesktopResolution = MinimumDesktopResolution?.Trim()
            };
        }
    }
}
