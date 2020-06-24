using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders
{
    internal sealed class TieredCataloguePriceDtoBuilder
    {
        private readonly int _cataloguePriceId;
        private readonly string _type;
        private readonly string _catalogueItemName;
        private readonly string _catalogueItemId;
        private readonly string _currencyCode;
        private readonly List<ITieredPrice> _tieredPrices;

        private TieredCataloguePriceDtoBuilder()
        {
            _cataloguePriceId = 1;
            _type = "Tiered";
            _catalogueItemName = "Item Name";
            _catalogueItemId = "Item Id";
            _currencyCode = "USD";
            _tieredPrices = new List<ITieredPrice>
            {
                new TieredPriceDto { BandStart = 1, BandEnd = 5, Price = 753.78m },
                new TieredPriceDto { BandStart = 6, Price = 546.32m }
            };
        }

        internal static TieredCataloguePriceDtoBuilder Create() => new TieredCataloguePriceDtoBuilder();

        internal ICataloguePrice Build()
        {
            return new TieredCataloguePriceDto
            {
                CataloguePriceId = _cataloguePriceId,
                Type = _type,
                CatalogueItemName = _catalogueItemName,
                CatalogueItemId = _catalogueItemId,
                CurrencyCode = _currencyCode,
                TieredPrices = _tieredPrices,
                PricingUnit = PriceUnitDtoBuilder.Create().Build()
            };
        }
    }
}
