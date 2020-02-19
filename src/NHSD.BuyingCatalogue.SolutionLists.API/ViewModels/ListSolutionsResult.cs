using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.API.ViewModels
{
    /// <summary>
    /// Represents the result for the <see cref="ListSolutionsQuery"/>.
    /// </summary>
    public sealed class ListSolutionsResult : ISolutionList
    {
        /// <summary>
        /// A list of solution summaries.
        /// </summary>
        [JsonProperty("solutions")]
        public IEnumerable<SolutionSummaryResult> SolutionsSummaries { get; }

        [JsonIgnore]
        public IEnumerable<ISolutionSummary> Solutions { get => SolutionsSummaries; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ListSolutionsResult"/> class.
        /// </summary>
        public ListSolutionsResult(ISolutionList solutionList)
        {
            SolutionsSummaries = solutionList.ThrowIfNull(nameof(solutionList)).Solutions.Select(summary => new SolutionSummaryResult(summary)).ToList();
        }
    }
}
