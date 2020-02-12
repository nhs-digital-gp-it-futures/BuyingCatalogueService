using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISolutionEpicsRepository
    {
        Task UpdateSolutionEpicAsync(string solutionId, IUpdateClaimedRequest request,
            CancellationToken cancellationToken);
    }
}
