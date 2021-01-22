using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders
{
    internal sealed class TieredCataloguePriceDtoBuilder
    {
        private readonly int cataloguePriceId;
        private readonly string type;
        private readonly string catalogueItemName;
        private readonly string catalogueItemId;
        private readonly string currencyCode;
        private readonly List<ITieredPrice> tieredPrices;

        private TieredCataloguePriceDtoBuilder()
        {
            cataloguePriceId = 1;
            type = "Tiered";
            catalogueItemName = "Item Name";
            catalogueItemId = "Item Id";
            currencyCode = "USD";
            tieredPrices = new List<ITieredPrice>
            {
                new TieredPriceDto { BandStart = 1, BandEnd = 5, Price = 753.78m },
                new TieredPriceDto { BandStart = 6, Price = 546.32m },
            };
        }

        internal static TieredCataloguePriceDtoBuilder Create() => new();

        internal ICataloguePrice Build()
        {
            return new TieredCataloguePriceDto
            {
                CataloguePriceId = cataloguePriceId,
                Type = type,
                CatalogueItemName = catalogueItemName,
                CatalogueItemId = catalogueItemId,
                CurrencyCode = currencyCode,
                TieredPrices = tieredPrices,
                PricingUnit = PriceUnitDtoBuilder.Create().Build(),
            };
        }
    }
}
