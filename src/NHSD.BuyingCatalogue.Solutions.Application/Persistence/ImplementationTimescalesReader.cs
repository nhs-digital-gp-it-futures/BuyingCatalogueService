using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class ImplementationTimescalesReader
    {
        private readonly ISolutionRepository solutionRepository;

        public ImplementationTimescalesReader(ISolutionRepository solutionRepository)
        {
            this.solutionRepository = solutionRepository;
        }

        public async Task<ImplementationTimescales> BySolutionIdAsync(string id, CancellationToken cancellationToken)
        {
            var implementationTimescalesResult = await solutionRepository.GetImplementationTimescalesBySolutionIdAsync(id, cancellationToken);
            return new ImplementationTimescales { Description = implementationTimescalesResult.Description };
        }
    }
}
