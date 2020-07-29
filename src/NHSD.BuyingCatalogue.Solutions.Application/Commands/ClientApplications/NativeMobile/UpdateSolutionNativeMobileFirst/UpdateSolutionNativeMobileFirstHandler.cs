using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileFirst
{
    internal sealed class UpdateSolutionNativeMobileFirstHandler : IRequestHandler<UpdateSolutionNativeMobileFirstCommand, ISimpleResult>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        public UpdateSolutionNativeMobileFirstHandler(ClientApplicationPartialUpdater clientApplicationPartialUpdater)
        {
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;
        }

        public async Task<ISimpleResult> Handle(UpdateSolutionNativeMobileFirstCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = UpdateSolutionNativeMobileFirstValidator.Validation(request);

            if (validationResult.IsValid)
            {
                await _clientApplicationPartialUpdater.UpdateAsync(request.SolutionId, clientApplication =>
                    {
                        clientApplication.NativeMobileFirstDesign = request.MobileFirstDesign.ToBoolean();
                    },
                    cancellationToken).ConfigureAwait(false);
            }

            return validationResult;
        }
    }
}
