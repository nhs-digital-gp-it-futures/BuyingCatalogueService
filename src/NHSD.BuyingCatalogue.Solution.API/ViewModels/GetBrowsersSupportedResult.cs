using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts.Solutions;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solution.API.ViewModels
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
        public GetBrowsersSupportedResult(IClientApplication clientApplication)
        {
            bool? clientApplicationMobileResponsive = clientApplication?.MobileResponsive;

            BrowsersSupported = clientApplication?.BrowsersSupported ?? new HashSet<string>();
            MobileResponsive = clientApplicationMobileResponsive.ToYesNoString();
        }
    }
}
