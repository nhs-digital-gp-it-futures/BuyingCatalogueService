using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateCapabilities
{
    internal sealed class UpdateCapabilitiesExecutor : IExecutor<UpdateCapabilitiesCommand>
    {
        private readonly SolutionVerifier verifier;
        private readonly SolutionCapabilitiesUpdater updater;

        public UpdateCapabilitiesExecutor(SolutionVerifier verifier, SolutionCapabilitiesUpdater updater)
        {
            this.verifier = verifier;
            this.updater = updater;
        }

        public async Task UpdateAsync(UpdateCapabilitiesCommand request, CancellationToken cancellationToken)
        {
            await verifier.ThrowWhenMissingAsync(request.SolutionId, cancellationToken);
            await updater.UpdateAsync(request.SolutionId, request.NewCapabilitiesReferences, cancellationToken);
        }
    }
}
