using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop
{
    public sealed class GetNativeDesktopHardwareRequirementsResult
    {
        [JsonProperty("hardware-requirements")]
        public string HardwareRequirements { get; private set; }

        public GetNativeDesktopHardwareRequirementsResult(string hardwareRequirements)
        {
            HardwareRequirements = hardwareRequirements;
        }
    }
}
