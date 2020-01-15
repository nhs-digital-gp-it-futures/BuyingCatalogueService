using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public class MobileAdditionalInformationSectionAnswers
    {
        [JsonProperty("additional-information")]
        public string NativeMobileAdditionalInformation { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(NativeMobileAdditionalInformation);

        public MobileAdditionalInformationSectionAnswers(IClientApplication clientApplication) =>
            NativeMobileAdditionalInformation = clientApplication?.NativeMobileAdditionalInformation;
    }
}
