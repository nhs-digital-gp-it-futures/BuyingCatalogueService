using System;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence.Epics
{
    internal sealed class ClaimedEpicResult : IClaimedEpicResult
    {
        public string EpicId { get; }
        public string StatusName { get; }

        public ClaimedEpicResult(string epicId, string statusName)
        {
            EpicId = epicId ?? throw new ArgumentNullException(nameof(epicId));
            StatusName = statusName ?? throw new ArgumentNullException(nameof(statusName));
        }
    }
}
