using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileThirdParty
{
    internal class UpdateSolutionMobileThirdPartyExecutor : IExecutor<UpdateSolutionMobileThirdPartyCommand>
    {
        private readonly ClientApplicationPartialUpdater _clientApplicationPartialUpdater;

        public UpdateSolutionMobileThirdPartyExecutor(
            ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            _clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateSolutionMobileThirdPartyCommand request, CancellationToken cancellationToken) =>
            await _clientApplicationPartialUpdater.UpdateAsync(request.Id,
                clientApplication =>
                {
                    clientApplication.MobileThirdParty = new MobileThirdParty()
                    {
                        ThirdPartyComponents = request.ViewModel.ThirdPartyComponents,
                        DeviceCapabilities = request.ViewModel.DeviceCapabilities
                    };
                }, cancellationToken).ConfigureAwait(false);
    }
}
