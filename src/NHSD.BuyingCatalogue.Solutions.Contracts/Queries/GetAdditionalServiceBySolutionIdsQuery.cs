using System.Collections.Generic;
using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetAdditionalServiceBySolutionIdsQuery : IRequest<IEnumerable<IAdditionalService>>
    {
        public GetAdditionalServiceBySolutionIdsQuery(IEnumerable<string> solutionIds)
        {
            SolutionIds = solutionIds;
        }

        public IEnumerable<string> SolutionIds { get; }
    }
}
