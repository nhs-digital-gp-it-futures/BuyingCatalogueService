using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop
{
    public sealed class GetNativeDesktopThirdPartyResult
    {
        public GetNativeDesktopThirdPartyResult(INativeDesktopThirdParty nativeDesktopThirdParty)
        {
            ThirdPartyComponents = nativeDesktopThirdParty?.ThirdPartyComponents;
            DeviceCapabilities = nativeDesktopThirdParty?.DeviceCapabilities;
        }

        [JsonProperty("third-party-components")]
        public string ThirdPartyComponents { get; }

        [JsonProperty("device-capabilities")]
        public string DeviceCapabilities { get; }
    }
}
