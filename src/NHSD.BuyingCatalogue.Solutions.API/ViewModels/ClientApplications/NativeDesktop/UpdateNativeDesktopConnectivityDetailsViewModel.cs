using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop
{
    public sealed class UpdateNativeDesktopConnectivityDetailsViewModel
    {
        [JsonProperty("minimum-connection-speed")]
        public string NativeDesktopMinimumConnectionSpeed { get; set; }
    }
}
