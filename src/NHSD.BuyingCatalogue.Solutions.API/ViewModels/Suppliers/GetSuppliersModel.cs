using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers
{
    public sealed class GetSuppliersModel
    {
        internal GetSuppliersModel(ISupplier supplier)
        {
            SupplierId = supplier.Id;
            Name = supplier.Name;
        }

        public string SupplierId { get; }

        public string Name { get; }
    }
}
