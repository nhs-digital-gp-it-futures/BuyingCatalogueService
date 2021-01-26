using System;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class SupplierContactEntity : EntityBase
    {
        public Guid Id { get; set; }

        public string SupplierId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        protected override string InsertSql => @"
            INSERT INTO dbo.SupplierContact
            (
                Id,
                SupplierId,
                FirstName,
                LastName,
                Email,
                PhoneNumber,
                LastUpdated,
                LastUpdatedBy
            )
            VALUES
            (
                @Id,
                @SupplierId,
                @FirstName,
                @LastName,
                @Email,
                @PhoneNumber,
                @LastUpdated,
                @LastUpdatedBy
            );";
    }
}
