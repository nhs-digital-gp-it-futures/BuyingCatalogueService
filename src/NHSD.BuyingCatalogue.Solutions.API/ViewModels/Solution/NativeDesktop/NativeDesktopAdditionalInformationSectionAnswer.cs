using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeDesktop
{
    public class NativeDesktopAdditionalInformationSectionAnswer
    {
        [JsonProperty("additional-information")]
        public string NativeDesktopAdditionalInformation { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(NativeDesktopAdditionalInformation);

        public NativeDesktopAdditionalInformationSectionAnswer(IClientApplication clientApplication) =>
            NativeDesktopAdditionalInformation = clientApplication?.NativeDesktopAdditionalInformation;
    }
}
