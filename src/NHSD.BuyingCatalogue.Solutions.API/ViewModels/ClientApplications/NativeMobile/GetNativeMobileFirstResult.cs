using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile
{
    public sealed class GetNativeMobileFirstResult
    {
        public GetNativeMobileFirstResult(IClientApplication clientApplication)
        {
            MobileFirstDesign = clientApplication?.NativeMobileFirstDesign.ToYesNoString();
        }

        [JsonProperty("mobile-first-design")]
        public string MobileFirstDesign { get; }
    }
}
