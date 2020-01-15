using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile
{
    public class GetNativeMobileAdditionalInformationResult
    {
        [JsonProperty("additional-information")]
        public string NativeMobileAdditionalInformation { get; set; }

        public GetNativeMobileAdditionalInformationResult(string additionalInformation)
        {
            NativeMobileAdditionalInformation = additionalInformation;
        }
    }
}
