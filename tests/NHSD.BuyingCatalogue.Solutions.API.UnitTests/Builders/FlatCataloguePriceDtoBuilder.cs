using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders
{
    internal sealed class FlatCataloguePriceDtoBuilder
    {
        private readonly int cataloguePriceId;
        private readonly string type;
        private readonly string catalogueItemName;
        private readonly string catalogueItemId;
        private readonly string currencyCode;
        private readonly decimal price;

        private FlatCataloguePriceDtoBuilder()
        {
            cataloguePriceId = 1;
            type = "Flat";
            catalogueItemName = "Item Name";
            catalogueItemId = "Item Id";
            currencyCode = "GBP";
            price = 474.32m;
        }

        internal static FlatCataloguePriceDtoBuilder Create() => new();

        internal ICataloguePrice Build()
        {
            return new FlatCataloguePriceDto
            {
                CataloguePriceId = cataloguePriceId,
                Type = type,
                CatalogueItemName = catalogueItemName,
                CatalogueItemId = catalogueItemId,
                CurrencyCode = currencyCode,
                Price = price,
                PricingUnit = PriceUnitDtoBuilder.Create().Build(),
            };
        }
    }
}
