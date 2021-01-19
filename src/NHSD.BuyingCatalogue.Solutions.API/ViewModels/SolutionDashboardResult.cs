using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class SolutionDashboardResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionDashboardResult"/> class.
        /// </summary>
        /// <param name="solution">The solution.</param>
        public SolutionDashboardResult(ISolution solution)
        {
            if (solution is null)
                return;

            Id = solution.Id;
            Name = solution.Name;
            SupplierName = solution.SupplierName;
            SolutionDashboardSections = new SolutionDashboardSections(solution);
        }

        public string Id { get; }

        public string Name { get; }

        [JsonProperty("supplier-name")]
        public string SupplierName { get; }

        [JsonProperty("sections")]
        public SolutionDashboardSections SolutionDashboardSections { get; }
    }
}
