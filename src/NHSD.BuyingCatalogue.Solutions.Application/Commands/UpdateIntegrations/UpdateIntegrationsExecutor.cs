using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateIntegrations
{
    internal sealed class UpdateIntegrationsExecutor : IExecutor<UpdateIntegrationsCommand>
    {
        private readonly SolutionVerifier verifier;
        private readonly SolutionIntegrationsUpdater updater;

        public UpdateIntegrationsExecutor(SolutionVerifier verifier, SolutionIntegrationsUpdater updater)
        {
            this.verifier = verifier;
            this.updater = updater;
        }

        public async Task UpdateAsync(UpdateIntegrationsCommand request, CancellationToken cancellationToken)
        {
            await verifier.ThrowWhenMissingAsync(request.SolutionId, cancellationToken);
            await updater.Update(request.SolutionId, request.Url, cancellationToken);
        }
    }
}
