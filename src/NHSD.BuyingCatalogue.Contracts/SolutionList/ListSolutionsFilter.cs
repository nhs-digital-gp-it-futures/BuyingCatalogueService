using System;
using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Contracts.SolutionList
{
    /// <summary>
    /// Provides the filter criteria for the <see cref="ListSolutionsQuery"/> query.
    /// </summary>
    public sealed class ListSolutionsFilter
    {
        public static ListSolutionsFilter Foundation => new ListSolutionsFilter(true);

        private ListSolutionsFilter(bool isFoundation)
        {
            IsFoundation = isFoundation;
        }

        public ListSolutionsFilter()
        {

        }

        /// <summary>
        /// A list of <see cref="Capability"/> IDs.
        /// </summary>
        public ISet<Guid> Capabilities { get; } = new HashSet<Guid>();

        /// <summary>
        /// Filters to only foundation solutions
        /// </summary>
        public bool IsFoundation { get; } = false;
    }
}
