using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public sealed class GetBrowsersSupportedResult
    {
        [JsonProperty("supported-browsers")]
        public IEnumerable<string> BrowsersSupported { get; }

        [JsonProperty("mobile-responsive")]
        public string MobileResponsive { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetBrowsersSupportedResult"/> class.
        /// </summary>
        public GetBrowsersSupportedResult(ClientApplication clientApplication)
        {
            bool? clientApplicationMobileResponsive = clientApplication?.MobileResponsive;

            BrowsersSupported = clientApplication?.BrowsersSupported ?? new HashSet<string>();
            MobileResponsive = clientApplicationMobileResponsive.HasValue
                ? clientApplicationMobileResponsive.Value ? "yes" : "no"
                : null;
        }
    }
}
