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
        private readonly IMarketingContactRepository marketingContactRepository;

        public ContactDetailsReader(IMarketingContactRepository marketingContactRepository)
        {
            this.marketingContactRepository = marketingContactRepository;
        }

        public async Task<IEnumerable<Contact>> ByIdAsync(string solutionId, CancellationToken cancellationToken) =>
            (await marketingContactRepository.BySolutionIdAsync(solutionId, cancellationToken)).Select(c => new Contact(c));
    }
}
