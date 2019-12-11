using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserAdditionalInformation
{
    public class UpdateSolutionBrowserAdditionalInformationViewModel
    {
        [JsonProperty("additional-information")]
        public string AdditionalInformation { get; set; }
    }
}
