﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Epics;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence.Epics
{
    internal sealed class SolutionEpicsUpdater
    {
        private readonly ISolutionEpicRepository solutionEpicRepository;

        public SolutionEpicsUpdater(ISolutionEpicRepository solutionEpicRepository) =>
            this.solutionEpicRepository = solutionEpicRepository;

        public async Task UpdateAsync(string solutionId, ISet<ClaimedEpic> epics, CancellationToken cancellationToken)
        {
            await solutionEpicRepository.UpdateSolutionEpicAsync(
                solutionId,
                new UpdateClaimedEpicListRequest(epics.Select(x => new ClaimedEpicResult(x.EpicId, x.StatusName))),
                cancellationToken);
        }
    }
}
