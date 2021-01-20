using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadMap
{
    internal sealed class UpdateRoadMapExecutor : IExecutor<UpdateRoadMapCommand>
    {
        private readonly SolutionVerifier verifier;
        private readonly SolutionRoadMapUpdater updater;

        public UpdateRoadMapExecutor(SolutionVerifier verifier, SolutionRoadMapUpdater updater)
        {
            this.verifier = verifier;
            this.updater = updater;
        }

        public async Task UpdateAsync(UpdateRoadMapCommand request, CancellationToken cancellationToken)
        {
            await verifier.ThrowWhenMissingAsync(request.SolutionId, cancellationToken);
            await updater.Update(request.SolutionId, request.Summary, cancellationToken);
        }
    }
}
