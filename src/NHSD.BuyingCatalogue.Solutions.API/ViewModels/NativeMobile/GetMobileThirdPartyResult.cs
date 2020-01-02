using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeMobile
{
    public sealed class GetMobileThirdPartyResult
    {
        [JsonProperty("third-party-components")]
        public string ThirdPartyComponents { get; set; }

        [JsonProperty("device-capabilities")]
        public string DeviceCapabilities { get; set; }

        public GetMobileThirdPartyResult(IMobileThirdParty mobileThirdParty)
        {
            ThirdPartyComponents = mobileThirdParty?.ThirdPartyComponents;
            DeviceCapabilities = mobileThirdParty?.DeviceCapabilities;
        }
    }
}
