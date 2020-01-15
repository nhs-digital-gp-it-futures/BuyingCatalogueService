using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.BrowserBased
{
    public class BrowserHardwareRequirementsSectionAnswers
    {
        [JsonProperty("hardware-requirements-description")]
        public string HardwareRequirements { get; }

        [JsonIgnore] public bool HasData => HardwareRequirements?.Any() == true;

        public BrowserHardwareRequirementsSectionAnswers(IClientApplication clientApplication) =>
            HardwareRequirements = clientApplication?.HardwareRequirements;
    }
}
