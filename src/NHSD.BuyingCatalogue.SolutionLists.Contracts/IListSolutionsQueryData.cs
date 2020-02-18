using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.SolutionLists.Contracts
{
    public interface IListSolutionsQueryData
    {
        ISet<ICapabilityReference> CapabilityReferences { get; }

        bool IsFoundation { get; }
    }
}
