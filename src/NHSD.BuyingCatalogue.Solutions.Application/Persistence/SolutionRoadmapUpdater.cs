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
        private readonly ISolutionRepository solutionRepository;

        public SolutionRoadMapUpdater(ISolutionRepository solutionRepository) =>
            this.solutionRepository = solutionRepository;

        public async Task Update(string solutionId, string description, CancellationToken cancellationToken) =>
            await solutionRepository.UpdateRoadMapAsync(
                new UpdateRoadMapRequest(solutionId, description),
                cancellationToken);
    }
}
