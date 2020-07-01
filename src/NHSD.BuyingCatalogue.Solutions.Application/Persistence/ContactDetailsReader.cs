using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class ContactDetailsReader
    {

        private readonly IMarketingContactRepository _marketingContactRepository;

        public ContactDetailsReader(IMarketingContactRepository marketingContactRepository)
        {
            _marketingContactRepository = marketingContactRepository;
        }

        public async Task<IEnumerable<Contact>> ByIdAsync(string solutionId, CancellationToken cancellationToken)
            => (await _marketingContactRepository.BySolutionIdAsync(solutionId, cancellationToken).ConfigureAwait(false)).Select(c => new Contact(c));
    }
}
