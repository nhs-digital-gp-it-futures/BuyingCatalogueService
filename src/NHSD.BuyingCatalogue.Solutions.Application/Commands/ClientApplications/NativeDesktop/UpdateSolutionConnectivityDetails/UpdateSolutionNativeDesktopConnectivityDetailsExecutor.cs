using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionConnectivityDetails
{
    internal sealed class UpdateSolutionNativeDesktopConnectivityDetailsExecutor : IExecutor<UpdateSolutionNativeDesktopConnectivityDetailsCommand>
    {
        private readonly ClientApplicationPartialUpdater clientApplicationPartialUpdater;

        public UpdateSolutionNativeDesktopConnectivityDetailsExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            this.clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateSolutionNativeDesktopConnectivityDetailsCommand request, CancellationToken cancellationToken) =>
            await clientApplicationPartialUpdater.UpdateAsync(
                request.SolutionId,
                clientApplication =>
                {
                    clientApplication.NativeDesktopMinimumConnectionSpeed = request.NativeDesktopMinimumConnectionSpeed;
                },
                cancellationToken);
    }
}
