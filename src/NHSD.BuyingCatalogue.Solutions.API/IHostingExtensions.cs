using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API
{
    internal static class HostingExtensions
    {
        public static bool IsPublicCloudComplete(this IHosting hosting) =>
            !string.IsNullOrWhiteSpace(hosting?.PublicCloud?.Summary) ||
            !string.IsNullOrWhiteSpace(hosting?.PublicCloud?.URL) ||
            !string.IsNullOrWhiteSpace(hosting?.PublicCloud?.ConnectivityRequired);
    }
}
