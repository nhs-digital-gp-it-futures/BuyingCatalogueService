using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAllSolutionSummaries
{
    public sealed class GetAllSolutionSummariesQueryFilter
    {
        public ISet<string> Capabilities { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetAllSolutionSummariesQueryFilter"/> class.
        /// </summary>
        public GetAllSolutionSummariesQueryFilter()
        {
            Capabilities = new HashSet<string>();
        }
    }
}
