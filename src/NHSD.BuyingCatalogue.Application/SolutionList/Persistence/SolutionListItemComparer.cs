using System.Collections.Generic;
using NHSD.BuyingCatalogue.Application.SolutionList.Domain;

namespace NHSD.BuyingCatalogue.Application.SolutionList.Persistence
{
    internal sealed class SolutionListItemComparer : IEqualityComparer<SolutionListItem>
    {
        public bool Equals(SolutionListItem x, SolutionListItem y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(SolutionListItem obj)
        {
            return EqualityComparer<string>.Default.GetHashCode(obj.Id);
        }
    }
}
