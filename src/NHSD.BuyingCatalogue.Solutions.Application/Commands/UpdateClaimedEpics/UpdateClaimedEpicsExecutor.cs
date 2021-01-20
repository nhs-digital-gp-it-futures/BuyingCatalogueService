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
        private readonly SolutionEpicsUpdater updater;
        private readonly SolutionVerifier verifier;

        public UpdateClaimedEpicsExecutor(SolutionVerifier verifier, SolutionEpicsUpdater updater)
        {
            this.verifier = verifier;
            this.updater = updater;
        }

        public async Task UpdateAsync(UpdateClaimedEpicsCommand request, CancellationToken cancellationToken)
        {
            await verifier.ThrowWhenMissingAsync(request.SolutionId, cancellationToken);
            await updater.UpdateAsync(
                request.SolutionId,
                request.Data.Select(x => new ClaimedEpic(x.EpicId, x.StatusName)).ToHashSet(),
                cancellationToken);
        }
    }
}
