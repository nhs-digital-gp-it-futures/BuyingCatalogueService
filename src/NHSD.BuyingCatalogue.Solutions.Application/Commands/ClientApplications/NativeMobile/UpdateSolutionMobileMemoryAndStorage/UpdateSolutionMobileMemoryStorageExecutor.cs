using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileMemoryAndStorage
{
    internal sealed class UpdateSolutionMobileMemoryStorageExecutor : IExecutor<UpdateSolutionMobileMemoryStorageCommand>
    {
        private readonly ClientApplicationPartialUpdater clientApplicationPartialUpdater;

        public UpdateSolutionMobileMemoryStorageExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            this.clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateSolutionMobileMemoryStorageCommand request, CancellationToken cancellationToken) =>
            await clientApplicationPartialUpdater.UpdateAsync(
                request.Id,
                clientApplication =>
                {
                    clientApplication.MobileMemoryAndStorage = new MobileMemoryAndStorage()
                    {
                        MinimumMemoryRequirement = request.MinimumMemoryRequirement,
                        Description = request.Description,
                    };
                },
                cancellationToken);
    }
}
