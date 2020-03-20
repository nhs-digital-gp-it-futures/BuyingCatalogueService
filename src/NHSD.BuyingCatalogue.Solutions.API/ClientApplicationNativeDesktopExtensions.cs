using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API
{
    internal static class ClientApplicationNativeDesktopExtensions
    {
        public static bool IsNativeDesktopOperatingSystemsComplete(this IClientApplication clientApplication) =>
            !string.IsNullOrWhiteSpace(clientApplication?.NativeDesktopOperatingSystemsDescription);

        public static bool IsNativeDesktopConnectionDetailsComplete(this IClientApplication clientApplication) =>
            !string.IsNullOrWhiteSpace(clientApplication?.NativeDesktopMinimumConnectionSpeed);

        public static bool IsNativeDesktopMemoryAndStorageComplete(this IClientApplication clientApplication) =>
            !string.IsNullOrWhiteSpace(clientApplication?.NativeDesktopMemoryAndStorage?.MinimumMemoryRequirement) &&
            !string.IsNullOrWhiteSpace(clientApplication?.NativeDesktopMemoryAndStorage
                ?.StorageRequirementsDescription) &&
            !string.IsNullOrWhiteSpace(clientApplication?.NativeDesktopMemoryAndStorage?.MinimumCpu);
        
        public static bool IsNativeDesktopThirdPartyComplete(this IClientApplication clientApplication) =>
            !string.IsNullOrWhiteSpace(clientApplication?.NativeDesktopThirdParty?.ThirdPartyComponents) ||
            !string.IsNullOrWhiteSpace(clientApplication?.NativeDesktopThirdParty?.DeviceCapabilities);

        public static bool IsNativeDesktopComplete(this IClientApplication clientApplication) =>
            IsNativeDesktopOperatingSystemsComplete(clientApplication) &&
            IsNativeDesktopConnectionDetailsComplete(clientApplication) &&
            IsNativeDesktopMemoryAndStorageComplete(clientApplication);
    }
}