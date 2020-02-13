using System.Collections.Generic;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence.Epics
{
    internal sealed class UpdateClaimedEpicListRequest : IUpdateClaimedEpicListRequest
    {
        public IEnumerable<IClaimedEpicResult> ClaimedEpics { get; internal set; }

        public UpdateClaimedEpicListRequest(IEnumerable<IClaimedEpicResult> claimedEpics)
        {
            ClaimedEpics = claimedEpics.ThrowIfNull(nameof(claimedEpics));
        }
    }
}
