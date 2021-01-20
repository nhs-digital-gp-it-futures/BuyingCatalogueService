using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SolutionRoadMapUpdater
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISolutionDetailRepository solutionDetailRepository;

        public SolutionRoadMapUpdater(ISolutionDetailRepository solutionDetailRepository) =>
            this.solutionDetailRepository = solutionDetailRepository;

        public async Task Update(string solutionId, string description, CancellationToken cancellationToken) =>
            await solutionDetailRepository.UpdateRoadmapAsync(
                new UpdateRoadmapRequest(solutionId, description),
                cancellationToken);
    }
}
