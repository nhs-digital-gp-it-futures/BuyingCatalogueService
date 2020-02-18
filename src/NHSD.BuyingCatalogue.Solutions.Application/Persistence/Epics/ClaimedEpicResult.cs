using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence.Epics
{
    internal sealed class ClaimedEpicResult : IClaimedEpicResult
    {
        public string EpicId { get; }
        public string StatusName { get; }

        public ClaimedEpicResult(string epicId, string statusName)
        {
            EpicId = epicId.ThrowIfNull(nameof(epicId));
            StatusName = statusName.ThrowIfNull(nameof(statusName));
        }
    }
}
