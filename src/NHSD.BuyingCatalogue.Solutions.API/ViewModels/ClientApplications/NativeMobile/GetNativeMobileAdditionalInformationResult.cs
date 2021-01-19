using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile
{
    public sealed class GetNativeMobileAdditionalInformationResult
    {
        public GetNativeMobileAdditionalInformationResult(string additionalInformation)
        {
            NativeMobileAdditionalInformation = additionalInformation;
        }

        [JsonProperty("additional-information")]
        public string NativeMobileAdditionalInformation { get; }
    }
}
