using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop
{
    public sealed class GetNativeDesktopHardwareRequirementsResult
    {
        public GetNativeDesktopHardwareRequirementsResult(string hardwareRequirements)
        {
            HardwareRequirements = hardwareRequirements;
        }

        [JsonProperty("hardware-requirements")]
        public string HardwareRequirements { get; }
    }
}
