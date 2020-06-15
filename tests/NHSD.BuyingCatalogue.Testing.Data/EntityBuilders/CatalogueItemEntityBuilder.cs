using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class CatalogueItemEntityBuilder
    {
        private string _catalogueItemId = " CatalogueId";
        private int _catalogueItemTypeId = 1;
        private DateTime _created = DateTime.UtcNow;
        private string _name = "SomeName";
        private int _publishedStatusId = 1;
        private string _supplierId;

        public static CatalogueItemEntityBuilder Create()
        {
            return new CatalogueItemEntityBuilder();
        }

        public CatalogueItemEntityBuilder WithCatalogueItemId(string catalogueItemId)
        {
            _catalogueItemId = catalogueItemId;
            return this;
        }

        public CatalogueItemEntityBuilder WithCatalogueItemTypeId(int catalogueItemTypeId)
        {
            _catalogueItemTypeId = catalogueItemTypeId;
            return this;
        }

        public CatalogueItemEntityBuilder WithCreated(DateTime created)
        {
            _created = created;
            return this;
        }

        public CatalogueItemEntityBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CatalogueItemEntityBuilder WithPublishedStatusId(int publishedStatusId)
        {
            _publishedStatusId = publishedStatusId;
            return this;
        }

        public CatalogueItemEntityBuilder WithSupplierId(string supplierId)
        {
            _supplierId = supplierId;
            return this;
        }

        public CatalogueItemEntity Build()
        {
            return new CatalogueItemEntity
            {
                CatalogueItemId = _catalogueItemId,
                CatalogueItemTypeId = _catalogueItemTypeId,
                Created = _created,
                PublishedStatusId = _publishedStatusId,
                Name = _name,
                SupplierId = _supplierId
            };
        }
    }
}
