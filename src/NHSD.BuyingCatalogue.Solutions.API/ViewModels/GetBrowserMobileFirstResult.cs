using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class GetBrowserMobileFirstResult
    {
        [JsonProperty("mobile-first-design")]
        public bool? MobileFirstDesign { get; set; }
    }
}
