using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.BrowserBased
{
    public sealed class BrowserConnectivityAndResolutionSectionAnswers
    {
        public BrowserConnectivityAndResolutionSectionAnswers(IClientApplication clientApplication)
        {
            MinimumConnectionSpeed = clientApplication?.MinimumConnectionSpeed;
            MinimumDesktopResolution = clientApplication?.MinimumDesktopResolution;
        }

        [JsonProperty("minimum-connection-speed")]
        public string MinimumConnectionSpeed { get; }

        [JsonProperty("minimum-desktop-resolution")]
        public string MinimumDesktopResolution { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(MinimumConnectionSpeed)
            || !string.IsNullOrWhiteSpace(MinimumDesktopResolution);
    }
}
