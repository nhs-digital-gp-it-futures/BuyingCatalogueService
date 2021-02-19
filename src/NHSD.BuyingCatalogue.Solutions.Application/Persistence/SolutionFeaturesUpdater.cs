using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SolutionFeaturesUpdater
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISolutionRepository solutionRepository;

        public SolutionFeaturesUpdater(ISolutionRepository solutionRepository) =>
            this.solutionRepository = solutionRepository;

        public async Task UpdateAsync(Solution solution, CancellationToken cancellationToken) =>
            await solutionRepository.UpdateFeaturesAsync(Map(solution), cancellationToken);

        private static IUpdateSolutionFeaturesRequest Map(Solution solution) =>
            new UpdateSolutionFeaturesRequest(solution.Id, solution.Features);
    }
}
