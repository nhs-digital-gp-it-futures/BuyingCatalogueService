using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class SolutionAuthorityDashboardResult
    {
        public string Id { get; }

        public string Name { get; }

        [JsonProperty("sections")]
        public SolutionAuthorityDashboardSections SolutionAuthorityDashboardSections { get; }

        public SolutionAuthorityDashboardResult(ISolution solution)
        {
            if (solution != null)
            {
                Id = solution.Id;
                Name = solution.Name;
                SolutionAuthorityDashboardSections = new SolutionAuthorityDashboardSections(solution);
            }
        }
    }
}
