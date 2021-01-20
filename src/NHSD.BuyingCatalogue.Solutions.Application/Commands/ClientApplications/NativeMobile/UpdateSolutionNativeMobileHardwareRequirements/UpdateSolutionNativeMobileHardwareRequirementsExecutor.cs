using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileHardwareRequirements
{
    internal sealed class UpdateSolutionNativeMobileHardwareRequirementsExecutor : IExecutor<UpdateSolutionNativeMobileHardwareRequirementsCommand>
    {
        private readonly ClientApplicationPartialUpdater clientApplicationPartialUpdater;

        public UpdateSolutionNativeMobileHardwareRequirementsExecutor(ClientApplicationPartialUpdater clientApplicationPartialUpdater) =>
            this.clientApplicationPartialUpdater = clientApplicationPartialUpdater;

        public async Task UpdateAsync(UpdateSolutionNativeMobileHardwareRequirementsCommand request, CancellationToken cancellationToken) =>
            await clientApplicationPartialUpdater.UpdateAsync(
                request.SolutionId,
                clientApplication =>
                {
                    clientApplication.NativeMobileHardwareRequirements = request.HardwareRequirements;
                },
                cancellationToken);
    }
}
