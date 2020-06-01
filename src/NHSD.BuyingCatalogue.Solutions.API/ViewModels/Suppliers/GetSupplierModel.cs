using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers
{
    public sealed class GetSupplierModel
    {
        internal GetSupplierModel(ISupplier supplier)
        {
            SupplierId = supplier.Id;
            Name = supplier.Name;
            Address = CreateAddressModel(supplier.Address);
            PrimaryContact = CreatePrimaryContactModel(supplier.PrimaryContact);
        }

        public string SupplierId { get; set; }

        public string Name { get; set; }

        public AddressModel Address { get; set; }

        public PrimaryContactModel PrimaryContact { get; set; }

        private static AddressModel CreateAddressModel(ISupplierAddress address)
        {
            if (address is null)
                return null;

            return new AddressModel
            {
                Line1 = address.Line1,
                Line2 = address.Line2,
                Line3 = address.Line3,
                Line4 = address.Line4,
                Line5 = address.Line5,
                Town = address.Town,
                Postcode = address.Postcode,
                County = address.County,
                Country = address.Country,
            };
        }

        private static PrimaryContactModel CreatePrimaryContactModel(IContact contact)
        {
            if (contact is null)
                return null;

            return new PrimaryContactModel
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                EmailAddress = contact.Email,
                TelephoneNumber = contact.PhoneNumber,
            };
        }
    }
}
