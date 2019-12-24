using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.BrowserBased
{
    public class GetBrowserMobileFirstResult
    {
        [JsonProperty("mobile-first-design")]
        public string MobileFirstDesign { get; set; }

        public GetBrowserMobileFirstResult(IClientApplication clientApplication)
        {
            MobileFirstDesign = clientApplication?.MobileFirstDesign.ToYesNoString();
        }
    }
}
