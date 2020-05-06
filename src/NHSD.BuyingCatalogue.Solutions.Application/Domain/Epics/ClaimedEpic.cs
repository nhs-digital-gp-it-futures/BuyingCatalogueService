using System;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Epics
{
    internal sealed class ClaimedEpic
    {
        public string EpicId { get; set; }

        public string StatusName { get; set; }

        internal ClaimedEpic(string epicId, string statusName)
        {
            EpicId = epicId ?? throw new ArgumentNullException(nameof(epicId));
            StatusName = statusName ?? throw new ArgumentNullException(nameof(statusName));
        }
    }
}
