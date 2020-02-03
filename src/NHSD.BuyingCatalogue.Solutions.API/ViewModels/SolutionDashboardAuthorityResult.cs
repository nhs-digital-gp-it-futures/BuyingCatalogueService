using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class SolutionDashboardAuthorityResult
    {
        public string Id { get; }

        public string Name { get; }

        [JsonProperty("sections")]
        public SolutionDashboardAuthoritySections SolutionDashboardAuthoritySections { get; }

        public SolutionDashboardAuthorityResult(ISolution solution)
        {
            if (solution != null)
            {
                Id = solution.Id;
                Name = solution.Name;
                SolutionDashboardAuthoritySections = new SolutionDashboardAuthoritySections(solution);
            }
        }
    }

    public sealed class SolutionDashboardAuthoritySections
    {
        [JsonProperty("capabilities")]
        public DashboardSection Capabilities { get; }

        public SolutionDashboardAuthoritySections(ISolution solution)
        {
            solution = solution.ThrowIfNull();

            Capabilities = DashboardSection.Mandatory(solution.Capabilities.Any(x => !string.IsNullOrWhiteSpace(x)));
        }
    }
}
