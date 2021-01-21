using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateClaimedEpics;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Epics;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal sealed class ClaimedEpicsVerifier : IVerifier<UpdateClaimedEpicsCommand, ISimpleResult>
    {
        private readonly IEpicRepository epicRepository;
        private readonly ISolutionEpicStatusRepository solutionEpicStatusRepository;
        private readonly VerifyEpicsResult verifyEpicsResult;

        public ClaimedEpicsVerifier(IEpicRepository epicRepository, ISolutionEpicStatusRepository solutionEpicStatusRepository)
        {
            this.epicRepository = epicRepository;
            this.solutionEpicStatusRepository = solutionEpicStatusRepository;
            verifyEpicsResult = new VerifyEpicsResult();
        }

        public static bool CheckNoDuplicateEpicIds(IEnumerable<string> epicIds)
        {
            return epicIds.GroupBy(x => x).Any(c => c.Count() > 1);
        }

        public async Task<ISimpleResult> VerifyAsync(UpdateClaimedEpicsCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var epics = command.Data.Select(x => new ClaimedEpic(x.EpicId, x.StatusName)).ToHashSet();

            if (CheckNoDuplicateEpicIds(epics.Select(x => x.EpicId)))
            {
                verifyEpicsResult.InvalidEpicsList.Add("epics");
                return verifyEpicsResult;
            }

            var result = await CheckEpicsInformationExist(
                command.Data.Select(x => new ClaimedEpic(x.EpicId, x.StatusName)).ToHashSet(),
                CancellationToken.None);

            if (!result)
            {
                verifyEpicsResult.InvalidEpicsList.Add("epics");
            }

            return verifyEpicsResult;
        }

        public async Task<bool> CheckEpicsInformationExist(ISet<ClaimedEpic> epics, CancellationToken cancellationToken)
        {
            var epicIdsCount = await CheckEpicIdsExist(epics.Select(x => x.EpicId).ToHashSet(), cancellationToken);

            var doAllStatusExist = await CheckStatusNamesExist(epics.Select(x => x.StatusName).ToHashSet(), cancellationToken);

            return epicIdsCount == epics.ToList().Count && doAllStatusExist;
        }

        private async Task<int> CheckEpicIdsExist(IEnumerable<string> epicIds, CancellationToken cancellationToken) =>
            await epicRepository.CountMatchingEpicIdsAsync(epicIds, cancellationToken);

        private async Task<bool> CheckStatusNamesExist(IEnumerable<string> statusNames, CancellationToken cancellationToken)
        {
            IEnumerable<string> uniqueStatusNameList = statusNames.Distinct().ToList();

            var statusNameCount = await solutionEpicStatusRepository.CountMatchingEpicStatusAsync(
                uniqueStatusNameList,
                cancellationToken);

            return statusNameCount == uniqueStatusNameList.Count();
        }
    }
}
