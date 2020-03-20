using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Epics;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence.Epics;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateClaimedEpics
{
    internal sealed class UpdateClaimedEpicsExecutor : IExecutor<UpdateClaimedEpicsCommand>
    {
        private readonly SolutionEpicsUpdater _updater;
        private readonly SolutionVerifier _verifier;

        public UpdateClaimedEpicsExecutor(SolutionVerifier verifier, SolutionEpicsUpdater updater)
        {
            _verifier = verifier;
            _updater = updater;
        }

        public async Task UpdateAsync(UpdateClaimedEpicsCommand request, CancellationToken cancellationToken)
        {
            await _verifier.ThrowWhenMissingAsync(request.SolutionId, cancellationToken).ConfigureAwait(false);
            await _updater.UpdateAsync(request.SolutionId,
                    request.Data.Select(x => new ClaimedEpic(x.EpicId, x.StatusName)).ToHashSet(), cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
