using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased
{
    public sealed class GetBrowserMobileFirstResult
    {
        public GetBrowserMobileFirstResult(IClientApplication clientApplication)
        {
            MobileFirstDesign = clientApplication?.MobileFirstDesign.ToYesNoString();
        }

        [JsonProperty("mobile-first-design")]
        public string MobileFirstDesign { get; }
    }
}
