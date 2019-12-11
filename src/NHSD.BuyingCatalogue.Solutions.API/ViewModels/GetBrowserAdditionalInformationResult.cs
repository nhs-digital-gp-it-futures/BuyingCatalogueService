using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class GetBrowserAdditionalInformationResult
    {
        [JsonProperty("additional-information")]
        public string AdditionalInformation { get; set; }
    }
}
