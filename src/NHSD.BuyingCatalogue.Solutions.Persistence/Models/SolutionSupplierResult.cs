using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    internal sealed class SolutionSupplierResult : ISolutionSupplierResult
    {
        public string SolutionId { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public string Url { get; set; }
    }
}
