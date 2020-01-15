using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased
{
    public sealed class UpdateBrowserBasedHardwareRequirementViewModel
    {
        [JsonProperty("hardware-requirements-description")]
        public string HardwareRequirements { get; set; }
    }
}
