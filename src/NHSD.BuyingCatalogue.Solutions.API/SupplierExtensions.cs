using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.API
{
    internal static class SupplierExtensions
    {
        public static bool IsSupplierComplete(this ISolutionSupplier solutionSupplier) =>
            !string.IsNullOrWhiteSpace(solutionSupplier?.Summary) ||
            !string.IsNullOrWhiteSpace(solutionSupplier?.Url);
    }
}
