using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SolutionHostingUpdater
    {
        private readonly ISolutionDetailRepository solutionDetailRepository;

        public SolutionHostingUpdater(ISolutionDetailRepository solutionDetailRepository) =>
            this.solutionDetailRepository = solutionDetailRepository;

        public async Task UpdateAsync(Hosting hosting, string solutionId, CancellationToken cancellationToken) =>
            await solutionDetailRepository.UpdateHostingAsync(new UpdateSolutionHostingRequest(solutionId, hosting), cancellationToken);
    }
}
