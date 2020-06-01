using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers
{
    internal sealed class Supplier
    {
        internal Supplier(ISupplierResult supplierResult)
        {
            Id = supplierResult.Id;
            Name = supplierResult.Name;
            Address = CreateSupplierAddress(supplierResult);
            PrimaryContact = CreateContact(supplierResult);
        }

        public string Id { get; }

        public string Name { get; }

        public ISupplierAddress Address { get; }

        public Contact PrimaryContact { get; }

        private static ISupplierAddress CreateSupplierAddress(ISupplierResult supplierResult)
        {
            if (!supplierResult.HasAddress)
                return null;

            return new SupplierAddress
            {
                Line1 = supplierResult.AddressLine1,
                Line2 = supplierResult.AddressLine2,
                Line3 = supplierResult.AddressLine3,
                Line4 = supplierResult.AddressLine4,
                Line5 = supplierResult.AddressLine5,
                Town = supplierResult.Town,
                Postcode = supplierResult.Postcode,
                County = supplierResult.County,
                Country = supplierResult.Country,
            };
        }

        private static Contact CreateContact(ISupplierResult supplierResult)
        {
            return supplierResult.HasContact ? new Contact(supplierResult) : null;
        }
    }
}
