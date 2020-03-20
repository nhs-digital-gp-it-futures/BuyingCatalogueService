using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile
{
    public sealed class UpdateHardwareRequirementsRequest
    {
        [JsonProperty("hardware-requirements")]
        public string HardwareRequirements { get; set; }
    }
}
