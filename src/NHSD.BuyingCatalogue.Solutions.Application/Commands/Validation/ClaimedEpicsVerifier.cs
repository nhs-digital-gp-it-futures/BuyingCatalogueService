using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateClaimedEpics;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Epics;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal sealed class ClaimedEpicsVerifier : IVerifier<UpdateClaimedEpicsCommand, ISimpleResult>
    {
        private readonly IEpicRepository _epicRepository;
        private readonly ISolutionEpicStatusRepository _solutionEpicStatusRepository;
        private readonly VerifyEpicsResult _verifyEpicsResult;

        public ClaimedEpicsVerifier(IEpicRepository epicRepository, ISolutionEpicStatusRepository solutionEpicStatusRepository)
        {
            _epicRepository = epicRepository;
            _solutionEpicStatusRepository = solutionEpicStatusRepository;
            _verifyEpicsResult = new VerifyEpicsResult();
        }

        public async Task<ISimpleResult> VerifyAsync(UpdateClaimedEpicsCommand command)
        {
            command = command.ThrowIfNull(nameof(command));

            var epics = command.Data.Select(x => new ClaimedEpic(x.EpicId, x.StatusName)).ToHashSet();

            if (CheckNoDuplicateEpicIds(epics.Select(x => x.EpicId)))
            {
                _verifyEpicsResult.InvalidEpicsList.Add("epics");
                return _verifyEpicsResult;
            }

            var result =
                await CheckEpicsInformationExist(command.Data.Select(x => new ClaimedEpic(x.EpicId, x.StatusName)).ToHashSet(),
                        new CancellationToken())
                    .ConfigureAwait(false);

            if (!result)
            {
                _verifyEpicsResult.InvalidEpicsList.Add("epics");
            }

            return _verifyEpicsResult;
        }

        public async Task<bool> CheckEpicsInformationExist(ISet<ClaimedEpic> epics, CancellationToken cancellationToken)
        {
            var epicIdsCount = await CheckEpicIdsExist(epics.Select(x => x.EpicId).ToHashSet(), cancellationToken)
                .ConfigureAwait(false);

            var doAllStatusExist =
                await CheckStatusNamesExist(epics.Select(x => x.StatusName).ToHashSet(), cancellationToken)
                    .ConfigureAwait(false);

            return epicIdsCount == epics.ToList().Count && doAllStatusExist;
        }

        private async Task<int> CheckEpicIdsExist(ISet<string> epicIds, CancellationToken cancellationToken) =>
            await _epicRepository
                .CountMatchingEpicIdsAsync(epicIds, cancellationToken).ConfigureAwait(false);

        private async Task<bool> CheckStatusNamesExist(ISet<string> statusNames, CancellationToken cancellationToken)
        {
            IEnumerable<string> uniqueStatusNameList = statusNames.Distinct().ToList();

            var statusNameCount = await _solutionEpicStatusRepository
                .CountMatchingEpicStatusAsync(uniqueStatusNameList, cancellationToken).ConfigureAwait(false);

            return statusNameCount == uniqueStatusNameList.Count();
        }

        public bool CheckNoDuplicateEpicIds(IEnumerable<string> epicIds)
        {
            return epicIds.GroupBy(x => x).Any(c => c.Count() > 1);
        }
    }
}
