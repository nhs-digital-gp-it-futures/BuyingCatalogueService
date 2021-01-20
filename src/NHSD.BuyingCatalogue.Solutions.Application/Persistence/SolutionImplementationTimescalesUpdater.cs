using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class SolutionImplementationTimescalesUpdater
    {
        /// <summary>
        /// Data access layer for the <see cref="Solution"/> entity.
        /// </summary>
        private readonly ISolutionDetailRepository solutionDetailRepository;

        public SolutionImplementationTimescalesUpdater(ISolutionDetailRepository solutionDetailRepository) =>
            this.solutionDetailRepository = solutionDetailRepository;

        public async Task Update(string solutionId, string description, CancellationToken cancellationToken) =>
            await solutionDetailRepository.UpdateImplementationTimescalesAsync(
                new UpdateImplementationTimescalesRequest(solutionId, description),
                cancellationToken);
    }
}
