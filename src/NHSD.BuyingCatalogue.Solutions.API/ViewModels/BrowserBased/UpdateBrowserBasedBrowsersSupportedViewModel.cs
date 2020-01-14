using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.BrowserBased;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.BrowserBased
{
    public sealed class UpdateBrowserBasedBrowsersSupportedViewModel : IUpdateBrowserBasedBrowsersSupportedData
    {
        [JsonProperty("supported-browsers")]
        public HashSet<string> BrowsersSupported { get; internal set; }

        [JsonProperty("mobile-responsive")]
        public string MobileResponsive { get; internal set; }
    }
}
