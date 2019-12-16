using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowsersSupported
{
    internal sealed class UpdateSolutionBrowsersSupportedHandler : IRequestHandler<UpdateSolutionBrowsersSupportedCommand, RequiredResult>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        private readonly UpdateSolutionBrowsersSupportedValidator _updateSolutionBrowsersSupportedValidator;

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionBrowsersSupportedHandler"/> class.
        /// </summary>
        public UpdateSolutionBrowsersSupportedHandler(ClientApplicationPartialUpdater clientApplicationPartialUpdater, UpdateSolutionBrowsersSupportedValidator updateSolutionBrowsersSupportedValidator)
        {
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;
            _updateSolutionBrowsersSupportedValidator = updateSolutionBrowsersSupportedValidator;
        }

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="request">The command parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A validationResult representing an operation to get the result of this command. Deeming if it is valid</returns>
        public async Task<RequiredResult> Handle(UpdateSolutionBrowsersSupportedCommand request, CancellationToken cancellationToken)
        {
            var validationResult =
                _updateSolutionBrowsersSupportedValidator.Validation(request.UpdateSolutionBrowsersSupportedViewModel);

            if (validationResult.IsValid)
            {
                await _clientApplicationPartialUpdater.UpdateAsync(request.SolutionId,
                    clientApplication =>
                    {
                        clientApplication.BrowsersSupported = new HashSet<string>(request.UpdateSolutionBrowsersSupportedViewModel.BrowsersSupported);
                        clientApplication.MobileResponsive = request.UpdateSolutionBrowsersSupportedViewModel.MobileResponsive.ToBoolean();
                    },
                    cancellationToken)
                    .ConfigureAwait(false);
            }

            return validationResult;
        }
    }
}
