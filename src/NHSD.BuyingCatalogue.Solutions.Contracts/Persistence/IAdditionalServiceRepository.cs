using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IAdditionalServiceRepository
    {
        Task<IEnumerable<IAdditionalServiceResult>> GetAdditionalServiceBySolutionIdsAsync(IEnumerable<string> solutionIds,
            CancellationToken cancellationToken);
    }
}
