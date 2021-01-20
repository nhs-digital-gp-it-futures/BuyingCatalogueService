using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class ImplementationTimescalesReader
    {
        private readonly ISolutionDetailRepository solutionDetailRepository;

        public ImplementationTimescalesReader(ISolutionDetailRepository solutionDetailRepository)
        {
            this.solutionDetailRepository = solutionDetailRepository;
        }

        public async Task<ImplementationTimescales> BySolutionIdAsync(string id, CancellationToken cancellationToken)
        {
            var implementationTimescalesResult = await solutionDetailRepository.GetImplementationTimescalesBySolutionIdAsync(id, cancellationToken);
            return new ImplementationTimescales { Description = implementationTimescalesResult.Description };
        }
    }
}
