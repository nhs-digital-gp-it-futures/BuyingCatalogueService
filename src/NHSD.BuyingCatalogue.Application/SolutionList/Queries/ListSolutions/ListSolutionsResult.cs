using System.Collections.Generic;
using NHSD.BuyingCatalogue.Contracts.SolutionList;

namespace NHSD.BuyingCatalogue.Application.SolutionList.Queries.ListSolutions
{
    /// <summary>
    /// Represents the result for the <see cref="ListSolutionsQuery"/>.
    /// </summary>
    public sealed class ListSolutionsResult
    {
        /// <summary>
        /// A list of solution summaries.
        /// </summary>
        public IEnumerable<ISolutionSummary> Solutions { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ListSolutionsResult"/> class.
        /// </summary>
        public ListSolutionsResult(IEnumerable<ISolutionSummary> solutions) => Solutions = solutions;
    }
}
