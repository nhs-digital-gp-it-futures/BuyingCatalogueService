using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionNativeDesktopOperatingSystems
{
    internal sealed class UpdateSolutionNativeDesktopOperatingSystemsExecutor : IExecutor<UpdateSolutionNativeDesktopOperatingSystemsCommand>
    {
        private readonly ClientApplicationPartialUpdater clientApplicationPartialUpdater;

        public UpdateSolutionNativeDesktopOperatingSystemsExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            this.clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateSolutionNativeDesktopOperatingSystemsCommand request, CancellationToken cancellationToken) =>
            await clientApplicationPartialUpdater.UpdateAsync(
                request.SolutionId,
                clientApplication =>
                {
                    clientApplication.NativeDesktopOperatingSystemsDescription = request.NativeDesktopOperatingSystemsDescription;
                },
                cancellationToken);
    }
}
