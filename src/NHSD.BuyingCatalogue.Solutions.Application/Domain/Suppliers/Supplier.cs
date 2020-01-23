using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers
{
    internal sealed class Supplier
    {
        public string Description { get; set; }

        public string Link { get; set; }

        internal Supplier(ISupplierResult supplierResult)
        {
            Description = supplierResult.Description;
            Link = supplierResult.Link;
        }

        public Supplier()
        {
        }
    }
}
