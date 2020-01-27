using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    internal sealed class SupplierResult : ISupplierResult
    {
        public string SolutionId { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public string Url { get; set; }
    }
}
