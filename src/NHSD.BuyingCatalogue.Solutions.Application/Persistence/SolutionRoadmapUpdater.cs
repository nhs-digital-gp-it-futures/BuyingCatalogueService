using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SolutionRoadmapUpdater
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISolutionDetailRepository _solutionDetailRepository;

        public SolutionRoadmapUpdater(ISolutionDetailRepository solutionDetailRepository)
            => _solutionDetailRepository = solutionDetailRepository;

        public async Task Update(string solutionId, string description, CancellationToken cancellationToken)
            => await _solutionDetailRepository.UpdateRoadmapAsync(
                    new UpdateRoadmapRequest(solutionId, description),
                    cancellationToken).ConfigureAwait(false);
    }
}
