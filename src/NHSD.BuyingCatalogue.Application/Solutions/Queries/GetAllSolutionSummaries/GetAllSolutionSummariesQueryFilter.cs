using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAllSolutionSummaries
{
    public sealed class GetAllSolutionSummariesQueryFilter
    {
        public ISet<string> Capabilities { get; } = new HashSet<string>();
    }
}
