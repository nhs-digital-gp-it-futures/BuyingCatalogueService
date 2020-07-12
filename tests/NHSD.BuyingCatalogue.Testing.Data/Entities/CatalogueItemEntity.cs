using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class CatalogueItemEntity : EntityBase
    {
        public string CatalogueItemId { get; set; }

        public int CatalogueItemTypeId { get; set; }

        public DateTime Created { get; set; }

        public string Name { get; set; }

        public int PublishedStatusId { get; set; }

        public string SupplierId { get; set; }

        protected override string InsertSql => @"
        INSERT INTO dbo.CatalogueItem
        (
            CatalogueItemId,
            [Name],
            Created,
            CatalogueItemTypeId,
            SupplierId,
            PublishedStatusId
        )
        VALUES
        (
            @CatalogueItemId,
            @Name,
            @Created,
            @CatalogueItemTypeId,
            @SupplierId,
            @PublishedStatusId
        );";

        public static async Task<IEnumerable<CatalogueItemEntity>> FetchAllAsync()
        {
            return await SqlRunner.FetchAllAsync<CatalogueItemEntity>($@"
            SELECT  CatalogueItemId,
                    [Name],
                    Created,
                    CatalogueItemTypeId,
                    SupplierId,
                    PublishedStatusId
            FROM    CatalogueItem");
        }

        public static async Task<CatalogueItemEntity> GetByIdAsync(string catalogueItemId)
        {
            return (await FetchAllAsync()).First(item => 
                string.Equals(item.CatalogueItemId, catalogueItemId, StringComparison.Ordinal));
        }
    }
}
