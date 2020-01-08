using System;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeDesktop
{
    public sealed class NativeDesktopHardwareRequirementsSectionAnswers
    {
        [JsonProperty("hardware-requirements")]
        public string HardwareRequirements { get; }

        [JsonIgnore] public bool HasData => !String.IsNullOrWhiteSpace(HardwareRequirements);

        public NativeDesktopHardwareRequirementsSectionAnswers(IClientApplication clientApplication) =>
            HardwareRequirements = clientApplication?.NativeDesktopHardwareRequirements;
    }
}
