using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.SolutionList.Persistence
{
    internal sealed class SolutionListReader
    {
        /// <summary>
        /// Data access layer for the <see cref="SolutionList"/> entity.
        /// </summary>
        private readonly ISolutionListRepository _solutionListRepository;

        public SolutionListReader(ISolutionListRepository solutionListRepository)
        {
            _solutionListRepository = solutionListRepository;
        }

        public async Task<Domain.SolutionList> ListAsync(ISet<Guid> capabilityIdList, bool foundationOnly, CancellationToken cancellationToken)
        {
            if (capabilityIdList is null)
            {
                throw new System.ArgumentNullException(nameof(capabilityIdList));
            }

            return new Domain.SolutionList(capabilityIdList, await _solutionListRepository.ListAsync(foundationOnly, cancellationToken).ConfigureAwait(false));
        }
    }
}

