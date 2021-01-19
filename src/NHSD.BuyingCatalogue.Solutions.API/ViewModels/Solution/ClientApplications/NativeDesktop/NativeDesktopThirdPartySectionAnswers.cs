using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeDesktop
{
    public sealed class NativeDesktopThirdPartySectionAnswers
    {
        public NativeDesktopThirdPartySectionAnswers(INativeDesktopThirdParty nativeDesktopThirdParty)
        {
            ThirdPartyComponents = nativeDesktopThirdParty?.ThirdPartyComponents;
            DeviceCapabilities = nativeDesktopThirdParty?.DeviceCapabilities;
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
