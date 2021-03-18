using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SolutionReader
    {
        private readonly IMarketingContactRepository contactRepository;
        private readonly IDocumentRepository documentRepository;
        private readonly ISolutionEpicRepository epicRepository;

        private readonly ISolutionCapabilityRepository solutionCapabilityRepository;
        private readonly ISolutionRepository solutionRepository;

        private readonly ISupplierRepository supplierRepository;
        private readonly ISolutionFrameworkRepository frameworkRepository;

        public SolutionReader(
            ISolutionRepository solutionRepository,
            ISolutionCapabilityRepository solutionCapabilityRepository,
            IMarketingContactRepository contactRepository,
            ISupplierRepository supplierRepository,
            IDocumentRepository documentRepository,
            ISolutionEpicRepository epicRepository,
            ISolutionFrameworkRepository frameworkRepository)
        {
            this.solutionRepository = solutionRepository;
            this.solutionCapabilityRepository = solutionCapabilityRepository;
            this.contactRepository = contactRepository;
            this.supplierRepository = supplierRepository;
            this.documentRepository = documentRepository;
            this.epicRepository = epicRepository;
            this.frameworkRepository = frameworkRepository;
        }

        public async Task<Solution> ByIdAsync(string id, CancellationToken cancellationToken) =>
            new(
                await solutionRepository.ByIdAsync(id, cancellationToken) ?? throw new NotFoundException(nameof(Solution), id),
                await solutionCapabilityRepository.ListSolutionCapabilitiesAsync(id, cancellationToken),
                await contactRepository.BySolutionIdAsync(id, cancellationToken),
                await supplierRepository.GetSupplierBySolutionIdAsync(id, cancellationToken),
                await documentRepository.GetDocumentResultBySolutionIdAsync(id, cancellationToken),
                await epicRepository.ListSolutionEpicsAsync(id, cancellationToken),
                await frameworkRepository.GetFrameworkBySolutionIdAsync(id, cancellationToken));
    }
}
