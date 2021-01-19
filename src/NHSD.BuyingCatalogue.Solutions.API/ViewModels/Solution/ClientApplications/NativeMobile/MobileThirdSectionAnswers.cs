using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public sealed class MobileThirdSectionAnswers
    {
        public MobileThirdSectionAnswers(IMobileThirdParty mobileThirdParty)
        {
            ThirdPartyComponents = mobileThirdParty?.ThirdPartyComponents;
            DeviceCapabilities = mobileThirdParty?.DeviceCapabilities;
        }

        [JsonProperty("third-party-components")]
        public string ThirdPartyComponents { get; }

        [JsonProperty("device-capabilities")]
        public string DeviceCapabilities { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(ThirdPartyComponents)
            || !string.IsNullOrWhiteSpace(DeviceCapabilities);
    }
}
