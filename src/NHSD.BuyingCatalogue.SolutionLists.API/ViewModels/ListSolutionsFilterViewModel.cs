using System;
using System.Collections.Generic;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.API
{
    /// <summary>
    /// Provides the filter criteria for the <see cref="ListSolutionsQuery"/> query.
    /// </summary>
    public sealed class ListSolutionsFilterViewModel
    {
        /// <summary>
        /// A list of <see cref="Capability"/> IDs.
        /// </summary>
        public ISet<Guid> Capabilities { get; } = new HashSet<Guid>();

        /// <summary>
        /// Filters to only foundation solutions
        /// </summary>
        public bool IsFoundation { get; } = false;

        private ListSolutionsFilterViewModel(bool isFoundation)
        {
            IsFoundation = isFoundation;
        }

        public ListSolutionsFilterViewModel()
        {

        }

        public static ListSolutionsFilterViewModel Foundation => new ListSolutionsFilterViewModel(true);
    }
}
