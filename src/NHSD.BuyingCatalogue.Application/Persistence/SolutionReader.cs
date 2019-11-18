using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.Persistence
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
