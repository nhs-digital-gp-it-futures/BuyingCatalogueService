using System;
using System.Collections.Generic;
using System.Text;
using NHSD.BuyingCatalogue.Contracts.SolutionList;

namespace NHSD.BuyingCatalogue.Application.SolutionList.Queries.ListSolutions
{
    internal sealed class SolutionListDto : ISolutionList
    {
        public IEnumerable<ISolutionSummary> Solutions { get; set; }
    }
}
