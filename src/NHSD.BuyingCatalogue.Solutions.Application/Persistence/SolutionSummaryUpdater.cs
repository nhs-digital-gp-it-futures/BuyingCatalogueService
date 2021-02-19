using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SolutionSummaryUpdater
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISolutionRepository solutionRepository;

        public SolutionSummaryUpdater(ISolutionRepository solutionRepository) =>
            this.solutionRepository = solutionRepository;

        public async Task UpdateSummaryAsync(Solution solution, CancellationToken cancellationToken) =>
            await solutionRepository.UpdateSummaryAsync(Map(solution), cancellationToken);

        private static IUpdateSolutionSummaryRequest Map(Solution solution) =>
            new UpdateSolutionSummaryRequest(solution.Id, solution.Summary, solution.Description, solution.AboutUrl);
    }
}
