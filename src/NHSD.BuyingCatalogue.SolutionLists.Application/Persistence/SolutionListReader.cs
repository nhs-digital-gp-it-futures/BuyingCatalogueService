using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Persistence
{
    internal sealed class SolutionListReader
    {
        /// <summary>
        /// Data access layer for the <see cref="NHSD.BuyingCatalogue.Application.SolutionList"/> entity.
        /// </summary>
        private readonly ISolutionListRepository _solutionListRepository;

        public SolutionListReader(ISolutionListRepository solutionListRepository)
        {
            _solutionListRepository = solutionListRepository;
        }

        public async Task<SolutionLists.Application.Domain.SolutionList> ListAsync(ISet<Guid> capabilityIdList, bool foundationOnly, CancellationToken cancellationToken)
        {
            if (capabilityIdList is null)
            {
                throw new System.ArgumentNullException(nameof(capabilityIdList));
            }

            return new SolutionLists.Application.Domain.SolutionList(capabilityIdList, await _solutionListRepository.ListAsync(foundationOnly, cancellationToken).ConfigureAwait(false));
        }
    }
}

