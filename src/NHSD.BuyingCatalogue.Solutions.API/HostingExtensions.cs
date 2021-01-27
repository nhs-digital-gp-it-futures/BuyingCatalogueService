using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API
{
    internal static class HostingExtensions
    {
        public static bool IsPublicCloudComplete(this IHosting hosting) =>
            !string.IsNullOrWhiteSpace(hosting?.PublicCloud?.Summary) ||
            !string.IsNullOrWhiteSpace(hosting?.PublicCloud?.Link) ||
            !string.IsNullOrWhiteSpace(hosting?.PublicCloud?.RequiresHscn);

        public static bool IsPrivateCloudComplete(this IHosting hosting) =>
            !string.IsNullOrWhiteSpace(hosting?.PrivateCloud?.Summary) ||
            !string.IsNullOrWhiteSpace(hosting?.PrivateCloud?.Link) ||
            !string.IsNullOrWhiteSpace(hosting?.PrivateCloud?.HostingModel) ||
            !string.IsNullOrWhiteSpace(hosting?.PrivateCloud?.RequiresHscn);

        public static bool IsOnPremiseComplete(this IHosting hosting) =>
            !string.IsNullOrWhiteSpace(hosting?.OnPremise?.Summary) ||
            !string.IsNullOrWhiteSpace(hosting?.OnPremise?.Link) ||
            !string.IsNullOrWhiteSpace(hosting?.OnPremise?.HostingModel) ||
            !string.IsNullOrWhiteSpace(hosting?.OnPremise?.RequiresHscn);

        public static bool IsHybridHostingTypeComplete(this IHosting hosting) =>
            !string.IsNullOrWhiteSpace(hosting?.HybridHostingType?.Summary) ||
            !string.IsNullOrWhiteSpace(hosting?.HybridHostingType?.Link) ||
            !string.IsNullOrWhiteSpace(hosting?.HybridHostingType?.HostingModel) ||
            !string.IsNullOrWhiteSpace(hosting?.HybridHostingType?.RequiresHscn);
    }
}
