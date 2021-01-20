using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SolutionContactDetailsUpdater
    {
        private readonly IMarketingContactRepository repository;

        public SolutionContactDetailsUpdater(IMarketingContactRepository repository)
        {
            this.repository = repository;
        }

        public async Task UpdateAsync(
            string solutionId,
            IEnumerable<IContact> newContacts,
            CancellationToken cancellationToken)
        {
            await repository.ReplaceContactsForSolution(solutionId, newContacts, cancellationToken);
        }
    }
}
