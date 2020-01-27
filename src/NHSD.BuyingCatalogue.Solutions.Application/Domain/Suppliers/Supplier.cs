using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers
{
    internal sealed class Supplier
    {
        public string Name { get; set; }

        public string Summary { get; set; }

        public string Url { get; set; }

        internal Supplier(ISupplierResult supplierResult)
        {
            Name = supplierResult.Name;
            Summary = supplierResult.Summary;
            Url = supplierResult.Url;
        }

        public Supplier()
        {
        }
    }
}
