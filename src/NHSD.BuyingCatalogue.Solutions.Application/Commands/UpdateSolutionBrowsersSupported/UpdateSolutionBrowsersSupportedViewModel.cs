using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowsersSupported
{
    public sealed class UpdateSolutionBrowsersSupportedViewModel
    {
        [JsonProperty("supported-browsers")]
        public HashSet<string> BrowsersSupported { get; internal set; }

        [JsonProperty("mobile-responsive")]
        public string MobileResponsive { get; internal set; }
    }
}
