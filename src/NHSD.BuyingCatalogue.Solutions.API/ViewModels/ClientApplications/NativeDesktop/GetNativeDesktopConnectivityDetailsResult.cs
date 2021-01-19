using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop
{
    public sealed class GetNativeDesktopConnectivityDetailsResult
    {
        public GetNativeDesktopConnectivityDetailsResult(string nativeDesktopMinimumConnectionSpeed)
        {
            NativeDesktopMinimumConnectionSpeed = nativeDesktopMinimumConnectionSpeed;
        }

        [JsonProperty("minimum-connection-speed")]
        public string NativeDesktopMinimumConnectionSpeed { get; }
    }
}
