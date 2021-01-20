using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SolutionCapabilitiesUpdater
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISolutionCapabilityRepository solutionCapabilityRepository;

        public SolutionCapabilitiesUpdater(ISolutionCapabilityRepository solutionCapabilityRepository) =>
            this.solutionCapabilityRepository = solutionCapabilityRepository;

        public async Task UpdateAsync(string solutionId, IEnumerable<string> newCapabilitiesReferences, CancellationToken cancellationToken)
        {
            await solutionCapabilityRepository.UpdateCapabilitiesAsync(
                new UpdateCapabilityRequest(solutionId, newCapabilitiesReferences),
                cancellationToken);
        }
    }
}
