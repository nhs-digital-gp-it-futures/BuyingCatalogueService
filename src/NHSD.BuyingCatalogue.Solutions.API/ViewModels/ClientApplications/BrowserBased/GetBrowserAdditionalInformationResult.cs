using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased
{
    public sealed class GetBrowserAdditionalInformationResult
    {
        public GetBrowserAdditionalInformationResult(string additionalInformation)
        {
            AdditionalInformation = additionalInformation;
        }

        [JsonProperty("additional-information")]
        public string AdditionalInformation { get; }
    }
}
