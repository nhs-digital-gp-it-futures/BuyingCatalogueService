using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution
{
    public sealed class UpdateSolutionBrowsersSupportedViewModel
    {
        [JsonProperty("supported-browsers")]
        public HashSet<string> BrowsersSupported { get; set; }

        [JsonProperty("mobile-responsive")]
        public string MobileResponsive { get; set; }
    }
}
