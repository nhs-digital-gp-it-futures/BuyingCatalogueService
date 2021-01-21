using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopMemoryAndStorage
{
    internal sealed class UpdateNativeDesktopMemoryAndStorageExecutor : IExecutor<UpdateNativeDesktopMemoryAndStorageCommand>
    {
        private readonly ClientApplicationPartialUpdater partialUpdater;

        public UpdateNativeDesktopMemoryAndStorageExecutor(ClientApplicationPartialUpdater partialUpdater) =>
            this.partialUpdater = partialUpdater;

        public async Task UpdateAsync(UpdateNativeDesktopMemoryAndStorageCommand request, CancellationToken cancellationToken) =>
            await partialUpdater.UpdateAsync(
                request.SolutionId,
                clientApplication =>
                clientApplication.NativeDesktopMemoryAndStorage = new NativeDesktopMemoryAndStorage
                {
                    MinimumMemoryRequirement = request.Data.MinimumMemoryRequirement,
                    StorageRequirementsDescription = request.Data.StorageRequirementsDescription,
                    MinimumCpu = request.Data.MinimumCpu,
                    RecommendedResolution = request.Data.RecommendedResolution,
                },
                cancellationToken);
    }
}
