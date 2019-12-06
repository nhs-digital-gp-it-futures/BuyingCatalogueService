using System;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class ClientApplicationPartialUpdater
    {
        private readonly SolutionReader _solutionReader;
        private readonly SolutionClientApplicationUpdater _solutionClientApplicationUpdater;

        /// <summary>
        /// Initialises a new instance of the <see cref="ClientApplicationPartialUpdater"/> class.
        /// </summary>
        public ClientApplicationPartialUpdater(SolutionReader solutionReader, SolutionClientApplicationUpdater solutionClientApplicationUpdater)
        {
            _solutionReader = solutionReader;
            _solutionClientApplicationUpdater = solutionClientApplicationUpdater;
        }

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="request">The command parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this command.</returns>
        public async Task UpdateAsync(string solutionId, Action<ClientApplication> updateAction, CancellationToken cancellationToken)
        {
            var clientApplication = (await _solutionReader.ByIdAsync(solutionId, cancellationToken).ConfigureAwait(false)).ClientApplication;

            updateAction(clientApplication);

            await _solutionClientApplicationUpdater.UpdateAsync(clientApplication, solutionId, cancellationToken).ConfigureAwait(false);
        }
    }
}
