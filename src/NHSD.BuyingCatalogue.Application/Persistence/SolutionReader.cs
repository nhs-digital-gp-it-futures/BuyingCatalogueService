using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class SolutionReader
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISolutionRepository _solutionRepository;

        public SolutionReader(ISolutionRepository solutionRepository)
        {
            _solutionRepository = solutionRepository;
        }

        public async Task<Solution> ByIdAsync(string id, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.ByIdAsync(id, cancellationToken);

            return solution == null ? null : Map(solution);
        }

        private Solution Map(ISolutionResult solutionResult)
            => new Solution
            {
                Id = solutionResult.Id,
                Name = solutionResult.Name,
                Summary = solutionResult.Summary,
                Description = solutionResult.Description,
                Features = solutionResult.Features,
                AboutUrl = solutionResult.AboutUrl
            };
    }
}

