using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.BrowserBased
{
    public sealed class UpdateBrowserBasedMobileFirstViewModel
    {
        [JsonProperty("mobile-first-design")]
        public string MobileFirstDesign { get; set; }
    }
}
