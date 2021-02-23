using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SolutionHostingUpdater
    {
        private readonly ISolutionRepository solutionRepository;

        public SolutionHostingUpdater(ISolutionRepository solutionRepository) =>
            this.solutionRepository = solutionRepository;

        public async Task UpdateAsync(Hosting hosting, string solutionId, CancellationToken cancellationToken) =>
            await solutionRepository.UpdateHostingAsync(new UpdateSolutionHostingRequest(solutionId, hosting), cancellationToken);
    }
}
