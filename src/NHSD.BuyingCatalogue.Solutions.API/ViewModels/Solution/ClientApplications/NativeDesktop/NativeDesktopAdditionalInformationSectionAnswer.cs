using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeDesktop
{
    public sealed class NativeDesktopAdditionalInformationSectionAnswer
    {
        public NativeDesktopAdditionalInformationSectionAnswer(IClientApplication clientApplication) =>
            NativeDesktopAdditionalInformation = clientApplication?.NativeDesktopAdditionalInformation;

        [JsonProperty("additional-information")]
        public string NativeDesktopAdditionalInformation { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(NativeDesktopAdditionalInformation);
    }
}
