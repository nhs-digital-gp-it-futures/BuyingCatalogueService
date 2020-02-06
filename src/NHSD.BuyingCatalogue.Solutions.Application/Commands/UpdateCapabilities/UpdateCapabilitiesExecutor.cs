using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateCapabilities
{
    internal sealed class UpdateCapabilitiesExecutor : IExecutor<UpdateCapabilitiesCommand>
    {
        private readonly SolutionVerifier _verifier;
        private readonly SolutionCapabilitiesUpdater _updater;

        public UpdateCapabilitiesExecutor(SolutionVerifier verifier, SolutionCapabilitiesUpdater updater)
        {
            _verifier = verifier;
            _updater = updater;
        }

        public async Task UpdateAsync(UpdateCapabilitiesCommand request, CancellationToken cancellationToken)
        {
            await _verifier.ThrowWhenMissingAsync(request.SolutionId, cancellationToken).ConfigureAwait(false);
            await _updater.UpdateAsync(request.SolutionId, request.NewCapabilitiesReferences, cancellationToken).ConfigureAwait(false);
        }
    }
}
