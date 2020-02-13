using System.Collections.Generic;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence.Epics
{
    internal sealed class UpdateClaimedEpicRequest : IUpdateClaimedEpicListRequest
    {
        public IEnumerable<IClaimedEpicResult> ClaimedEpics { get; internal set; }

        public UpdateClaimedEpicRequest(IEnumerable<IClaimedEpicResult> claimedEpics)
        {
            ClaimedEpics = claimedEpics.ThrowIfNull(nameof(claimedEpics));
        }
    }
}
