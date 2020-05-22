using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSupplierBySolutionId
{
    internal sealed class SolutionSupplierDto : ISolutionSupplier
    {
        public string Name { get; set; }

        public string Summary { get; set; }

        public string Url { get; set; }
    }
}
