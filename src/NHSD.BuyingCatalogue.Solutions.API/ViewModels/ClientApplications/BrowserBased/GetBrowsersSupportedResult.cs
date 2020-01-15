using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased
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
