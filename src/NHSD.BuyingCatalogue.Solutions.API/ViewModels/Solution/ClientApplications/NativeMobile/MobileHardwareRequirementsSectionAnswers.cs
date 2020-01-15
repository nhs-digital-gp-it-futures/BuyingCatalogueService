using System;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public class MobileHardwareRequirementsSectionAnswers
    {
        [JsonProperty("hardware-requirements")]
        public string HardwareRequirements { get; }

        [JsonIgnore] public bool HasData => !String.IsNullOrWhiteSpace(HardwareRequirements);

        public MobileHardwareRequirementsSectionAnswers(IClientApplication clientApplication) =>
            HardwareRequirements = clientApplication?.NativeMobileHardwareRequirements;
    }
}
