using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing
{
    public sealed class CataloguePriceTier : CataloguePriceBase
    {
        public CataloguePriceTier()
            : base(CataloguePriceType.Tiered)
        {
        }

        public IList<TieredPrice> TieredPrices { get; } = new List<TieredPrice>();
    }
}
