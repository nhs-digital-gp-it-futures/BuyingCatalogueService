using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.SolutionLists.Contracts
{
    public interface IListSolutionsQueryData
    {
        IEnumerable<ICapabilityReference> CapabilityReferences { get; }

        bool IsFoundation { get; }
    }
}
