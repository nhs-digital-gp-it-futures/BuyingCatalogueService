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
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISolutionRepository _solutionRepository;

        public SolutionListReader(ISolutionRepository solutionRepository)
        {
            _solutionRepository = solutionRepository;
        }

        public async Task<Domain.SolutionList> ListAsync(ISet<Guid> capabilityIdList, bool foundationOnly, CancellationToken cancellationToken)
        {
            if (capabilityIdList is null)
            {
                throw new System.ArgumentNullException(nameof(capabilityIdList));
            }

            return new Domain.SolutionList(capabilityIdList, await _solutionRepository.ListAsync(foundationOnly, cancellationToken).ConfigureAwait(false));
        }
    }
}

