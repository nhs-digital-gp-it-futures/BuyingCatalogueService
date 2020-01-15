using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased
{
    public sealed class UpdateBrowserBasedAdditionalInformationViewModel
    { 
        [JsonProperty("additional-information")]
        public string AdditionalInformation { get; set; }
    }
}
