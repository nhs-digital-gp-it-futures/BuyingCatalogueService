using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Application.Persistence;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution
{
    internal sealed class UpdateSolutionBrowsersSupportedHandler : IRequestHandler<UpdateSolutionBrowsersSupportedCommand>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionBrowsersSupportedHandler"/> class.
        /// </summary>
        public UpdateSolutionBrowsersSupportedHandler(ClientApplicationPartialUpdater clientApplicationPartialUpdater)
        {
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;
        }

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="request">The command parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this command.</returns>
        public async Task<Unit> Handle(UpdateSolutionBrowsersSupportedCommand request, CancellationToken cancellationToken)
        {
            await _clientApplicationPartialUpdater.UpdateAsync(request.SolutionId,
                clientApplication =>
                {
                    clientApplication.BrowsersSupported = new HashSet<string>(request.UpdateSolutionBrowsersSupportedViewModel.BrowsersSupported);
                    clientApplication.MobileResponsive = MapMobileResponsive(request.UpdateSolutionBrowsersSupportedViewModel.MobileResponsive);
                },
                cancellationToken);

            return Unit.Value;
        }

        private bool? MapMobileResponsive(string mobileResponsive) => mobileResponsive == "yes" ? true : (mobileResponsive == "no" ? false : (bool?)null);
    }
}
