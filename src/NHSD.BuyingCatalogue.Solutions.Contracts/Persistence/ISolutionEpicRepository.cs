using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISolutionEpicRepository
    {
        Task<IEnumerable<ISolutionEpicListResult>> ListSolutionEpicsAsync(string solutionId, CancellationToken cancellationToken);
        Task UpdateSolutionEpicAsync(string solutionId, IUpdateClaimedEpicListRequest request,
            CancellationToken cancellationToken);
    }
}
