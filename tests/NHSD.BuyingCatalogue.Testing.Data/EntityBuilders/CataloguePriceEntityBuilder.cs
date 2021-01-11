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
                CurrencyCode = "GBP",
                PricingUnitId = Guid.Parse("F8D06518-1A20-4FBA-B369-AB583F9FA8C0"),
                LastUpdated = DateTime.UtcNow
            };
        }

        public static CataloguePriceEntityBuilder Create() => new();

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

        public CataloguePriceEntityBuilder WithPrice(decimal? price)
        {
            _cataloguePriceEntity.Price = price;
            return this;
        }

        public CataloguePriceEntityBuilder WithPricingUnitId(Guid pricingUnitId)
        {
            _cataloguePriceEntity.PricingUnitId = pricingUnitId;
            return this;
        }

        public CataloguePriceEntityBuilder WithTimeUnit(int? timeUnitId)
        {
            _cataloguePriceEntity.TimeUnitId = timeUnitId;
            return this;
        }

        public CataloguePriceEntityBuilder WithPriceTypeId(int typeId)
        {
            _cataloguePriceEntity.CataloguePriceTypeId = typeId;
            return this;
        }

        public CataloguePriceEntityBuilder WithProvisioningTypeId(int typeId)
        {
            _cataloguePriceEntity.ProvisioningTypeId = typeId;
            return this;
        }

        public CataloguePriceEntity Build()
        {
            return _cataloguePriceEntity;
        }
    }
}
