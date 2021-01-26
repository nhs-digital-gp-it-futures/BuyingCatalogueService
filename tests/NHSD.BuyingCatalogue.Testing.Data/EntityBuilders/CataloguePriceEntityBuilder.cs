using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class CataloguePriceEntityBuilder
    {
        private readonly CataloguePriceEntity cataloguePriceEntity;

        private CataloguePriceEntityBuilder()
        {
            cataloguePriceEntity = new CataloguePriceEntity
            {
                ProvisioningTypeId = 1,
                CataloguePriceTypeId = 1,
                CurrencyCode = "GBP",
                PricingUnitId = Guid.Parse("F8D06518-1A20-4FBA-B369-AB583F9FA8C0"),
                LastUpdated = DateTime.UtcNow,
            };
        }

        public static CataloguePriceEntityBuilder Create() => new();

        public CataloguePriceEntityBuilder WithCatalogueItemId(string itemId)
        {
            cataloguePriceEntity.CatalogueItemId = itemId;
            return this;
        }

        public CataloguePriceEntityBuilder WithCurrencyCode(string code)
        {
            cataloguePriceEntity.CurrencyCode = code;
            return this;
        }

        public CataloguePriceEntityBuilder WithPrice(decimal? price)
        {
            cataloguePriceEntity.Price = price;
            return this;
        }

        public CataloguePriceEntityBuilder WithPricingUnitId(Guid pricingUnitId)
        {
            cataloguePriceEntity.PricingUnitId = pricingUnitId;
            return this;
        }

        public CataloguePriceEntityBuilder WithTimeUnit(int? timeUnitId)
        {
            cataloguePriceEntity.TimeUnitId = timeUnitId;
            return this;
        }

        public CataloguePriceEntityBuilder WithPriceTypeId(int typeId)
        {
            cataloguePriceEntity.CataloguePriceTypeId = typeId;
            return this;
        }

        public CataloguePriceEntityBuilder WithProvisioningTypeId(int typeId)
        {
            cataloguePriceEntity.ProvisioningTypeId = typeId;
            return this;
        }

        public CataloguePriceEntity Build()
        {
            return cataloguePriceEntity;
        }
    }
}
