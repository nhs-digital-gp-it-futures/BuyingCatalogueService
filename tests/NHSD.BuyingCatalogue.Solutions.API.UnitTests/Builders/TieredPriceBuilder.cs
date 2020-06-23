using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders
{
    internal sealed class TieredPriceBuilder
    {
        private readonly ICataloguePrice _cataloguePrice;

        private TieredPriceBuilder()
        {
            _cataloguePrice = new TieredCataloguePriceDto
            {
                CataloguePriceId = 1,
                Type = "Tiered",
                CatalogueItemName = "Item Name",
                CatalogueItemId = "Item Id",
                PricingUnit = new PricingUnitDto
                {
                    Name = "name",
                    Description = "Desc",
                    TierName = "tierName"
                },
                CurrencyCode = "GBP",
                TieredPrices = new List<ITieredPrice> { 
                    new TieredPriceDto
                {
                    BandStart = 1,
                    BandEnd = 5,
                    Price = new decimal(753.78)
                },
                    new TieredPriceDto
                    {
                        BandStart = 6,
                        Price = new decimal(546.32)
                    }
                }
            };
        }

        internal static TieredPriceBuilder Create() => new TieredPriceBuilder();

        internal ICataloguePrice Build() => _cataloguePrice;
    }
}
