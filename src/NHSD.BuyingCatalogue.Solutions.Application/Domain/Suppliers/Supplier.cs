using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers
{
    internal sealed class Supplier
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        internal Supplier(ISupplierResult supplierResult)
        {
            Name = supplierResult.Name;
            Description = supplierResult.Description;
            Link = supplierResult.Link;
        }

        public Supplier()
        {
        }
    }
}
