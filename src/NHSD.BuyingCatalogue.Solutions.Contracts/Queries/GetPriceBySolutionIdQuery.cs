using System.Collections.Generic;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Contracts.Pricing;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetPriceBySolutionIdQuery : IRequest<IEnumerable<ICataloguePrice>>
    {
        public string SolutionId { get; }

        public GetPriceBySolutionIdQuery(string solutionId)
        {
            SolutionId = solutionId;
        }
    }
}
