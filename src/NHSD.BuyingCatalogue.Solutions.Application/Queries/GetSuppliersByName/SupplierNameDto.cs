using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSuppliersByName
{
    internal sealed class SupplierNameDto : ISupplierName
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
