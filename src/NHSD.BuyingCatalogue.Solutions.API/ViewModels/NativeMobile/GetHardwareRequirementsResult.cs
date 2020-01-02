using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeMobile
{
    public sealed class GetHardwareRequirementsResult
    {
        [JsonProperty("hardware-requirements")]
        public string HardwareRequirements { get; private set; }

        public GetHardwareRequirementsResult(string hardwareRequirements)
        {
            HardwareRequirements = hardwareRequirements;
        }
    }
}
