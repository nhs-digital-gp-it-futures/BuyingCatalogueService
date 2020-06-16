using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetPricingBySolutionIdQuery : IRequest<IPricingUnit>
    {
        public string SolutionId { get; }

        public GetPricingBySolutionIdQuery(string solutionId)
        {
            SolutionId = solutionId;
        }
    }
}
