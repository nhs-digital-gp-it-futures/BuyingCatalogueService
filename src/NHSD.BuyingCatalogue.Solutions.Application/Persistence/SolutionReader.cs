using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SolutionReader
    {
        private readonly ISolutionRepository _solutionRepository;

        private readonly ISolutionCapabilityRepository _solutionCapabilityRepository;

        private readonly IMarketingContactRepository _contactRepository;

        public SolutionReader(ISolutionRepository solutionRepository,
            ISolutionCapabilityRepository solutionCapabilityRepository,
            IMarketingContactRepository contactRepository)
        {
            _solutionRepository = solutionRepository;
            _solutionCapabilityRepository = solutionCapabilityRepository;
            _contactRepository = contactRepository;
        }

        public async Task<Solution> ByIdAsync(string id, CancellationToken cancellationToken) =>
            new Solution((await _solutionRepository.ByIdAsync(id, cancellationToken))
                         ?? throw new NotFoundException(nameof(Solution), id),
                await _solutionCapabilityRepository.ListSolutionCapabilities(id, cancellationToken),
                await _contactRepository.BySolutionIdAsync(id, cancellationToken));
    }
}
