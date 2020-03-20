using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile
{
    public sealed class UpdateNativeMobileAdditionalInformationViewModel
    {
        [JsonProperty("additional-information")]
        public string NativeMobileAdditionalInformation { get; set; }
    }
}
