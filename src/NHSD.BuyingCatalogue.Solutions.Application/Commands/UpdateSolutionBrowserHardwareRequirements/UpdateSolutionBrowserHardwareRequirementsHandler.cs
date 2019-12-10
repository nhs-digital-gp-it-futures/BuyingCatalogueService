using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserHardwareRequirements
{
    internal sealed class UpdateSolutionBrowserHardwareRequirementsHandler : IRequestHandler<UpdateSolutionBrowserHardwareRequirementsCommand, UpdateSolutionBrowserHardwareRequirementsValidationResult>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        private readonly UpdateSolutionBrowserHardwareRequirementsValidator _updateSolutionHardwareRequirementsValidator;

        public UpdateSolutionBrowserHardwareRequirementsHandler(ClientApplicationPartialUpdater clientApplicationPartialUpdater, UpdateSolutionBrowserHardwareRequirementsValidator updateSolutionHardwareRequirementsValidator)
        {
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;
            _updateSolutionHardwareRequirementsValidator = updateSolutionHardwareRequirementsValidator;
        }

        public async Task<UpdateSolutionBrowserHardwareRequirementsValidationResult> Handle(UpdateSolutionBrowserHardwareRequirementsCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = _updateSolutionHardwareRequirementsValidator.Validation(request.UpdateSolutionHardwareRequirementsViewModel);

            if (validationResult.IsValid)
            {
                await _clientApplicationPartialUpdater.UpdateAsync(request.SolutionId, clientApplication =>
                    {
                        clientApplication.HardwareRequirements =
                            request.UpdateSolutionHardwareRequirementsViewModel.HardwareRequirements;
                    },
                    cancellationToken).ConfigureAwait(false);
            }

            return validationResult;
        }
    }
}
