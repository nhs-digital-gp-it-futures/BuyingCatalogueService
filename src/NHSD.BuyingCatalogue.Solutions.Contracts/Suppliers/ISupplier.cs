using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers
{
    public interface ISupplier
    {
        string Id { get; }

        string Name { get; }

        ISupplierAddress Address { get; }

        IContact PrimaryContact { get; }
    }
}
