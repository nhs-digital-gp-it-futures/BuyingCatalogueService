using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Application.Solutions.Persistence;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionBrowsersSupported
{
    internal sealed class UpdateSolutionBrowsersSupportedHandler : IRequestHandler<UpdateSolutionBrowsersSupportedCommand, UpdateSolutionBrowserSupportedValidationResult>
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
        public async Task<UpdateSolutionBrowserSupportedValidationResult> Handle(UpdateSolutionBrowsersSupportedCommand request, CancellationToken cancellationToken)
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
                    cancellationToken);
            }

            return validationResult;
        }
    }
}
