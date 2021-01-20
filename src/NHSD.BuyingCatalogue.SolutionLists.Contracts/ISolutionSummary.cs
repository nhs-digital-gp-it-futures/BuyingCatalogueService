using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.SolutionLists.Contracts
{
    public interface ISolutionSummary
    {
        /// <summary>
        /// Gets the ID of the solution.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the name of the solution.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the summary of the solution.
        /// </summary>
        string Summary { get; }

        /// <summary>
        /// Gets a value indicating whether this is a foundation solution.
        /// </summary>
        bool IsFoundation { get; }

        /// <summary>
        /// Gets the details of the supplier associated with the solution.
        /// </summary>
        ISolutionSupplier Supplier { get; }

        /// <summary>
        /// Gets the list of capabilities linked with the solution.
        /// </summary>
        IEnumerable<ISolutionCapability> Capabilities { get; }
    }
}
