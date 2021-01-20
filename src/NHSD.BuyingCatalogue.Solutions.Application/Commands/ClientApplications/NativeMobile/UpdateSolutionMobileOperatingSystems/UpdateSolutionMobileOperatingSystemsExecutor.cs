using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileOperatingSystems
{
    internal sealed class UpdateSolutionMobileOperatingSystemsExecutor : IExecutor<UpdateSolutionMobileOperatingSystemsCommand>
    {
        private readonly ClientApplicationPartialUpdater clientApplicationPartialUpdater;

        public UpdateSolutionMobileOperatingSystemsExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            this.clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateSolutionMobileOperatingSystemsCommand request, CancellationToken cancellationToken) =>
            await clientApplicationPartialUpdater.UpdateAsync(
                request.Id,
                clientApplication =>
                {
                    clientApplication.MobileOperatingSystems = new MobileOperatingSystems
                    {
                        OperatingSystems = request.Data.OperatingSystems,
                        OperatingSystemsDescription = request.Data.OperatingSystemsDescription,
                    };
                },
                cancellationToken);
    }
}
