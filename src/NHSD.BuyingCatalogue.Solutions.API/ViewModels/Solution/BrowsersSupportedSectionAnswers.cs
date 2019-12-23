using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class BrowsersSupportedSectionAnswers
    {
        [JsonProperty("supported-browsers")]
        public IEnumerable<string> SupportedBrowsers { get; }

        [JsonProperty("mobile-responsive")]
        public string MobileResponsive { get; }

        [JsonIgnore]
        public bool HasData => SupportedBrowsers?.Any() == true || MobileResponsive != null;

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowsersSupportedSectionAnswers"/> class.
        /// </summary>
        public BrowsersSupportedSectionAnswers(IClientApplication clientApplication)
        {
            bool? mobileResponsive = clientApplication?.MobileResponsive;

            SupportedBrowsers = clientApplication?.BrowsersSupported?.Any() == true ? clientApplication?.BrowsersSupported : null;
            MobileResponsive = mobileResponsive.ToYesNoString();
        }
    }
}
