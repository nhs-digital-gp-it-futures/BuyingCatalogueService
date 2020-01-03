using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileThirdParty
{
    public class UpdateSolutionMobileThirdPartyViewModel
    {
        [JsonProperty("third-party-components")]
        public string ThirdPartyComponents { get; set; }

        [JsonProperty("device-capabilities")]
        public string DeviceCapabilities { get; set; }
    }
}
