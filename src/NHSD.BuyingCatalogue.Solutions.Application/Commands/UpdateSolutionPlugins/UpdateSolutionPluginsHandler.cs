using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionPlugins
{
    internal sealed class UpdateSolutionPluginsHandler : IRequestHandler<UpdateSolutionPluginsCommand, RequiredMaxLengthResult>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        private readonly UpdateSolutionPluginsValidator _updateSolutionPluginsValidator;

        public UpdateSolutionPluginsHandler(ClientApplicationPartialUpdater clientApplicationPartialUpdater, UpdateSolutionPluginsValidator updateSolutionPluginsValidator)
        {
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;
            _updateSolutionPluginsValidator = updateSolutionPluginsValidator;
        }

        public async Task<RequiredMaxLengthResult> Handle(UpdateSolutionPluginsCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = _updateSolutionPluginsValidator.Validation(request.UpdateSolutionPluginsViewModel);

            if (validationResult.IsValid)
            {
                await _clientApplicationPartialUpdater.UpdateAsync(request.SolutionId, clientApplication =>
                        {
                            clientApplication.Plugins = new Plugins()
                            {
                                Required = request.UpdateSolutionPluginsViewModel.Required.ToBoolean(),
                                AdditionalInformation = request.UpdateSolutionPluginsViewModel.AdditionalInformation
                            };
                        },
                    cancellationToken).ConfigureAwait(false);
            }

            return validationResult;
        }
    }
}
