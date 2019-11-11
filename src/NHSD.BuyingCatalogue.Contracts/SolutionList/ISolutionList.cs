using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Contracts.SolutionList
{
    public interface ISolutionList
    {
        IEnumerable<ISolutionSummary> Solutions { get; }
    }
}
