using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels.Preview
{
    public class BrowsersSupportedPreviewSectionAnswers
    {
        [JsonProperty("supported-browsers")]
        public IEnumerable<string> SupportedBrowsers { get; }

        [JsonProperty("mobile-responsive")]
        public string MobileResponsive { get; }

        [JsonIgnore]
        public bool HasData => SupportedBrowsers?.Any() == true || MobileResponsive != null;

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowsersSupportedPreviewSectionAnswers"/> class.
        /// </summary>
        public BrowsersSupportedPreviewSectionAnswers(IClientApplication clientApplication)
        {
            bool? mobileResponsive = clientApplication?.MobileResponsive;

            SupportedBrowsers = clientApplication?.BrowsersSupported;
            MobileResponsive = mobileResponsive.HasValue
                ? mobileResponsive.Value ? "yes" : "no"
                : null;
        }
    }
}
