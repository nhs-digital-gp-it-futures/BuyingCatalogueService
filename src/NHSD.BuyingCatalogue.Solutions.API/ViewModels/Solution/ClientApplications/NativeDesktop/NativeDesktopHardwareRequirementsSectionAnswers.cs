using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeDesktop
{
    public sealed class NativeDesktopHardwareRequirementsSectionAnswers
    {
        public NativeDesktopHardwareRequirementsSectionAnswers(IClientApplication clientApplication) =>
            HardwareRequirements = clientApplication?.NativeDesktopHardwareRequirements;

        [JsonProperty("hardware-requirements")]
        public string HardwareRequirements { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(HardwareRequirements);
    }
}
