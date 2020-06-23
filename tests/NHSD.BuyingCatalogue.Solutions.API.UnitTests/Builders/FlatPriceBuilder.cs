using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders
{
    internal sealed class FlatPriceBuilder
    {
        private readonly ICataloguePrice _cataloguePrice;

        private FlatPriceBuilder()
        {
            _cataloguePrice = new FlatCataloguePriceDto
            {
                CataloguePriceId = 1,
                Type = "Flat",
                CatalogueItemName = "Item Name",
                CatalogueItemId = "Item Id",
                PricingUnit = new PricingUnitDto
                {
                    Name = "name",
                    Description = "Desc",
                    TierName = "tierName"
                },
                CurrencyCode = "GBP",
                Price = new decimal(474.32)
            };
        }

        internal static FlatPriceBuilder Create() => new FlatPriceBuilder();

        internal ICataloguePrice Build() => _cataloguePrice;
    }
}
