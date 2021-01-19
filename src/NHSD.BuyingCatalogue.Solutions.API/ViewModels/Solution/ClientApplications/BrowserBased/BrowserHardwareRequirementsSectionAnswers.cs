using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.BrowserBased
{
    public sealed class BrowserHardwareRequirementsSectionAnswers
    {
        public BrowserHardwareRequirementsSectionAnswers(IClientApplication clientApplication) =>
            HardwareRequirements = clientApplication?.HardwareRequirements;

        [JsonProperty("hardware-requirements-description")]
        public string HardwareRequirements { get; }

        [JsonIgnore]
        public bool HasData => HardwareRequirements?.Any() == true;
    }
}
