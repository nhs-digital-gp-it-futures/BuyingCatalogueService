using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SolutionVerifier
    {
        private readonly ISolutionRepository solutionRepository;

        public SolutionVerifier(ISolutionRepository solutionRepository) => this.solutionRepository = solutionRepository;

        public async Task<bool> CheckExists(string solutionId, CancellationToken cancellationToken) =>
            await solutionRepository.CheckExists(solutionId, cancellationToken);

        public async Task ThrowWhenMissingAsync(string solutionId, CancellationToken cancellationToken)
        {
            if (!await CheckExists(solutionId, cancellationToken))
            {
                throw new NotFoundException(nameof(Solution), solutionId);
            }
        }
    }
}
