using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionConnectivityAndResolution
{
    internal class UpdateSolutionConnectivityAndResolutionHandler : IRequestHandler<UpdateSolutionConnectivityAndResolutionCommand, UpdateSolutionConnectivityAndResolutionValidationResult>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;
        private readonly UpdateSolutionConnectivityAndResolutionValidator _updateSolutionConnectivityAndResolutionValidator;

        public UpdateSolutionConnectivityAndResolutionHandler(ClientApplicationPartialUpdater clientApplicationPartialUpdater, UpdateSolutionConnectivityAndResolutionValidator updateSolutionConnectivityAndResolutionValidator)
        {
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;
            _updateSolutionConnectivityAndResolutionValidator = updateSolutionConnectivityAndResolutionValidator;
        }

        public async Task<UpdateSolutionConnectivityAndResolutionValidationResult> Handle(UpdateSolutionConnectivityAndResolutionCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _updateSolutionConnectivityAndResolutionValidator.Validation(request.ViewModel);

            if (validationResult.IsValid)
            {
                await _clientApplicationPartialUpdater.UpdateAsync(request.Id,
                        clientApplication =>
                        {
                            clientApplication.MinimumConnectionSpeed = request.ViewModel.MinimumConnectionSpeed;
                            clientApplication.MinimumDesktopResolution = request.ViewModel.MinimumDesktopResolution;
                        },
                        cancellationToken)
                    .ConfigureAwait(false);
            }

            return validationResult;
        }
    }
}
