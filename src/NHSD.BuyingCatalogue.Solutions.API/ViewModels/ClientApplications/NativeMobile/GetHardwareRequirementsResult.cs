using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile
{
    public sealed class GetHardwareRequirementsResult
    {
        public GetHardwareRequirementsResult(string hardwareRequirements)
        {
            HardwareRequirements = hardwareRequirements;
        }

        [JsonProperty("hardware-requirements")]
        public string HardwareRequirements { get; }
    }
}
