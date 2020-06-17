using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class CataloguePriceEntityBuilder
    {
        private readonly CataloguePriceEntity _cataloguePriceEntity;

        private CataloguePriceEntityBuilder()
        {
            _cataloguePriceEntity = new CataloguePriceEntity
            {
                ProvisioningTypeId = 1,
                CataloguePriceTypeId = 1,
                PricingUnitId = Guid.Parse("F8D06518-1A20-4FBA-B369-AB583F9FA8C0"),
                LastUpdated = DateTime.UtcNow
            };
        }

        public static CataloguePriceEntityBuilder Create() => new CataloguePriceEntityBuilder();

        public CataloguePriceEntityBuilder WithCatalogueItemId(string itemId)
        {
            _cataloguePriceEntity.CatalogueItemId = itemId;
            return this;
        }

        public CataloguePriceEntityBuilder WithCurrencyCode(string code)
        {
            _cataloguePriceEntity.CurrencyCode = code;
            return this;
        }

        public CataloguePriceEntity Build()
        {
            return _cataloguePriceEntity;
        }
    }
}
