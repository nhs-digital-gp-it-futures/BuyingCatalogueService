using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public sealed class GetBrowsersSupportedResult
    {
        public GetBrowsersSupportedResult(ClientApplication clientApplication)
        {
            BrowsersSupported = clientApplication?.BrowsersSupported ?? new HashSet<string>();
            MobileResponsive = clientApplication?.MobileResponsive.HasValue == true
                ? (clientApplication.MobileResponsive.Value ? "yes" : "no")
                : null;
        }

        [JsonProperty("supported-browsers")]
        public IEnumerable<string> BrowsersSupported { get; }

        [JsonProperty("mobile-responsive")]
        public string MobileResponsive { get; }

    }
}
