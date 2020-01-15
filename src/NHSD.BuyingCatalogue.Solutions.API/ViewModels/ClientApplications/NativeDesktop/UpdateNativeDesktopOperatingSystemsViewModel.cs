using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop
{
    public sealed class UpdateNativeDesktopOperatingSystemsViewModel
    {
        [JsonProperty("operating-systems-description")]
        public string NativeDesktopOperatingSystemsDescription { get; set; }
    }
}
