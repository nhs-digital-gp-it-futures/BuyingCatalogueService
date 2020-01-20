using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API
{
    internal static class HostingExtensions
    {
        public static bool IsPublicCloudComplete(this IHosting hosting) =>
            !string.IsNullOrWhiteSpace(hosting?.PublicCloud?.Summary) ||
            !string.IsNullOrWhiteSpace(hosting?.PublicCloud?.URL) ||
            !string.IsNullOrWhiteSpace(hosting?.PublicCloud?.ConnectivityRequired);

        public static bool IsPrivateCloudComplete(this IHosting hosting) =>
            !string.IsNullOrWhiteSpace(hosting?.PrivateCloud?.Summary) ||
            !string.IsNullOrWhiteSpace(hosting?.PrivateCloud?.Link) ||
            !string.IsNullOrWhiteSpace(hosting?.PrivateCloud?.HostingModel) ||
            !string.IsNullOrWhiteSpace(hosting?.PrivateCloud?.RequiresHSCN);
    }
}
