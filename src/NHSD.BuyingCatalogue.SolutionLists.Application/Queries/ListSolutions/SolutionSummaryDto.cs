using System.Collections.Generic;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions
{
    /// <summary>
    /// Represents the data to summarize a solution entity and associated relationships.
    /// </summary>
    internal sealed class SolutionSummaryDto : ISolutionSummary
    {
        /// <summary>
        /// Gets or sets the ID of the solution.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the solution.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the summary of the solution.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a foundation solution.
        /// </summary>
        public bool IsFoundation { get; set; }

        /// <summary>
        /// Gets or sets the details of the supplier associated with the solution.
        /// </summary>
        public ISolutionSupplier Supplier { get; set; }

        /// <summary>
        /// Gets or sets the list of capabilities linked with the solution.
        /// </summary>
        public IEnumerable<ISolutionCapability> Capabilities { get; set; }
    }
}
