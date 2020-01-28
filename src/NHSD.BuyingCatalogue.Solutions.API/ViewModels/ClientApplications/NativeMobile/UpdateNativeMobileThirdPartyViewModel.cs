using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.NativeMobile;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile
{
    public sealed class UpdateNativeMobileThirdPartyViewModel : IUpdateNativeMobileThirdPartyData
    {
        [JsonProperty("third-party-components")]
        public string ThirdPartyComponents { get; set; }

        [JsonProperty("device-capabilities")]
        public string DeviceCapabilities { get; set; }
    }
}
