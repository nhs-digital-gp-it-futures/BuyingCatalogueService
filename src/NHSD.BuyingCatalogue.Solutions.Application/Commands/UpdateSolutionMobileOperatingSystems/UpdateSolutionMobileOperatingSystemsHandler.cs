using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileOperatingSystems
{
    internal class UpdateSolutionMobileOperatingSystemsHandler : IRequestHandler<UpdateSolutionMobileOperatingSystemsCommand, RequiredMaxLengthResult>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;
        private readonly UpdateSolutionMobileOperatingSystemsValidator _updateSolutionMobileOperatingSystemsValidator;

        public UpdateSolutionMobileOperatingSystemsHandler(ClientApplicationPartialUpdater clientApplicationPartialUpdater, UpdateSolutionMobileOperatingSystemsValidator updateSolutionMobileOperatingSystemsValidator)
        {
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;
            _updateSolutionMobileOperatingSystemsValidator = updateSolutionMobileOperatingSystemsValidator;
        }

        public async Task<RequiredMaxLengthResult> Handle(UpdateSolutionMobileOperatingSystemsCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _updateSolutionMobileOperatingSystemsValidator.Validation(request.ViewModel);

            if (validationResult.IsValid)
            {
                await _clientApplicationPartialUpdater.UpdateAsync(request.Id,
                    clientApplication =>
                    {
                        clientApplication.MobileOperatingSystems = new MobileOperatingSystems()
                        {
                            OperatingSystems = request.ViewModel.OperatingSystems,
                            OperatingSystemsDescription = request.ViewModel.OperatingSystemsDescription
                        };
                    }, cancellationToken).ConfigureAwait(false);
            }

            return validationResult;
        }
    }
}
