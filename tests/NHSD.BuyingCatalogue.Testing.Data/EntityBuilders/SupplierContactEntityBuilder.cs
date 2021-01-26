using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class SupplierContactEntityBuilder
    {
        private readonly SupplierContactEntity entity;

        public SupplierContactEntityBuilder()
        {
            entity = new SupplierContactEntity
            {
                Id = Guid.NewGuid(),
                SupplierId = "Sup 1",
                FirstName = "Bob",
                LastName = "Smith",
                Email = "bobsmith@email.com",
            };
        }

        public static SupplierContactEntityBuilder Create()
        {
            return new();
        }

        public SupplierContactEntityBuilder WithId(Guid id)
        {
            entity.Id = id;
            return this;
        }

        public SupplierContactEntityBuilder WithSupplierId(string supplierId)
        {
            entity.SupplierId = supplierId;
            return this;
        }

        public SupplierContactEntityBuilder WithFirstName(string firstName)
        {
            entity.FirstName = firstName;
            return this;
        }

        public SupplierContactEntityBuilder WithLastName(string lastName)
        {
            entity.LastName = lastName;
            return this;
        }

        public SupplierContactEntityBuilder WithEmail(string email)
        {
            entity.Email = email;
            return this;
        }

        public SupplierContactEntityBuilder WithPhoneNumber(string phoneNumber)
        {
            entity.PhoneNumber = phoneNumber;
            return this;
        }

        public SupplierContactEntity Build()
        {
            return entity;
        }
    }
}
