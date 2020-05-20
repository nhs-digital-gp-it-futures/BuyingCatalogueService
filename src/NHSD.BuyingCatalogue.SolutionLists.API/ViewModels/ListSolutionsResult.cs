using System;
using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.API.ViewModels
{
    /// <summary>
    /// Represents the result for the <see cref="ListSolutionsQuery"/>.
    /// </summary>
    public sealed class ListSolutionsResult
    {
        /// <summary>
        /// A list of solution summaries.
        /// </summary>
        public IEnumerable<SolutionSummaryResult> Solutions { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ListSolutionsResult"/> class.
        /// </summary>
        public ListSolutionsResult(ISolutionList solutionList)
        {
            if (solutionList is null)
            {
                throw new ArgumentNullException(nameof(solutionList));
            }

            Solutions = solutionList.Solutions.Select(summary => new SolutionSummaryResult(summary)).ToList();
        }
    }
}
