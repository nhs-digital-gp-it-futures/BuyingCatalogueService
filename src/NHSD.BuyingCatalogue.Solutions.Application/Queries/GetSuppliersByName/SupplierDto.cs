using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSuppliersByName
{
    internal sealed class SupplierDto : ISupplier
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
