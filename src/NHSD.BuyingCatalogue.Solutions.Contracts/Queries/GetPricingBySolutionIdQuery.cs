using System.Collections.Generic;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetPricingBySolutionIdQuery : IRequest<IEnumerable<ICataloguePrice>>
    {
        public string SolutionId { get; }

        public GetPricingBySolutionIdQuery(string solutionId)
        {
            SolutionId = solutionId;
        }
    }
}
