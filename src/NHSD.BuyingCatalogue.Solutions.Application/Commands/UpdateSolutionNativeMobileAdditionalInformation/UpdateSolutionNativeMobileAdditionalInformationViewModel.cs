using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionNativeMobileAdditionalInformation
{
    public class UpdateSolutionNativeMobileAdditionalInformationViewModel
    {
        [JsonProperty("additional-information")]
        public string NativeMobileAdditionalInformation { get; set; }
    }
}
