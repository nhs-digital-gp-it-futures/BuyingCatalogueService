using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SolutionReader
    {
        private readonly ISolutionRepository _solutionRepository;

        private readonly ISolutionCapabilityRepository _solutionCapabilityRepository;

        private readonly IMarketingContactRepository _contactRepository;
        private readonly ISupplierRepository _supplierRepository;

        public SolutionReader(ISolutionRepository solutionRepository,
            ISolutionCapabilityRepository solutionCapabilityRepository,
            IMarketingContactRepository contactRepository,
            ISupplierRepository supplierRepository)
        {
            _solutionRepository = solutionRepository;
            _solutionCapabilityRepository = solutionCapabilityRepository;
            _contactRepository = contactRepository;
            _supplierRepository = supplierRepository;
        }

        public async Task<Solution> ByIdAsync(string id, CancellationToken cancellationToken) =>
            new Solution(await _solutionRepository.ByIdAsync(id, cancellationToken).ConfigureAwait(false)
                         ?? throw new NotFoundException(nameof(Solution), id),
                await _solutionCapabilityRepository.ListSolutionCapabilities(id, cancellationToken)
                    .ConfigureAwait(false),
                await _contactRepository.BySolutionIdAsync(id, cancellationToken)
                    .ConfigureAwait(false),
                await _supplierRepository.GetSupplierBySolutionIdAsync(id, cancellationToken)
                    .ConfigureAwait(false));
    }
}
