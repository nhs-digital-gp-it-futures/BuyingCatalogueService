using System.Linq;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API
{
    internal static class ClientApplicationNativeMobileExtensions
    {
        public static bool IsMobileConnectionDetailsComplete(this IClientApplication clientApplication) =>
            clientApplication?.MobileConnectionDetails?.ConnectionType?.Any() == true ||
            !string.IsNullOrEmpty(clientApplication?.MobileConnectionDetails?.MinimumConnectionSpeed) ||
            !string.IsNullOrEmpty(clientApplication?.MobileConnectionDetails?.Description);

        public static bool IsNativeMobileOperatingSystemsComplete(this IClientApplication clientApplication) =>
            clientApplication?.MobileOperatingSystems?.OperatingSystems?.Any() == true;

        public static bool IsNativeMobileFirstComplete(this IClientApplication clientApplication) =>
            clientApplication?.NativeMobileFirstDesign.HasValue == true;

        public static bool IsNativeMobileComplete(this IClientApplication clientApplication) =>
            clientApplication.IsNativeMobileOperatingSystemsComplete() &&
            clientApplication.IsNativeMobileFirstComplete() &&
            clientApplication.IsNativeMobileMemoryAndStorageComplete();

        public static bool IsNativeMobileMemoryAndStorageComplete(this IClientApplication clientApplication) =>
            !string.IsNullOrWhiteSpace(clientApplication?.MobileMemoryAndStorage?.MinimumMemoryRequirement) &&
            !string.IsNullOrWhiteSpace(clientApplication.MobileMemoryAndStorage?.Description);

        public static bool IsMobileThirdPartyComplete(this IClientApplication clientApplication) =>
            !string.IsNullOrWhiteSpace(clientApplication?.MobileThirdParty?.ThirdPartyComponents) ||
            !string.IsNullOrWhiteSpace(clientApplication?.MobileThirdParty?.DeviceCapabilities);

        public static bool IsNativeMobileAdditionalInformationComplete(this IClientApplication clientApplication) =>
            !string.IsNullOrWhiteSpace(clientApplication?.NativeMobileAdditionalInformation);
    }
}
