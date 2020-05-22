using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers
{
    internal sealed class Supplier
    {
        internal Supplier(ISupplierResult name)
        {
            Id = name.Id;
            Name = name.Name;
        }

        public string Id { get; }

        public string Name { get; }
    }
}
