using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserAdditionalInformation
{
    internal sealed class UpdateSolutionBrowserAdditionalInformationExecutor : IExecutor<UpdateSolutionBrowserAdditionalInformationCommand>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        public UpdateSolutionBrowserAdditionalInformationExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateSolutionBrowserAdditionalInformationCommand request, CancellationToken cancellationToken) =>
            await _clientApplicationPartialUpdater.UpdateAsync(request.SolutionId, clientApplication =>
                {
                    clientApplication.AdditionalInformation =
                        request.UpdateSolutionBrowserAdditionalInformationViewModel.AdditionalInformation;
                },
                cancellationToken).ConfigureAwait(false);
    }
}
