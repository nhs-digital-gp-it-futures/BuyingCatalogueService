using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId
{
    public sealed class PricingUnitDto : IPricingUnit
    {
        public string Name { get; set; }
        public string TierName { get; set; }
        public string Description { get; set; }
    }
}
