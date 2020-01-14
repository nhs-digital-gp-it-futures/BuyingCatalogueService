using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop
{
    public sealed class GetNativeDesktopAdditionalInformationResult
    {
        [JsonProperty("additional-information")]
        public string NativeDesktopAdditionalInformation { get; set; }

        public GetNativeDesktopAdditionalInformationResult(string additionalInformation)
        {
            NativeDesktopAdditionalInformation = additionalInformation;
        }
    }
}
