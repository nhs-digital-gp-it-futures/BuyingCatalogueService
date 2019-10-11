using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class SolutionUpdater
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISolutionRepository _solutionRepository;

        public SolutionUpdater(ISolutionRepository solutionRepository)
        {
            _solutionRepository = solutionRepository;
        }

        public async Task UpdateAsync(Solution solution, CancellationToken cancellationToken)
        {
            await _solutionRepository.UpdateAsync(Map(solution), cancellationToken);
        }

        private IUpdateSolutionRequest Map(Solution solution)
            => new UpdateSolutionRequest
            {
                Id = solution.Id,
                Summary = solution.Summary,
                Description = solution.Description,
                Features = solution.Features,
                AboutUrl = solution.AboutUrl
            };
    }
}
