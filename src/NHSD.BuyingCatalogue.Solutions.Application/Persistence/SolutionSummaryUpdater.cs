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
        private readonly ISolutionDetailRepository solutionDetailRepository;

        public SolutionSummaryUpdater(ISolutionDetailRepository solutionDetailRepository) =>
            this.solutionDetailRepository = solutionDetailRepository;

        public async Task UpdateSummaryAsync(Solution solution, CancellationToken cancellationToken) =>
            await solutionDetailRepository.UpdateSummaryAsync(Map(solution), cancellationToken);

        private static IUpdateSolutionSummaryRequest Map(Solution solution) =>
            new UpdateSolutionSummaryRequest(solution.Id, solution.Summary, solution.Description, solution.AboutUrl);
    }
}
