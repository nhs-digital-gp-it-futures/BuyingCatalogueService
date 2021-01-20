using System;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class ClientApplicationPartialUpdater
    {
        private readonly SolutionReader solutionReader;
        private readonly SolutionClientApplicationUpdater solutionClientApplicationUpdater;

        public ClientApplicationPartialUpdater(
            SolutionReader solutionReader,
            SolutionClientApplicationUpdater solutionClientApplicationUpdater)
        {
            this.solutionReader = solutionReader;
            this.solutionClientApplicationUpdater = solutionClientApplicationUpdater;
        }

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="solutionId">The ID of the solution to update.</param>
        /// <param name="updateAction">The action to run for updating the application.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this command.</returns>
        public async Task UpdateAsync(string solutionId, Action<ClientApplication> updateAction, CancellationToken cancellationToken)
        {
            var clientApplication = (await solutionReader.ByIdAsync(solutionId, cancellationToken)).ClientApplication;

            updateAction(clientApplication);

            await solutionClientApplicationUpdater.UpdateAsync(clientApplication, solutionId, cancellationToken);
        }
    }
}
