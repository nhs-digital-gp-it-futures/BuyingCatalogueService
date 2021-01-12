using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileConnectionDetails
{
    internal sealed class UpdateSolutionMobileConnectionDetailsExecutor : IExecutor<UpdateSolutionMobileConnectionDetailsCommand>
    {
        private readonly ClientApplicationPartialUpdater _updater;

        public UpdateSolutionMobileConnectionDetailsExecutor(ClientApplicationPartialUpdater updater) => _updater = updater;

        public async Task UpdateAsync(UpdateSolutionMobileConnectionDetailsCommand request, CancellationToken cancellationToken) =>
            await _updater.UpdateAsync(request.SolutionId, application =>
                    {
                        application.MobileConnectionDetails = new MobileConnectionDetails
                        {
                            MinimumConnectionSpeed = request.Data.MinimumConnectionSpeed,
                            Description = request.Data.ConnectionRequirementsDescription,
                            ConnectionType = request.Data.ConnectionType,
                        };
                    },
                    cancellationToken)
                .ConfigureAwait(false);
    }
}
