using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class SupplierEntity : EntityBase
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string LegalName { get; set; }

        public string Summary { get; set; }

        public string SupplierUrl { get; set; }

        public string Address { get; set; }

        public bool Deleted { get; set; }

        protected override string InsertSql => @"
            INSERT INTO dbo.Supplier
            (
                Id,
                [Name],
                LegalName,
                Summary,
                SupplierUrl,
                [Address],
                Deleted,
                LastUpdated,
                LastUpdatedBy
            )
            VALUES
            (
                @Id,
                @Name,
                @LegalName,
                @Summary,
                @SupplierUrl,
                @Address,
                @Deleted,
                @LastUpdated,
                @LastUpdatedBy
            );";

        public static async Task<IEnumerable<SupplierEntity>> FetchAllAsync()
        {
            const string selectSql = @"
                SELECT Id,
                       Summary,
                       SupplierUrl,
                       LastUpdated
                  FROM dbo.Supplier;";

            return await SqlRunner.FetchAllAsync<SupplierEntity>(selectSql);
        }

        public static async Task<SupplierEntity> GetByIdAsync(string supplierId)
        {
            return (await FetchAllAsync()).First(item => supplierId == item.Id);
        }
    }
}
