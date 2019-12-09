using System.Collections.Generic;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions
{
    internal sealed class SolutionListDto : ISolutionList
    {
        public IEnumerable<ISolutionSummary> Solutions { get; set; }
    }
}
