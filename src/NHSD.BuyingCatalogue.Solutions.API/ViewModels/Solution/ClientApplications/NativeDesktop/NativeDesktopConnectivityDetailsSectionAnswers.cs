using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeDesktop
{
    public sealed class NativeDesktopConnectivityDetailsSectionAnswers
    {
        public NativeDesktopConnectivityDetailsSectionAnswers(IClientApplication clientApplication) =>
            NativeDesktopMinimumConnectionSpeed = clientApplication?.NativeDesktopMinimumConnectionSpeed;

        [JsonProperty("minimum-connection-speed")]
        public string NativeDesktopMinimumConnectionSpeed { get; set; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(NativeDesktopMinimumConnectionSpeed);
    }
}
