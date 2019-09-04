using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.ListSolutions
{
    /// <summary>
    /// Provides the filter criteria for the <see cref="ListSolutionsQuery"/> query.
    /// </summary>
    public sealed class ListSolutionsFilter
    {
        /// <summary>
        /// A list of <see cref="Capability"/> IDs.
        /// </summary>
        public ISet<string> Capabilities { get; } = new HashSet<string>();
    }
}
