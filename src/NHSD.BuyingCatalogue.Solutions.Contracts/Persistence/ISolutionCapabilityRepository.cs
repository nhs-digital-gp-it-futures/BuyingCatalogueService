using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISolutionCapabilityRepository
    {
        Task<IEnumerable<ISolutionCapabilityListResult>> ListSolutionCapabilities(string solutionId, CancellationToken cancellationToken);

        Task UpdateCapabilitiesAsync(IUpdateCapabilityRequest updateCapabilityRequest, CancellationToken cancellationToken);
    }
}
