using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionNativeDesktopThirdParty
{
    internal sealed class UpdateSolutionNativeDesktopThirdPartyExecutor : IExecutor<UpdateSolutionNativeDesktopThirdPartyCommand>
    {
        private readonly ClientApplicationPartialUpdater clientApplicationPartialUpdater;

        public UpdateSolutionNativeDesktopThirdPartyExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            this.clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateSolutionNativeDesktopThirdPartyCommand request, CancellationToken cancellationToken) =>
            await clientApplicationPartialUpdater.UpdateAsync(
                request.SolutionId,
                clientApplication =>
                {
                    clientApplication.NativeDesktopThirdParty = new NativeDesktopThirdParty
                    {
                        ThirdPartyComponents = request.Data.ThirdPartyComponents,
                        DeviceCapabilities = request.Data.DeviceCapabilities,
                    };
                },
                cancellationToken);
    }
}
