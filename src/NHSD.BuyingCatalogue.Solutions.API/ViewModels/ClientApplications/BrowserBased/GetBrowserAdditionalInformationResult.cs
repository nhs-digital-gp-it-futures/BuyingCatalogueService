using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased
{
    public class GetBrowserAdditionalInformationResult
    {
        [JsonProperty("additional-information")]
        public string AdditionalInformation { get; set; }

        public GetBrowserAdditionalInformationResult(string additionalInformation)
        {
            AdditionalInformation = additionalInformation;
        }
    }
}
