using System;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing
{
    public sealed class PricingUnit
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string TierName { get; set; }

        public string Description { get; set; }

        public static PricingUnit Create(IPricingUnit pricingUnit)
        {
            if (pricingUnit is null)
            {
                return null;
            }

            return new PricingUnit
            {
                Id = pricingUnit.PricingUnitId,
                Name = pricingUnit.Name,
                TierName = pricingUnit.TierName,
                Description = pricingUnit.Description
            };
        }
    }
}
