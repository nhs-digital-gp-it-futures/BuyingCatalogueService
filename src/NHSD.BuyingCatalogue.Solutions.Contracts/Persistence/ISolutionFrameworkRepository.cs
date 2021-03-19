using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISolutionFrameworkRepository
    {
        Task<IEnumerable<ISolutionFrameworkListResult>> GetFrameworkBySolutionIdAsync(string solutionId, CancellationToken cancellationToken);
    }
}
