using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId
{
    public sealed class TieredCataloguePriceDto : CataloguePriceDto
    {
        public IEnumerable<ITieredPrice> TieredPrices { get; set; }
    }
}
