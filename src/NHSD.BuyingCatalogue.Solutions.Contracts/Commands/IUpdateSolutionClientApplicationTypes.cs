using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands
{
    public interface IUpdateSolutionClientApplicationTypes
    {
        HashSet<string> ClientApplicationTypes { get; }

        IEnumerable<string> FilteredClientApplicationTypes { get; }
    }
}
