using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISolutionEpicRepository
    {
        Task UpdateSolutionEpicAsync(string solutionId, IUpdateClaimedEpicListRequest request,
            CancellationToken cancellationToken);
    }
}
