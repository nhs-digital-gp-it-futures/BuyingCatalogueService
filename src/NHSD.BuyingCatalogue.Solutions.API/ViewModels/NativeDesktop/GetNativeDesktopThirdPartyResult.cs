using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop
{
    public sealed class GetNativeDesktopThirdPartyResult
    {
        [JsonProperty("third-party-components")]
        public string ThirdPartyComponents { get; set; }

        [JsonProperty("device-capabilities")]
        public string DeviceCapabilities { get; set; }

        public GetNativeDesktopThirdPartyResult(INativeDesktopThirdParty nativeDesktopThirdParty)
        {
            ThirdPartyComponents = nativeDesktopThirdParty?.ThirdPartyComponents;
            DeviceCapabilities = nativeDesktopThirdParty?.DeviceCapabilities;
        }
    }
}
