using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAllSolutionSummaries
{
    /// <summary>
    /// Represents the result for the <see cref="GetAllSolutionSummariesQuery"/>.
    /// </summary>
    public sealed class GetAllSolutionSummariesQueryResult
    {
        /// <summary>
        /// A list of solution summaries.
        /// </summary>
        public IEnumerable<SolutionSummaryViewModel> Solutions { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetAllSolutionSummariesQueryResult"/> class.
        /// </summary>
        public GetAllSolutionSummariesQueryResult()
        {
        }
    }
}
