using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal class SolutionContactDetailsUpdater
    {
        private readonly IMarketingContactRepository _repository;

        public SolutionContactDetailsUpdater(IMarketingContactRepository repository)
        {
            _repository = repository;
        }

        public async Task UpdateAsync(string solutionId, IEnumerable<IContact> newContacts, CancellationToken cancellationToken)
            => await _repository.ReplaceContactsForSolution(solutionId, newContacts, cancellationToken);
    }
}
