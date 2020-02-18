using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.SolutionLists.Application.Domain;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Persistence
{
    internal sealed class SolutionListReader
    {
        /// <summary>
        /// Data access layer for the <see cref="SolutionList"/> entity.
        /// </summary>
        private readonly ISolutionListRepository _solutionListRepository;

        public SolutionListReader(ISolutionListRepository solutionListRepository)
            => _solutionListRepository = solutionListRepository;

        public async Task<SolutionList> ListAsync(ISet<ICapabilityReference> capabilityReferences, bool foundationOnly, CancellationToken cancellationToken)
            => new SolutionList(capabilityReferences.ThrowIfNull(nameof(capabilityReferences)),
                await _solutionListRepository.ListAsync(foundationOnly, cancellationToken).ConfigureAwait(false));
    }
}

