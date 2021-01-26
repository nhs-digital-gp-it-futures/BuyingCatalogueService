using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class CatalogueItemEntityBuilder
    {
        private readonly DateTime created = DateTime.UtcNow;

        private string catalogueItemId = " CatalogueId";
        private int catalogueItemTypeId = 1;
        private string name = "SomeName";
        private int publishedStatusId = 1;
        private string supplierId;

        public static CatalogueItemEntityBuilder Create()
        {
            return new();
        }

        public CatalogueItemEntityBuilder WithCatalogueItemId(string id)
        {
            catalogueItemId = id;
            return this;
        }

        public CatalogueItemEntityBuilder WithCatalogueItemTypeId(int id)
        {
            catalogueItemTypeId = id;
            return this;
        }

        public CatalogueItemEntityBuilder WithName(string itemName)
        {
            name = itemName;
            return this;
        }

        public CatalogueItemEntityBuilder WithPublishedStatusId(int id)
        {
            publishedStatusId = id;
            return this;
        }

        public CatalogueItemEntityBuilder WithSupplierId(string id)
        {
            supplierId = id;
            return this;
        }

        public CatalogueItemEntity Build()
        {
            return new()
            {
                CatalogueItemId = catalogueItemId,
                CatalogueItemTypeId = catalogueItemTypeId,
                Created = created,
                PublishedStatusId = publishedStatusId,
                Name = name,
                SupplierId = supplierId,
            };
        }
    }
}
