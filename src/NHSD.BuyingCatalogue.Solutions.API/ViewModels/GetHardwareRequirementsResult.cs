using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class GetHardwareRequirementsResult
    {
        [JsonProperty("hardware-requirements-description")]
        public string HardwareRequirements { get; set; } 
    }
}
