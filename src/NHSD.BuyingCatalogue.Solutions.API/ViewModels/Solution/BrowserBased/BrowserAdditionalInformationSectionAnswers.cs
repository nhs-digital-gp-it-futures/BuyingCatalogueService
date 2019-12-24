using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.BrowserBased
{
    public class BrowserAdditionalInformationSectionAnswers
    {
        [JsonProperty("additional-information")]
        public string AdditionalInformation { get; }

        [JsonIgnore]
        public bool HasData => AdditionalInformation?.Any() == true;

        public BrowserAdditionalInformationSectionAnswers(IClientApplication clientApplication) =>
            AdditionalInformation = clientApplication?.AdditionalInformation;
    }
}
