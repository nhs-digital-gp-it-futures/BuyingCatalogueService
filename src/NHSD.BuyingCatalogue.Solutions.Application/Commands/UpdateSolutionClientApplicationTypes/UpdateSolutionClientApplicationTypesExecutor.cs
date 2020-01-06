using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionClientApplicationTypes
{
    internal sealed class UpdateSolutionClientApplicationTypesExecutor : IExecutor<UpdateSolutionClientApplicationTypesCommand>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionClientApplicationTypesHandler"/> class.
        /// </summary>
        public UpdateSolutionClientApplicationTypesExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="request">The command parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this command.</returns>
        public async Task UpdateAsync(UpdateSolutionClientApplicationTypesCommand request, CancellationToken cancellationToken) =>
            await _clientApplicationPartialUpdater.UpdateAsync(request.SolutionId,
                    clientApplication => clientApplication.ClientApplicationTypes = new HashSet<string>(request.Data.FilteredClientApplicationTypes),
                    cancellationToken)
                .ConfigureAwait(false);
    }
}
