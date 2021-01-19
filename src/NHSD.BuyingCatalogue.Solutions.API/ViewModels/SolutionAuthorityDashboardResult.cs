using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class SolutionAuthorityDashboardResult
    {
        public SolutionAuthorityDashboardResult(ISolution solution)
        {
            if (solution is null)
                return;

            Id = solution.Id;
            Name = solution.Name;
            SolutionAuthorityDashboardSections = new SolutionAuthorityDashboardSections(solution);
        }

        public string Id { get; }

        public string Name { get; }

        [JsonProperty("sections")]
        public SolutionAuthorityDashboardSections SolutionAuthorityDashboardSections { get; }
    }
}
