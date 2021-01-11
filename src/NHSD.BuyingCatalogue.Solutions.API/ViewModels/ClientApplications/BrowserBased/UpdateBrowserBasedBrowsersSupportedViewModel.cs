using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.BrowserBased;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased
{
    public sealed class UpdateBrowserBasedBrowsersSupportedViewModel : IUpdateBrowserBasedBrowsersSupportedData
    {
        [JsonProperty("supported-browsers")]
        public HashSet<string> BrowsersSupported { get; internal set; } = new();

        [JsonProperty("mobile-responsive")]
        public string MobileResponsive { get; internal set; }
    }
}
