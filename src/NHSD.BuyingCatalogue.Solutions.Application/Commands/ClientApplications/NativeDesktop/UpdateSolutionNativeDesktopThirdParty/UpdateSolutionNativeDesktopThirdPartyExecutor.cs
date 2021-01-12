using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionNativeDesktopThirdParty
{
    internal sealed class UpdateSolutionNativeDesktopThirdPartyExecutor : IExecutor<UpdateSolutionNativeDesktopThirdPartyCommand>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        public UpdateSolutionNativeDesktopThirdPartyExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateSolutionNativeDesktopThirdPartyCommand request, CancellationToken cancellationToken) =>
            await _clientApplicationPartialUpdater.UpdateAsync(request.SolutionId, clientApplication =>
                {
                    clientApplication.NativeDesktopThirdParty = new NativeDesktopThirdParty
                    {
                        ThirdPartyComponents = request.Data.ThirdPartyComponents,
                        DeviceCapabilities = request.Data.DeviceCapabilities,
                    };
                },
                cancellationToken).ConfigureAwait(false);
    }
}
