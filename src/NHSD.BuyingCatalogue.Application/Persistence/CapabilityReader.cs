using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Application.Capabilities.Domain;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class CapabilityReader
    {
        /// <summary>
        /// Data access layer for the <see cref="Capability"/> entity.
        /// </summary>
        private ICapabilityRepository CapabilityRepository { get; }

        public CapabilityReader(ICapabilityRepository capabilityRepository) => CapabilityRepository = capabilityRepository;

        public async Task<IEnumerable<Capability>> ListAsync(CancellationToken cancellationToken) =>
            (await CapabilityRepository.ListAsync(cancellationToken).ConfigureAwait(false))
            .Select(c => new Capability(c));
    }
}
