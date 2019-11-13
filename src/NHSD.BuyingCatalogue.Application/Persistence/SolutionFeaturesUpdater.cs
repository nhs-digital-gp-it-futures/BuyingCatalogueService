using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class SolutionFeaturesUpdater
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISolutionDetailRepository _solutionDetailRepository;

        public SolutionFeaturesUpdater(ISolutionDetailRepository solutionDetailRepository)
            => _solutionDetailRepository = solutionDetailRepository;

        public async Task UpdateAsync(Solution solution, CancellationToken cancellationToken)
            => await _solutionDetailRepository.UpdateFeaturesAsync(Map(solution), cancellationToken);

        private IUpdateSolutionFeaturesRequest Map(Solution solution)
            => new UpdateSolutionFeaturesRequest(solution.Id, solution.Features);
    }
}
