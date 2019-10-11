using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.ListSolutions
{
    /// <summary>
    /// Represents the result for the <see cref="ListSolutionsQuery"/>.
    /// </summary>
    public sealed class ListSolutionsResult
    {
        /// <summary>
        /// A list of solution summaries.
        /// </summary>
        public IEnumerable<SolutionSummaryViewModel> Solutions { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ListSolutionsResult"/> class.
        /// </summary>
        public ListSolutionsResult(IEnumerable<SolutionSummaryViewModel> solutions) => Solutions = solutions;
    }
}
