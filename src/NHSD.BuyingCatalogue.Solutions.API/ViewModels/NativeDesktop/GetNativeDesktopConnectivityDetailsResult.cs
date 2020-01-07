using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop
{
    public sealed class GetNativeDesktopConnectivityDetailsResult
    {
        [JsonProperty("minimum-connection-speed")]
        public string NativeDesktopMinimumConnectionSpeed { get; set; }
    }
}
