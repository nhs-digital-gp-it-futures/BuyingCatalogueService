using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SolutionVerifier
    {
        private readonly ISolutionRepository _solutionRepository;

        public SolutionVerifier(ISolutionRepository solutionRepository)
         => _solutionRepository = solutionRepository;

        public async Task<bool> CheckExists(string solutionId, CancellationToken cancellationToken)
            => await _solutionRepository.CheckExists(solutionId, cancellationToken).ConfigureAwait(false);
        
        public async Task ThrowWhenMissing(string solutionId, CancellationToken cancellationToken)
        {
            if (!await CheckExists(solutionId, cancellationToken).ConfigureAwait(false))
            {
                throw new NotFoundException(nameof(Solution), solutionId);
            }
        }
    }
}
