using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop
{
    public sealed class UpdateNativeDesktopHardwareRequirementsViewModel
    {
        [JsonProperty("hardware-requirements")]
        public string HardwareRequirements { get; set; }
    }
}
