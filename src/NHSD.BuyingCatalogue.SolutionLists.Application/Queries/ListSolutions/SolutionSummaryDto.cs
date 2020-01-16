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
        /// Identifier of the solution.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the solution.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Summary of the solution.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Determines whether this is a foundation solution.
        /// </summary>
        public bool IsFoundation { get; set; }

        /// <summary>
        /// Details of the supplier associated with the solution.
        /// </summary>
        public ISolutionSupplier Supplier { get; set; }

        /// <summary>
        /// List of capabilities linked with the solution.
        /// </summary>
        public IEnumerable<ISolutionCapability> Capabilities { get; set; }
    }
}
