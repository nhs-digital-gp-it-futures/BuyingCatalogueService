using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileAdditionalInformation
{
    internal sealed class UpdateSolutionNativeMobileAdditionalInformationExecutor : IExecutor<UpdateSolutionNativeMobileAdditionalInformationCommand>
    {
        private readonly ClientApplicationPartialUpdater clientApplicationPartialUpdater;

        public UpdateSolutionNativeMobileAdditionalInformationExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            this.clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateSolutionNativeMobileAdditionalInformationCommand request, CancellationToken cancellationToken) =>
            await clientApplicationPartialUpdater.UpdateAsync(
                request.SolutionId,
                clientApplication =>
                {
                    clientApplication.NativeMobileAdditionalInformation = request.AdditionalInformation;
                },
                cancellationToken);
    }
}
