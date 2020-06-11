using System;
using System.Collections.Generic;
using System.Text;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{
    public sealed class CatalogueItemEntity : EntityBase
    {
        public string CatalogueItemId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public int CatalogueItemTypeId { get; set; } 
        public string SupplierId { get; set; }
        public int PublishedStatusId { get; set; }

        protected override string InsertSql => $@"
        INSERT INTO dbo.CatalogueItem
        (
            CatalogueItemId,
            Name,
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
    }
}
