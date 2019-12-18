using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class UpdateSolutionNativeMobileFirstResult
    {
        [JsonProperty("mobile-first-design")]
        public string MobileFirstDesign { get; set; }
    }
}
