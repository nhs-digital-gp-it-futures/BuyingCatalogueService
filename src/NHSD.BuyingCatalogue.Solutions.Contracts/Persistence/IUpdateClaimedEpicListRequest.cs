using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IUpdateClaimedEpicListRequest
    {
        IEnumerable<IClaimedEpicResult> ClaimedEpics { get; }
    }
}
