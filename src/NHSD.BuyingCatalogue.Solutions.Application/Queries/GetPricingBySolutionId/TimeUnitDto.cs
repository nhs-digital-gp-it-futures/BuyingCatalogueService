using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId
{
    public sealed class TimeUnitDto : ITimeUnit
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
