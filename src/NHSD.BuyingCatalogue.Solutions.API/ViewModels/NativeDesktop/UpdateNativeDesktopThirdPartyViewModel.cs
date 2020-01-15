using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop
{
    public sealed class UpdateNativeDesktopThirdPartyViewModel : IUpdateNativeDesktopThirdPartyData
    {
        [JsonProperty("third-party-components")]
        public string ThirdPartyComponents { get; set; }

        [JsonProperty("device-capabilities")]
        public string DeviceCapabilities { get; set; }
    }
}
