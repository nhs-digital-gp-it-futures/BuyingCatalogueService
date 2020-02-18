using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISolutionCapabilityRepository
    {
        Task<IEnumerable<ISolutionCapabilityListResult>> ListSolutionCapabilitiesAsync(string solutionId, CancellationToken cancellationToken);

        Task UpdateCapabilitiesAsync(IUpdateCapabilityRequest updateCapabilityRequest, CancellationToken cancellationToken);

        Task<int> GetMatchingCapabilitiesCountAsync(IEnumerable<string> capabilitiesToMatch,
            CancellationToken cancellationToken);
    }
}
