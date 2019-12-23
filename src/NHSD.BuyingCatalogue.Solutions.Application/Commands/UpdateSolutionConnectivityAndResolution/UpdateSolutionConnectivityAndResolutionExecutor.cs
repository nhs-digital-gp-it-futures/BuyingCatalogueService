using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionConnectivityAndResolution
{
    internal class UpdateSolutionConnectivityAndResolutionExecutor : IExecutor<UpdateSolutionConnectivityAndResolutionCommand>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        public UpdateSolutionConnectivityAndResolutionExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateSolutionConnectivityAndResolutionCommand request, CancellationToken cancellationToken) =>
            await _clientApplicationPartialUpdater.UpdateAsync(request.Id,
                    clientApplication =>
                    {
                        clientApplication.MinimumConnectionSpeed = request.ViewModel.MinimumConnectionSpeed;
                        clientApplication.MinimumDesktopResolution = request.ViewModel.MinimumDesktopResolution;
                    },
                    cancellationToken)
                .ConfigureAwait(false);
    }
}
