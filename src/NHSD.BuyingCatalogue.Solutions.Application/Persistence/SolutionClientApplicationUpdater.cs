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
        private readonly ISolutionRepository solutionRepository;

        public SolutionClientApplicationUpdater(ISolutionRepository solutionRepository) =>
            this.solutionRepository = solutionRepository;

        public async Task UpdateAsync(
            ClientApplication clientApplication,
            string solutionId,
            CancellationToken cancellationToken)
        {
            await solutionRepository.UpdateClientApplicationAsync(
                new UpdateSolutionClientApplicationRequest(solutionId, clientApplication),
                cancellationToken);
        }
    }
}
