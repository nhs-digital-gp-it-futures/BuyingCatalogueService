using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileConnectionDetails
{
    internal sealed class UpdateSolutionMobileConnectionDetailsExecutor : IExecutor<UpdateSolutionMobileConnectionDetailsCommand>
    {
        private readonly ClientApplicationPartialUpdater updater;

        public UpdateSolutionMobileConnectionDetailsExecutor(ClientApplicationPartialUpdater updater) => this.updater = updater;

        public async Task UpdateAsync(UpdateSolutionMobileConnectionDetailsCommand request, CancellationToken cancellationToken) =>
            await updater.UpdateAsync(
                request.SolutionId,
                application =>
                {
                    application.MobileConnectionDetails = new MobileConnectionDetails
                    {
                        MinimumConnectionSpeed = request.Data.MinimumConnectionSpeed,
                        Description = request.Data.ConnectionRequirementsDescription,
                        ConnectionType = request.Data.ConnectionType,
                    };
                },
                cancellationToken);
    }
}
