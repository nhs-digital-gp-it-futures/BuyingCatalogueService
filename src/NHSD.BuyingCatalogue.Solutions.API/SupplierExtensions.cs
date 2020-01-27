using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.API
{
    internal static class SupplierExtensions
    {
        public static bool IsSupplierComplete(this ISupplier supplier) =>
            !string.IsNullOrWhiteSpace(supplier?.Summary) ||
            !string.IsNullOrWhiteSpace((supplier?.Url));
    }
}
