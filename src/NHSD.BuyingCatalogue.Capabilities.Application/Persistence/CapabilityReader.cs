using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Capabilities.Application.Domain;
using NHSD.BuyingCatalogue.Capabilities.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Capabilities.Application.Persistence
{
    internal sealed class CapabilityReader
    {
        public CapabilityReader(ICapabilityRepository capabilityRepository) => CapabilityRepository = capabilityRepository;

        /// <summary>
        /// Gets the data access layer for the <see cref="Capability"/> entity.
        /// </summary>
        private ICapabilityRepository CapabilityRepository { get; }

        public async Task<IEnumerable<Capability>> ListAsync(CancellationToken cancellationToken) =>
            (await CapabilityRepository.ListAsync(cancellationToken)).Select(c => new Capability(c));
    }
}
