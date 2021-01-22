using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders
{
    internal sealed class PriceUnitDtoBuilder
    {
        private readonly string name;
        private readonly string description;
        private readonly string tierName;

        private PriceUnitDtoBuilder()
        {
            name = "name";
            description = "desc";
            tierName = "tier";
        }

        internal static PriceUnitDtoBuilder Create() => new();

        internal IPricingUnit Build()
        {
            return new PricingUnitDto
            {
                Name = name,
                Description = description,
                TierName = tierName,
            };
        }
    }
}
