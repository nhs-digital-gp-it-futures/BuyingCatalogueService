using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateIntegrations
{
    internal sealed class UpdateIntegrationsExecutor : IExecutor<UpdateIntegrationsCommand>
    {
        private readonly SolutionVerifier _verifier;
        private readonly SolutionIntegrationsUpdater _updater;

        public UpdateIntegrationsExecutor(SolutionVerifier verifier, SolutionIntegrationsUpdater updater)
        {
            _verifier = verifier;
            _updater = updater;
        }

        public async Task UpdateAsync(UpdateIntegrationsCommand request, CancellationToken cancellationToken)
        {
            await _verifier.ThrowWhenMissing(request.SolutionId, cancellationToken).ConfigureAwait(false);
            await _updater.Update(request.SolutionId, request.Url, cancellationToken).ConfigureAwait(false);
        }
    }
}
