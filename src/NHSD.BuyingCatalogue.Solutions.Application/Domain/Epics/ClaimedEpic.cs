using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Epics
{
    internal sealed class ClaimedEpic
    {
        public string EpicId { get; set; }

        public string StatusName { get; set; }

        internal ClaimedEpic(string epicId, string statusName)
        {
            EpicId = epicId.ThrowIfNull(nameof(epicId));
            StatusName = statusName.ThrowIfNull(nameof(statusName));
        }
    }
}
