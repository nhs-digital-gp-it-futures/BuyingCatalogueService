using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.BrowserBased
{
    public sealed class BrowserAdditionalInformationSectionAnswers
    {
        public BrowserAdditionalInformationSectionAnswers(IClientApplication clientApplication) =>
            AdditionalInformation = clientApplication?.AdditionalInformation;

        [JsonProperty("additional-information")]
        public string AdditionalInformation { get; }

        [JsonIgnore]
        public bool HasData => AdditionalInformation?.Any() == true;
    }
}
