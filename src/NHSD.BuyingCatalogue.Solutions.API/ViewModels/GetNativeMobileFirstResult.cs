using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class GetNativeMobileFirstResult
    {
        [JsonProperty("mobile-first-design")]
        public string MobileFirstDesign { get; set; }

        public GetNativeMobileFirstResult()
        {
        }
    }
}
