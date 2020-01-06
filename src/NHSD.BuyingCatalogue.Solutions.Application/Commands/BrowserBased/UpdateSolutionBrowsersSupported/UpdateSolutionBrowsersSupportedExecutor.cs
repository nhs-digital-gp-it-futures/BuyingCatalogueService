using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowsersSupported
{
    internal sealed class UpdateSolutionBrowsersSupportedExecutor : IExecutor<UpdateSolutionBrowsersSupportedCommand>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionBrowsersSupportedExecutor"/> class.
        /// </summary>
        public UpdateSolutionBrowsersSupportedExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="request">The command parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        public async Task UpdateAsync(UpdateSolutionBrowsersSupportedCommand request, CancellationToken cancellationToken) =>
            await _clientApplicationPartialUpdater.UpdateAsync(request.SolutionId,
                    clientApplication =>
                    {
                        clientApplication.BrowsersSupported = new HashSet<string>(request.UpdateSolutionBrowsersSupportedViewModel.BrowsersSupported);
                        clientApplication.MobileResponsive = request.UpdateSolutionBrowsersSupportedViewModel.MobileResponsive.ToBoolean();
                    },
                    cancellationToken)
                .ConfigureAwait(false);
    }
}
