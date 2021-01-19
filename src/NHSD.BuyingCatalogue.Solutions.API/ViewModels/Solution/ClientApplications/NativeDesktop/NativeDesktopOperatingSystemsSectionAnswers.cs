using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeDesktop
{
    public sealed class NativeDesktopOperatingSystemsSectionAnswers
    {
        public NativeDesktopOperatingSystemsSectionAnswers(IClientApplication clientApplication) =>
            OperatingSystemsDescription = clientApplication?.NativeDesktopOperatingSystemsDescription;

        [JsonProperty("operating-systems-description")]
        public string OperatingSystemsDescription { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(OperatingSystemsDescription);
    }
}
