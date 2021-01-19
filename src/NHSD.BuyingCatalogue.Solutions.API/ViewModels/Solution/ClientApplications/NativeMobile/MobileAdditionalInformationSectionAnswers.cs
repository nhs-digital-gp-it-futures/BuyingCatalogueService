using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public sealed class MobileAdditionalInformationSectionAnswers
    {
        public MobileAdditionalInformationSectionAnswers(IClientApplication clientApplication) =>
            NativeMobileAdditionalInformation = clientApplication?.NativeMobileAdditionalInformation;

        [JsonProperty("additional-information")]
        public string NativeMobileAdditionalInformation { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(NativeMobileAdditionalInformation);
    }
}
