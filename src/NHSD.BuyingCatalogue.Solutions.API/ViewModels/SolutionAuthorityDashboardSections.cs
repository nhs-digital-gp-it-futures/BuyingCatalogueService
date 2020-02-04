using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class SolutionAuthorityDashboardSections
    {
        [JsonProperty("capabilities")]
        public DashboardSection Capabilities { get; }

        public SolutionAuthorityDashboardSections(ISolution solution)
        {
            solution = solution.ThrowIfNull(nameof(solution));

            Capabilities = DashboardSection.Mandatory(solution.Capabilities.Any(x => !string.IsNullOrWhiteSpace(x)));
        }
    }
}
