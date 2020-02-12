using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Epics;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence.Epics
{
    internal sealed class SolutionEpicsUpdater
    {
        private readonly ISolutionEpicsRepository _solutionEpicsRepository;

        public SolutionEpicsUpdater(ISolutionEpicsRepository solutionEpicsRepository) =>
            _solutionEpicsRepository = solutionEpicsRepository;

        public async Task UpdateAsync(string solutionId, ISet<ClaimedEpic> epics, CancellationToken cancellationToken)
        {
            await _solutionEpicsRepository
                .UpdateSolutionEpicAsync(solutionId, new UpdateClaimedEpicRequest(epics.Select(x => new ClaimedEpicResult(x.EpicId, x.StatusName))), cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
