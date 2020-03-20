using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.SolutionLists.Contracts
{
    public interface ISolutionSummary
    {
        /// <summary>
        /// Identifier of the solution.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Name of the solution.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Summary of the solution.
        /// </summary>
        string Summary { get; }

        /// <summary>
        /// Determines whether this is a foundation solution.
        /// </summary>
        bool IsFoundation { get; }

        /// <summary>
        /// Details of the supplier associated with the solution.
        /// </summary>
        ISolutionSupplier Supplier { get; }

        /// <summary>
        /// List of capabilities linked with the solution.
        /// </summary>
        IEnumerable<ISolutionCapability> Capabilities { get; }
    }
}
