using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    internal static class ClientApplicationNativeDesktopExtensions
    {
        public static bool IsNativeDesktopThirdPartyComplete(this IClientApplication clientApplication) =>
            !string.IsNullOrWhiteSpace(clientApplication?.NativeDesktopThirdParty?.ThirdPartyComponents) ||
            !string.IsNullOrWhiteSpace(clientApplication?.NativeDesktopThirdParty?.DeviceCapabilities);
    }
}
