using System;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeDesktop
{
    public class NativeDesktopOperatingSystemsSectionAnswers
    {
        [JsonProperty("operating-systems-description")]
        public string OperatingSystemsDescription { get; }

        [JsonIgnore] public bool HasData => !String.IsNullOrWhiteSpace(OperatingSystemsDescription);

        public NativeDesktopOperatingSystemsSectionAnswers(IClientApplication clientApplication) =>
            OperatingSystemsDescription = clientApplication?.NativeDesktopOperatingSystemsDescription;
    }
}
