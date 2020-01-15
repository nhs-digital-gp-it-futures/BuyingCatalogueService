using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop
{
    public sealed class GetNativeDesktopConnectivityDetailsResult
    {
        [JsonProperty("minimum-connection-speed")]
        public string NativeDesktopMinimumConnectionSpeed { get; private set; }

        public GetNativeDesktopConnectivityDetailsResult(string nativeDesktopMinimumConnectionSpeed)
        {
            NativeDesktopMinimumConnectionSpeed = nativeDesktopMinimumConnectionSpeed;
        }
    }
}
