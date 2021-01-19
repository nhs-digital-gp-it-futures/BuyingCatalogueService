using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public sealed class MobileHardwareRequirementsSectionAnswers
    {
        public MobileHardwareRequirementsSectionAnswers(IClientApplication clientApplication) =>
            HardwareRequirements = clientApplication?.NativeMobileHardwareRequirements;

        [JsonProperty("hardware-requirements")]
        public string HardwareRequirements { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(HardwareRequirements);
    }
}
