using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SolutionClientApplicationUpdater
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISolutionDetailRepository solutionDetailRepository;

        public SolutionClientApplicationUpdater(ISolutionDetailRepository solutionDetailRepository) =>
            this.solutionDetailRepository = solutionDetailRepository;

        public async Task UpdateAsync(
            ClientApplication clientApplication,
            string solutionId,
            CancellationToken cancellationToken)
        {
            await solutionDetailRepository.UpdateClientApplicationAsync(
                new UpdateSolutionClientApplicationRequest(solutionId, clientApplication),
                cancellationToken);
        }
    }
}
