using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop
{
    public sealed class UpdateNativeDesktopAdditionalInformationViewModel
    {
        [JsonProperty("additional-information")]
        public string NativeDesktopAdditionalInformation { get; set; }
    }
}
