using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop
{
    public sealed class GetNativeDesktopAdditionalInformationResult
    {
        public GetNativeDesktopAdditionalInformationResult(string additionalInformation)
        {
            NativeDesktopAdditionalInformation = additionalInformation;
        }

        [JsonProperty("additional-information")]
        public string NativeDesktopAdditionalInformation { get; }
    }
}
