using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateNativeDesktopMemoryAndStorage
{
    class UpdateNativeDesktopMemoryAndStorageExecutor : IExecutor<UpdateNativeDesktopMemoryAndStorageCommand>
    {
        private readonly ClientApplicationPartialUpdater _partialUpdater;

        public UpdateNativeDesktopMemoryAndStorageExecutor(ClientApplicationPartialUpdater partialUpdater) =>
            _partialUpdater = partialUpdater;

        public async Task UpdateAsync(UpdateNativeDesktopMemoryAndStorageCommand request, CancellationToken cancellationToken) =>
            await _partialUpdater.UpdateAsync(request.SolutionId, clientApplication =>
                clientApplication.NativeDesktopMemoryAndStorage = new NativeDesktopMemoryAndStorage
                {
                    MinimumMemoryRequirement = request.Data.MinimumMemoryRequirement,
                    StorageRequirementsDescription = request.Data.StorageRequirementsDescription,
                    MinimumCpu = request.Data.MinimumCpu,
                    RecommendedResolution = request.Data.RecommendedResolution
                },cancellationToken).ConfigureAwait(false);
    }
}
