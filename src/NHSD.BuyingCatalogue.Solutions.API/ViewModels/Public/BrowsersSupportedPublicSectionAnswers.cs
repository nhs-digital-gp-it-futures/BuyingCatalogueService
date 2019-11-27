using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts.Solutions;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Public
{
    public class BrowsersSupportedPublicSectionAnswers
    {
        [JsonProperty("supported-browsers")]
        public IEnumerable<string> SupportedBrowsers { get; }

        [JsonProperty("mobile-responsive")]
        public string MobileResponsive { get; }

        [JsonIgnore]
        public bool HasData => SupportedBrowsers?.Any() == true || MobileResponsive != null;

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowsersSupportedPublicSectionAnswers"/> class.
        /// </summary>
        public BrowsersSupportedPublicSectionAnswers(IClientApplication clientApplication)
        {
            bool? mobileResponsive = clientApplication?.MobileResponsive;

            SupportedBrowsers = clientApplication?.BrowsersSupported;
            MobileResponsive = mobileResponsive.ToYesNoString();
        }
    }
}
