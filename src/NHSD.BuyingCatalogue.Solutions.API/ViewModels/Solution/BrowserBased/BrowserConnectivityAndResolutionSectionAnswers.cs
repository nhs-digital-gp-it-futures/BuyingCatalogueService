using System;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.BrowserBased
{
    public class BrowserConnectivityAndResolutionSectionAnswers
    {
        [JsonProperty("minimum-connection-speed")]
        public string MinimumConnectionSpeed { get; }

        [JsonProperty("minimum-desktop-resolution")]
        public string MinimumDesktopResolution { get; }

        [JsonIgnore] public bool HasData => !String.IsNullOrWhiteSpace(MinimumConnectionSpeed) || !String.IsNullOrWhiteSpace(MinimumDesktopResolution);

        public BrowserConnectivityAndResolutionSectionAnswers(IClientApplication clientApplication)
        {
            MinimumConnectionSpeed = clientApplication?.MinimumConnectionSpeed;
            MinimumDesktopResolution = clientApplication?.MinimumDesktopResolution;
        }
    }
}
