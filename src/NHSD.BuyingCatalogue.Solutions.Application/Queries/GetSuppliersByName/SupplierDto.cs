using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSuppliersByName
{
    internal sealed class SupplierDto : ISupplier
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ISupplierAddress Address { get; set; }

        public IContact PrimaryContact { get; set; }
    }
}
