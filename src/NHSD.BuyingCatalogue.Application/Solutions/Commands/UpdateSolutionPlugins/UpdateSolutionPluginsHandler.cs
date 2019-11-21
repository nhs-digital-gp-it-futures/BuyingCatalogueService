using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;
using NHSD.BuyingCatalogue.Application.Solutions.Persistence;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionPlugins
{
    internal sealed class UpdateSolutionPluginsHandler : IRequestHandler<UpdateSolutionPluginsCommand, UpdateSolutionPluginsValidationResult>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        private readonly UpdateSolutionPluginsValidator _updateSolutionPluginsValidator;

        public UpdateSolutionPluginsHandler(ClientApplicationPartialUpdater clientApplicationPartialUpdater, UpdateSolutionPluginsValidator updateSolutionPluginsValidator)
        {
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;
            _updateSolutionPluginsValidator = updateSolutionPluginsValidator;
        }

        public async Task<UpdateSolutionPluginsValidationResult> Handle(UpdateSolutionPluginsCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = _updateSolutionPluginsValidator.Validation(request.UpdateSolutionPluginsViewModel);

            if (validationResult.IsValid)
            {
                await _clientApplicationPartialUpdater.UpdateAsync(request.SolutionId, clientApplication =>
                {
                    clientApplication.Plugins = new Plugins()
                    {
                        Required = MapRequired(request.UpdateSolutionPluginsViewModel.Required),
                        AdditionalInformation = request.UpdateSolutionPluginsViewModel.AdditionalInformation
                    };
                },
                    cancellationToken);
            }

            return validationResult;
        }

        private bool? MapRequired(string required) => required == "yes" ? true : (required == "no" ? false : (bool?)null);

    }
}
