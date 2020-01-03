using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeMobile
{
    public class MobileThirdSectionAnswers
    {
        [JsonProperty("third-party-components")]
        public string ThirdPartyComponents { get; }

        [JsonProperty("device-capabilities")]
        public string DeviceCapabilities { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(ThirdPartyComponents) ||
                               !string.IsNullOrWhiteSpace(DeviceCapabilities);

        public MobileThirdSectionAnswers(IMobileThirdParty mobileThirdParty)
        {
            ThirdPartyComponents = mobileThirdParty?.ThirdPartyComponents;
            DeviceCapabilities = mobileThirdParty?.DeviceCapabilities;
        }
    }
}
