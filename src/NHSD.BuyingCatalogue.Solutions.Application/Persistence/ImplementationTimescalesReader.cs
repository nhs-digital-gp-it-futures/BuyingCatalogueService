using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class ImplementationTimescalesReader
    {
        private readonly ISolutionDetailRepository _solutionDetailRepository;

        public ImplementationTimescalesReader(ISolutionDetailRepository solutionDetailRepository)
        {
            _solutionDetailRepository = solutionDetailRepository;
        }

        public async Task<ImplementationTimescales> BySolutionIdAsync(string id, CancellationToken cancellationToken)
        {
            var implementationTimescalesResult = await _solutionDetailRepository.GetImplementationTimescalesBySolutionIdAsync(id, cancellationToken)
                .ConfigureAwait(false);
            return new ImplementationTimescales{Description = implementationTimescalesResult.Description};
        }
    }
}
