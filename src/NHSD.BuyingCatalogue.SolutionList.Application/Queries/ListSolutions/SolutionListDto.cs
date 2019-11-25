using System.Collections.Generic;
using NHSD.BuyingCatalogue.Contracts.SolutionList;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions
{
    internal sealed class SolutionListDto : ISolutionList
    {
        public IEnumerable<ISolutionSummary> Solutions { get; set; }
    }
}
