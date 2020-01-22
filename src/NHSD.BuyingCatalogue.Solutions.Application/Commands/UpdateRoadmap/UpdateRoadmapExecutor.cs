using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateRoadmap
{
    internal sealed class UpdateRoadmapExecutor : IExecutor<UpdateRoadmapCommand>
    {
        private readonly SolutionVerifier _verifier;
        private readonly SolutionRoadmapUpdater _updater;

        public UpdateRoadmapExecutor(SolutionVerifier verifier, SolutionRoadmapUpdater updater)
        {
            _verifier = verifier;
            _updater = updater;
        }

        public async Task UpdateAsync(UpdateRoadmapCommand request, CancellationToken cancellationToken)
        {
            await _verifier.ThrowWhenMissing(request.SolutionId, cancellationToken).ConfigureAwait(false);
            await _updater.Update(request.SolutionId, request.Description, cancellationToken).ConfigureAwait(false);
        }
    }
}
