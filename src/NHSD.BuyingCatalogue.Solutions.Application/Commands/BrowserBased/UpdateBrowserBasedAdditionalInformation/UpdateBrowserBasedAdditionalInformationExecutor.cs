using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateBrowserBasedAdditionalInformation
{
    internal sealed class UpdateBrowserBasedAdditionalInformationExecutor : IExecutor<UpdateBrowserBasedAdditionalInformationCommand>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        public UpdateBrowserBasedAdditionalInformationExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateBrowserBasedAdditionalInformationCommand request, CancellationToken cancellationToken) =>
            await _clientApplicationPartialUpdater.UpdateAsync(request.SolutionId, clientApplication =>
                {
                    clientApplication.AdditionalInformation = request.AdditionalInformation;
                },
                cancellationToken).ConfigureAwait(false);
    }
}
