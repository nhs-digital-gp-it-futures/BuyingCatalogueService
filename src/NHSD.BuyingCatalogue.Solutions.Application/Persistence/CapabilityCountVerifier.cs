using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class CapabilityCountVerifier 
    {
        private readonly ISolutionCapabilityRepository _solutionCapabilityRepository;

        public CapabilityCountVerifier(ISolutionCapabilityRepository solutionCapabilityRepository)
        {
            _solutionCapabilityRepository = solutionCapabilityRepository;
        }

        public async Task<bool> CheckCapabilityReferenceExists(IEnumerable<string> capabilitiesToMatch,
            CancellationToken cancellationToken)
        {
            var count = await _solutionCapabilityRepository.GetMatchingCapabilitiesCount(capabilitiesToMatch,
                cancellationToken).ConfigureAwait(false);

            return count == capabilitiesToMatch.ToList().Count;
        }
    }
}
