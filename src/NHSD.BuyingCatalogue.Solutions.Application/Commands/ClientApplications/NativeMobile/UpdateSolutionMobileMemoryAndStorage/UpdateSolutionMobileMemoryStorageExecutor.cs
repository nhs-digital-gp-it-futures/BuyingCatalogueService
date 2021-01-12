using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileMemoryAndStorage
{
    internal class UpdateSolutionMobileMemoryStorageExecutor : IExecutor<UpdateSolutionMobileMemoryStorageCommand>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        public UpdateSolutionMobileMemoryStorageExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateSolutionMobileMemoryStorageCommand request, CancellationToken cancellationToken) =>
            await _clientApplicationPartialUpdater.UpdateAsync(request.Id,
                    clientApplication =>
                    {
                        clientApplication.MobileMemoryAndStorage = new MobileMemoryAndStorage()
                        {
                            MinimumMemoryRequirement = request.MinimumMemoryRequirement,
                            Description = request.Description,
                        };
                    },
                    cancellationToken)
                .ConfigureAwait(false);
    }
}
