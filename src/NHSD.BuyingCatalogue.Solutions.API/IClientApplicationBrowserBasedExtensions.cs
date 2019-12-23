using System.Linq;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API
{
    internal static class IClientApplicationBrowserBasedExtensions
    {
        public static bool IsBrowserSupportedComplete(this IClientApplication clientApplication) =>
            clientApplication?.BrowsersSupported?.Any() == true && clientApplication?.MobileResponsive.HasValue == true;

        public static bool IsMobileFirstComplete(this IClientApplication clientApplication) =>
            clientApplication?.MobileFirstDesign.HasValue == true;

        public static bool IsPluginsComplete(this IClientApplication clientApplication) =>
            clientApplication?.Plugins?.Required.HasValue == true;

        public static bool IsHardwareRequirementComplete(this IClientApplication clientApplication) =>
            !string.IsNullOrWhiteSpace(clientApplication?.HardwareRequirements);

        public static bool IsAdditionalInformationComplete(this IClientApplication clientApplication) =>
            !string.IsNullOrWhiteSpace(clientApplication?.AdditionalInformation);

        public static bool IsConnectivityAndResolutionComplete(this IClientApplication clientApplication) =>
            !string.IsNullOrWhiteSpace(clientApplication?.MinimumConnectionSpeed);

        public static bool IsBrowserBasedComplete(this IClientApplication clientApplication) =>
            clientApplication.IsBrowserSupportedComplete() &&
            clientApplication.IsMobileFirstComplete() &&
            clientApplication.IsPluginsComplete() &&
            clientApplication.IsConnectivityAndResolutionComplete();
    }
}
