using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISolutionEpicStatusRepository
    {
        Task<int> GetMatchingEpicStatusAsync(IEnumerable<string> statusNames, CancellationToken cancellationToken);
    }
}
