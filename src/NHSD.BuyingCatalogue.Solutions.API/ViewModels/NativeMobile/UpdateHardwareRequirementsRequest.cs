using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeMobile
{
    public sealed class UpdateHardwareRequirementsRequest
    {
        [JsonProperty("hardware-requirements")]
        public string HardwareRequirements { get; set; }
    }
}
