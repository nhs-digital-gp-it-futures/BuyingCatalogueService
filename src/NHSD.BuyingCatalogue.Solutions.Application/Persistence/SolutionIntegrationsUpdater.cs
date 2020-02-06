using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SolutionIntegrationsUpdater
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISolutionDetailRepository _solutionDetailRepository;

        public SolutionIntegrationsUpdater(ISolutionDetailRepository solutionDetailRepository)
            => _solutionDetailRepository = solutionDetailRepository;

        public async Task Update(string solutionId, string url, CancellationToken cancellationToken)
            => await _solutionDetailRepository.UpdateIntegrationsAsync(
                new UpdateIntegrationsRequest(solutionId, url),
                cancellationToken).ConfigureAwait(false);
    }
}
