using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing
{
    public sealed class CataloguePriceTier : CataloguePriceBase
    {
        public IList<TieredPrice> TieredPrices { get; }

        public CataloguePriceTier() : base(CataloguePriceType.Tiered)
        {
        }
    }

    public class TieredPrice
    {
        public int BandStart { get; }

        public int? BandEnd { get; }

        public decimal Price { get; }

        public TieredPrice(int bandStart, int? bandEnd, decimal price)
        {
            BandStart = bandStart;
            BandEnd = bandEnd;
            Price = price;
        }
    }
}
