using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class SupplierContactEntityBuilder
    {
        private readonly SupplierContactEntity _entity;

        public static SupplierContactEntityBuilder Create()
        {
            return new();
        }

        public SupplierContactEntityBuilder()
        {
            _entity = new SupplierContactEntity
            {
                Id = Guid.NewGuid(),
                SupplierId = "Sup 1",
                FirstName = "Bob",
                LastName = "Smith",
                Email = "bobsmith@email.com"
            };
        }

        public SupplierContactEntityBuilder WithId(Guid id)
        {
            _entity.Id = id;
            return this;
        }

        public SupplierContactEntityBuilder WithSupplierId(string supplierId)
        {
            _entity.SupplierId = supplierId;
            return this;
        }

        public SupplierContactEntityBuilder WithFirstName(string firstName)
        {
            _entity.FirstName = firstName;
            return this;
        }

        public SupplierContactEntityBuilder WithLastName(string lastName)
        {
            _entity.LastName = lastName;
            return this;
        }

        public SupplierContactEntityBuilder WithEmail(string email)
        {
            _entity.Email = email;
            return this;
        }

        public SupplierContactEntityBuilder WithPhoneNumber(string phoneNumber)
        {
            _entity.PhoneNumber = phoneNumber;
            return this;
        }

        public SupplierContactEntityBuilder WithLastUpdated(DateTime time)
        {
            _entity.LastUpdated = time;
            return this;
        }

        public SupplierContactEntityBuilder WithLastUpdatedBy(Guid id)
        {
            _entity.LastUpdatedBy = id;
            return this;
        }

        public SupplierContactEntity Build()
        {
            return _entity;
        }
    }
}
