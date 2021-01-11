using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders
{
    internal sealed class FlatCataloguePriceDtoBuilder
    {
        private readonly int _cataloguePriceId;
        private readonly string _type;
        private readonly string _catalogueItemName;
        private readonly string _catalogueItemId;
        private readonly string _currencyCode;
        private readonly decimal _price;

        private FlatCataloguePriceDtoBuilder()
        {
            _cataloguePriceId = 1;
            _type = "Flat";
            _catalogueItemName = "Item Name";
            _catalogueItemId = "Item Id";
            _currencyCode = "GBP";
            _price = 474.32m;
        }

        internal static FlatCataloguePriceDtoBuilder Create() => new();

        internal ICataloguePrice Build()
        {
            return new FlatCataloguePriceDto
            {
                CataloguePriceId = _cataloguePriceId,
                Type = _type,
                CatalogueItemName = _catalogueItemName,
                CatalogueItemId = _catalogueItemId,
                CurrencyCode = _currencyCode,
                Price = _price,
                PricingUnit = PriceUnitDtoBuilder.Create().Build()
            };
        }
    }
}
