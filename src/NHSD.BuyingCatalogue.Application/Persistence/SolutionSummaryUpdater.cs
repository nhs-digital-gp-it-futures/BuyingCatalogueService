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
        private readonly ISolutionRepository _solutionRepository;

        public SolutionSummaryUpdater(ISolutionRepository solutionRepository)
        {
            _solutionRepository = solutionRepository;
        }

        public async Task UpdateSummaryAsync(Solution solution, CancellationToken cancellationToken)
        {
            await _solutionRepository.UpdateSummaryAsync(Map(solution), cancellationToken);
        }

        private IUpdateSolutionSummaryRequest Map(Solution solution)
            => new UpdateSolutionSummaryRequest
            {
                Id = solution.Id,
                Summary = solution.Summary,
                Description = solution.Description,
                AboutUrl = solution.AboutUrl
            };
    }
}
