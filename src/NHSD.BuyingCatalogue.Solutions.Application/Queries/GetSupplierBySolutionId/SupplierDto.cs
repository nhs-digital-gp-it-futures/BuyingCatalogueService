using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSupplierBySolutionId
{
    internal sealed class SupplierDto : ISupplier
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
    }
}
