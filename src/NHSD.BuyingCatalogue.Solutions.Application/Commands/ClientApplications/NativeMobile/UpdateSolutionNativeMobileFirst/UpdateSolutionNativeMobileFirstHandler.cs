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
        private readonly ClientApplicationPartialUpdater clientApplicationPartialUpdater;
        private readonly UpdateSolutionNativeMobileFirstValidator updateSolutionNativeMobileFirstValidator;

        public UpdateSolutionNativeMobileFirstHandler(
            ClientApplicationPartialUpdater clientApplicationPartialUpdater,
            UpdateSolutionNativeMobileFirstValidator updateSolutionNativeMobileFirstValidator)
        {
            this.clientApplicationPartialUpdater = clientApplicationPartialUpdater;
            this.updateSolutionNativeMobileFirstValidator = updateSolutionNativeMobileFirstValidator;
        }

        public async Task<ISimpleResult> Handle(
            UpdateSolutionNativeMobileFirstCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = updateSolutionNativeMobileFirstValidator.Validate(request);

            if (validationResult.IsValid)
            {
                await clientApplicationPartialUpdater.UpdateAsync(
                    request.SolutionId,
                    clientApplication =>
                    {
                        clientApplication.NativeMobileFirstDesign = request.MobileFirstDesign.ToBoolean();
                    },
                    cancellationToken);
            }

            return validationResult;
        }
    }
}
