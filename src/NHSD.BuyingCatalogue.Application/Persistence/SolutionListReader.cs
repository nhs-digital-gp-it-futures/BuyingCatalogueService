using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Persistence
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

        public async Task<IEnumerable<Solution>> ListAsync(ISet<Guid> capabilityIdList, CancellationToken cancellationToken)
        {
            if (capabilityIdList is null)
            {
                throw new System.ArgumentNullException(nameof(capabilityIdList));
            }

            var solutionList = (await _solutionRepository.ListAsync(cancellationToken).ConfigureAwait(false)).Map();

            return capabilityIdList.Any() ?
                solutionList.Where(solution => capabilityIdList.Intersect(solution.Capabilities.Select(capability => capability.Id)).Count() == capabilityIdList.Count()) :
                solutionList;
        }
    }
}

