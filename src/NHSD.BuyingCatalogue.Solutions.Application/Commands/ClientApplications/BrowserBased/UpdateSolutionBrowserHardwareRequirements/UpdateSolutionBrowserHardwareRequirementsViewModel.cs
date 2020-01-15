using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionBrowserHardwareRequirements
{
    public sealed class UpdateSolutionBrowserHardwareRequirementsViewModel
    {
        [JsonProperty("hardware-requirements-description")]
        public string HardwareRequirements { get; set; }
    }
}
