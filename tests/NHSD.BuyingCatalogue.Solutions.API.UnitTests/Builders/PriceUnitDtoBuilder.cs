using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetPricingBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.Builders
{
    internal sealed class PriceUnitDtoBuilder
    {
        private readonly string _name;
        private readonly string _description;
        private readonly string _tierName;

        private PriceUnitDtoBuilder()
        {
            _name = "name";
            _description = "desc";
            _tierName = "tier";
        }

        internal static PriceUnitDtoBuilder Create() => new PriceUnitDtoBuilder();

        internal IPricingUnit Build()
        {
            return new PricingUnitDto
            {
                Name = _name,
                Description = _description,
                TierName = _tierName
            };
        }
    }
}
