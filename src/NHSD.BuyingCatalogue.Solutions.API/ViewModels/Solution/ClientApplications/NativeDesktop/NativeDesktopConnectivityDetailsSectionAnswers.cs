using System;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeDesktop
{
    public sealed class NativeDesktopConnectivityDetailsSectionAnswers
    {
        [JsonProperty("minimum-connection-speed")]
        public string NativeDesktopMinimumConnectionSpeed { get; set; }

        [JsonIgnore]
        public bool HasData => !String.IsNullOrWhiteSpace(NativeDesktopMinimumConnectionSpeed);

        public NativeDesktopConnectivityDetailsSectionAnswers(IClientApplication clientApplication) =>
            NativeDesktopMinimumConnectionSpeed = clientApplication?.NativeDesktopMinimumConnectionSpeed;
    }
}
