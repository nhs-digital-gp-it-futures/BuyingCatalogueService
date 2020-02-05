using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API
{
    internal static class ImplementationTimescalesExtensions
    {
        public static bool IsImplementationTimescalesComplete(this ISolution solution) =>
            !string.IsNullOrWhiteSpace(solution.ImplementationTimescales?.Description);
    }
}
