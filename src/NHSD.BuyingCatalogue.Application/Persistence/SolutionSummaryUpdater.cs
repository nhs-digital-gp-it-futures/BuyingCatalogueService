using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class SolutionSummaryUpdater
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISolutionDetailRepository _solutionDetailRepository;

        public SolutionSummaryUpdater(ISolutionDetailRepository solutionDetailRepository)
            => _solutionDetailRepository = solutionDetailRepository;

        public async Task UpdateSummaryAsync(Solution solution, CancellationToken cancellationToken)
            => await _solutionDetailRepository.UpdateSummaryAsync(Map(solution), cancellationToken);

        private IUpdateSolutionSummaryRequest Map(Solution solution)
            => new UpdateSolutionSummaryRequest(solution.Id, solution.Summary, solution.Description, solution.AboutUrl);
    }
}
