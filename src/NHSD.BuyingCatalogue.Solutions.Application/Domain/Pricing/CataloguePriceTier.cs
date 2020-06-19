using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing
{
    public sealed class CataloguePriceTier : CataloguePriceBase
    {
        public IList<TieredPrice> TieredPrices { get; } = new List<TieredPrice>();

        public CataloguePriceTier() : base(CataloguePriceType.Tiered)
        {
        }
    }
}
