using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAllSolutionSummaries
{
    /// <summary>
    /// Provides the filter criteria for the <see cref="GetAllSolutionSummariesQuery"/> query.
    /// </summary>
    public sealed class GetAllSolutionSummariesFilter
    {
        /// <summary>
        /// A list of <see cref="Capability"/> IDs.
        /// </summary>
        public ISet<string> Capabilities { get; } = new HashSet<string>();
    }
}
