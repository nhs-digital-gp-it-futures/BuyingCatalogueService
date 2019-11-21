using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.Solutions.Persistence
{
    internal sealed class SolutionClientApplicationUpdater
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISolutionDetailRepository _solutionDetailRepository;

        public SolutionClientApplicationUpdater(ISolutionDetailRepository solutionDetailRepository)
            => _solutionDetailRepository = solutionDetailRepository;

        public async Task UpdateAsync(ClientApplication clientApplication, string solutionId, CancellationToken cancellationToken)
            => await _solutionDetailRepository.UpdateClientApplicationAsync(new UpdateSolutionClientApplicationRequest(solutionId, clientApplication), cancellationToken);
    }
}
