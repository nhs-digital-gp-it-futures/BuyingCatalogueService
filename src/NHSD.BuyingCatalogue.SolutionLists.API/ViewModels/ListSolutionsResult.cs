using System;
using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.API.ViewModels
{
    /// <summary>
    /// Represents the result for the <see cref="ListSolutionsQuery"/>.
    /// </summary>
    public sealed class ListSolutionsResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListSolutionsResult"/> class.
        /// </summary>
        /// <param name="solutionList">The solution list.</param>
        public ListSolutionsResult(ISolutionList solutionList)
        {
            if (solutionList is null)
            {
                throw new ArgumentNullException(nameof(solutionList));
            }

            Solutions = solutionList.Solutions.Select(summary => new SolutionSummaryResult(summary)).ToList();
        }

        /// <summary>
        /// Gets a list of solution summaries.
        /// </summary>
        public IEnumerable<SolutionSummaryResult> Solutions { get; }
    }
}
