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
    internal sealed class EpicsVerifier : IVerifier<UpdateClaimedEpicsCommand, ISimpleResult>
    {
        private readonly IEpicRepository _epicRepository;
        private readonly ISolutionEpicStatusRepository _solutionEpicStatusRepository;
        private readonly VerifyEpicsResult _verifyEpicsResult;

        public EpicsVerifier(IEpicRepository epicRepository, ISolutionEpicStatusRepository solutionEpicStatusRepository)
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
                _verifyEpicsResult.ValidEpicsList.Add("epics");
                return _verifyEpicsResult;
            }

            var result =
                await CheckEpicsInformationExist(command.Data.Select(x => new ClaimedEpic(x.EpicId, x.StatusName)).ToHashSet(),
                        new CancellationToken())
                    .ConfigureAwait(false);

            if (!result)
            {
                _verifyEpicsResult.ValidEpicsList.Add("epics");
            }

            return _verifyEpicsResult;
        }

        public async Task<bool> CheckEpicsInformationExist(ISet<ClaimedEpic> epics, CancellationToken cancellationToken)
        {
            var epicIdsCount = await _epicRepository
                .GetMatchingEpicIdsAsync(epics.Select(x => x.EpicId), cancellationToken).ConfigureAwait(false);

            IEnumerable<string> uniqueStatusNameList = epics.Select(x => x.StatusName).Distinct().ToList();

            var statusNameCount = await _solutionEpicStatusRepository
                .GetMatchingEpicStatusAsync(uniqueStatusNameList, cancellationToken).ConfigureAwait(false);

            return epicIdsCount == epics.ToList().Count && statusNameCount == uniqueStatusNameList.Count();
        }

        public bool CheckNoDuplicateEpicIds(IEnumerable<string> epicIds)
        {
            return epicIds.GroupBy(x => x).Any(c => c.Count() > 1);
        }
    }
}
