using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopHardwareRequirements
{
    internal sealed class UpdateNativeDesktopHardwareRequirementsExecutor : IExecutor<UpdateNativeDesktopHardwareRequirementsCommand>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        public UpdateNativeDesktopHardwareRequirementsExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateNativeDesktopHardwareRequirementsCommand request, CancellationToken cancellationToken) =>
            await _clientApplicationPartialUpdater.UpdateAsync(request.SolutionId, clientApplication =>
                {
                    clientApplication.NativeDesktopHardwareRequirements = request.HardwareRequirements;
                },
                cancellationToken).ConfigureAwait(false);
    }
}
