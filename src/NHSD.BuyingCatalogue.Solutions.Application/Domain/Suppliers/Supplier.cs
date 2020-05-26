using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers
{
    internal sealed class Supplier
    {
        internal Supplier(ISupplierResult supplierResult)
        {
            Id = supplierResult.Id;
            Name = supplierResult.Name;
        }

        public string Id { get; }

        public string Name { get; }
    }
}
