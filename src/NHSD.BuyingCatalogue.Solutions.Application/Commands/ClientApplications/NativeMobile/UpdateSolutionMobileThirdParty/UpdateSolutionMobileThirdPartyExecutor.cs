using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileThirdParty
{
    internal sealed class UpdateSolutionMobileThirdPartyExecutor : IExecutor<UpdateSolutionMobileThirdPartyCommand>
    {
        private readonly ClientApplicationPartialUpdater clientApplicationPartialUpdater;

        public UpdateSolutionMobileThirdPartyExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            this.clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateSolutionMobileThirdPartyCommand request, CancellationToken cancellationToken) =>
            await clientApplicationPartialUpdater.UpdateAsync(
                request.Id,
                clientApplication =>
                {
                    clientApplication.MobileThirdParty = new MobileThirdParty
                    {
                        ThirdPartyComponents = request.Data.ThirdPartyComponents,
                        DeviceCapabilities = request.Data.DeviceCapabilities,
                    };
                },
                cancellationToken);
    }
}
