using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile
{
    public sealed class GetMobileThirdPartyResult
    {
        public GetMobileThirdPartyResult(IMobileThirdParty mobileThirdParty)
        {
            ThirdPartyComponents = mobileThirdParty?.ThirdPartyComponents;
            DeviceCapabilities = mobileThirdParty?.DeviceCapabilities;
        }

        [JsonProperty("third-party-components")]
        public string ThirdPartyComponents { get; }

        [JsonProperty("device-capabilities")]
        public string DeviceCapabilities { get; }
    }
}
