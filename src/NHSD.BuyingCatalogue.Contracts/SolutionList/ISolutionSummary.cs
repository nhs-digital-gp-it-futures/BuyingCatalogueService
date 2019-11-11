using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Contracts.SolutionList
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
        /// Details of the organisation associated with the solution.
        /// </summary>
        ISolutionOrganisation Organisation { get; }

        /// <summary>
        /// List of capabilities linked with the solution.
        /// </summary>
        IEnumerable<ISolutionCapability> Capabilities { get; }
    }
}
