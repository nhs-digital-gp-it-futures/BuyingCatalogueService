using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        protected override string InsertSql => $@"
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

        public static async Task<IEnumerable<SupplierContactEntity>> FetchAllAsync()
        {
            return await SqlRunner.FetchAllAsync<SupplierContactEntity>(@"
                SELECT  Id,
                        SupplierId,
                        FirstName,
                        LastName,
                        Email,
                        PhoneNumber,
	                    LastUpdated,
	                    LastUpdatedBy
                FROM    Supplier;");
        }

        public static async Task<SupplierContactEntity> GetByIdAsync(Guid supplierContactId)
        {
            return (await FetchAllAsync()).First(item => supplierContactId == item.Id);
        }
    }
}
