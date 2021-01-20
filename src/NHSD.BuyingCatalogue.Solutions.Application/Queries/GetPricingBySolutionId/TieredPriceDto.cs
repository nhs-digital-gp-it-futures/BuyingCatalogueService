using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId
{
    public sealed class TieredPriceDto : ITieredPrice
    {
        public int BandStart { get; set; }

        public int? BandEnd { get; set; }

        public decimal Price { get; set; }
    }
}
