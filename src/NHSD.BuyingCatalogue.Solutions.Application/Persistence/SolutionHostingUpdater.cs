using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SolutionHostingUpdater
    {
        private readonly ISolutionDetailRepository _solutionDetailRepository;

        public SolutionHostingUpdater(ISolutionDetailRepository solutionDetailRepository)
            => _solutionDetailRepository = solutionDetailRepository;

        public async Task UpdateAsync(Hosting hosting, string solutionId, CancellationToken cancellationToken)
            => await _solutionDetailRepository.UpdateHostingAsync(new UpdateSolutionHostingRequest(solutionId, hosting), cancellationToken)
                .ConfigureAwait(false);
    }
}
