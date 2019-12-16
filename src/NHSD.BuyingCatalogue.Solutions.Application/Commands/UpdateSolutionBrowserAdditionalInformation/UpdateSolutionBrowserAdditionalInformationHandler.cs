using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserAdditionalInformation
{
    internal sealed class UpdateSolutionBrowserAdditionalInformationHandler : IRequestHandler<UpdateSolutionBrowserAdditionalInformationCommand, MaxLengthResult>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        private readonly UpdateSolutionBrowserAdditionalInformationValidator _updateSolutionBrowserAdditionalInformationValidator;

        public UpdateSolutionBrowserAdditionalInformationHandler(ClientApplicationPartialUpdater clientApplicationPartialUpdater, UpdateSolutionBrowserAdditionalInformationValidator updatesolutionBrowserAdditionalInformationValidator)
        {
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;
            _updateSolutionBrowserAdditionalInformationValidator = updatesolutionBrowserAdditionalInformationValidator;
        }

        public async Task<MaxLengthResult> Handle(UpdateSolutionBrowserAdditionalInformationCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = _updateSolutionBrowserAdditionalInformationValidator.Validation(request.UpdateSolutionBrowserAdditionalInformationViewModel);

            if (validationResult.IsValid)
            {
                await _clientApplicationPartialUpdater.UpdateAsync(request.SolutionId, clientApplication =>
                    {
                        clientApplication.AdditionalInformation =
                            request.UpdateSolutionBrowserAdditionalInformationViewModel.AdditionalInformation;
                    },
                    cancellationToken).ConfigureAwait(false);
            }

            return validationResult;
        }
    }
}
