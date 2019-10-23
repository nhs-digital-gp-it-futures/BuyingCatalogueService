using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public class BrowsersSupportedResult
    {
        [JsonProperty("supported-browsers")]
        public IEnumerable<string> BrowsersSupported { get; set; }

        [JsonProperty("mobile-responsive")]
        public string MobileResponsive { get; set; }
    }
}
