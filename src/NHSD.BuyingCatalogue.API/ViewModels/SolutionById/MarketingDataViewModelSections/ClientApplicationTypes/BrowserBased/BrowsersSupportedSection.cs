using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public class BrowsersSupportedSection : Section
    {
        internal BrowsersSupportedSection(ClientApplication clientApplication)
        {
            Data = new BrowserSupportedSectionData(clientApplication);
            Mandatory.Add("supported-browsers");
            Mandatory.Add("mobile-responsive");
            _isComplete = Data.SupportedBrowsers?.Any() == true && Data.MobileResponsive != null;
        }

        public BrowserSupportedSectionData Data { get; }

        public override string Id => "browsers-supported";
    }

    public class BrowserSupportedSectionData
    {
        internal BrowserSupportedSectionData(ClientApplication clientApplication)
        {
            SupportedBrowsers = clientApplication?.BrowsersSupported ?? new HashSet<string>();
            MobileResponsive = clientApplication?.MobileResponsive.HasValue == true
                ? (clientApplication.MobileResponsive.Value ? "yes" : "no")
                : null;
        }

        [JsonProperty("supported-browsers")]
        public HashSet<string> SupportedBrowsers { get; set; } = new HashSet<string>();

        [JsonProperty("mobile-responsive")]
        public string MobileResponsive { get; set; }
    }
}
