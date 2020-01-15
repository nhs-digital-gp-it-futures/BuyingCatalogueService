using System;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class HostingPartialUpdater
    {
        private readonly SolutionReader _solutionReader;
        private readonly SolutionHostingUpdater _solutionHostingUpdater;

        public HostingPartialUpdater(SolutionReader solutionReader, SolutionHostingUpdater solutionHostingUpdater)
        {
            _solutionReader = solutionReader;
            _solutionHostingUpdater = solutionHostingUpdater;
        }

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="solutionId">The ID of the solution to update</param>
        /// <param name="updateAction">The action to run for updating the application</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this command.</returns>
        public async Task UpdateAsync(string solutionId, Action<Hosting> updateAction, CancellationToken cancellationToken)
        {
            var hosting = (await _solutionReader.ByIdAsync(solutionId, cancellationToken).ConfigureAwait(false)).Hosting;

            updateAction(hosting);

            await _solutionHostingUpdater.UpdateAsync(hosting, solutionId, cancellationToken).ConfigureAwait(false);
        }
    }
}
