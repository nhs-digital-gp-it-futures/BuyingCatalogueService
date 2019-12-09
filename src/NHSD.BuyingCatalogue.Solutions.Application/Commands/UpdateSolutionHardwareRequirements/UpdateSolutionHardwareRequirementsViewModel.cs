using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionHardwareRequirements
{
    public sealed class UpdateSolutionHardwareRequirementsViewModel
    {
        [JsonProperty("hardware-requirements-description")]
        public string HardwareRequirements { get; set; }
    }
}
