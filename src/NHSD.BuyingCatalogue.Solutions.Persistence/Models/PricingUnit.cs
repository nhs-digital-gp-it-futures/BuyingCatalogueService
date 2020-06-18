using System;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    public sealed class PricingUnit : IPricingUnit
    {
        public Guid PricingUnitId { get; }
        public string Name { get; }
        public string TierName { get; set; }
        public string Description { get; set; }
    }
}
