using System.Collections.Generic;
using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Queries
{
    public sealed class GetAdditionalServiceBySolutionIdsQuery : IRequest<IEnumerable<IAdditionalService>>
    {
        public IEnumerable<string> SolutionIds { get; }

        public GetAdditionalServiceBySolutionIdsQuery(IEnumerable<string> solutionIds)
        {
            SolutionIds = solutionIds;
        }
    }
}
