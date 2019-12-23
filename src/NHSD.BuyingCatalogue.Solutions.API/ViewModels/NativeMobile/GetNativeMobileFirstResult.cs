using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class GetNativeMobileFirstResult
    {
        [JsonProperty("mobile-first-design")]
        public string MobileFirstDesign { get; set; }

        public GetNativeMobileFirstResult(IClientApplication clientApplication)
        {
            MobileFirstDesign = clientApplication?.NativeMobileFirstDesign.ToYesNoString();
        }
    }
}
