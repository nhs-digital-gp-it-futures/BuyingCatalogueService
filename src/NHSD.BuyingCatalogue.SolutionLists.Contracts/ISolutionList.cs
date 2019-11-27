using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.SolutionLists.Contracts
{
    public interface ISolutionList
    {
        IEnumerable<ISolutionSummary> Solutions { get; }
    }
}
