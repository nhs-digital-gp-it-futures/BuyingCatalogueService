using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserMobileFirst
{
    internal sealed class UpdateSolutionBrowserMobileFirstHandler : IRequestHandler<UpdateSolutionBrowserMobileFirstCommand, UpdateSolutionBrowserMobileFirstValidationResult>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        private readonly UpdateSolutionBrowserMobileFirstValidator _updateSolutionBrowserMobileFirstValidator;

        public UpdateSolutionBrowserMobileFirstHandler(ClientApplicationPartialUpdater clientApplicationPartialUpdater, UpdateSolutionBrowserMobileFirstValidator updateSolutionBrowserMobileFirstValidator)
        {
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;
            _updateSolutionBrowserMobileFirstValidator = updateSolutionBrowserMobileFirstValidator;
        }

        public async Task<UpdateSolutionBrowserMobileFirstValidationResult> Handle(UpdateSolutionBrowserMobileFirstCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = _updateSolutionBrowserMobileFirstValidator.Validation(request.UpdateSolutionBrowserMobileFirstViewModel);

            if (validationResult.IsValid)
            {
                await _clientApplicationPartialUpdater.UpdateAsync(request.SolutionId, clientApplication =>
                    {
                        clientApplication.MobileFirstDesign =
                            request.UpdateSolutionBrowserMobileFirstViewModel?.MobileFirstDesign.ToBoolean(); 
                    },
                    cancellationToken).ConfigureAwait(false);
            }

            return validationResult;
        }
    }
}
