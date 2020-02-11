using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IUpdateClaimedRequest
    {
        IEnumerable<IClaimedEpicResult> ClaimedEpics { get; }
    }
}
