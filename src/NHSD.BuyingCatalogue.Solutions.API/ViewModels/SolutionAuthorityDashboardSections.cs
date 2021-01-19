using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class SolutionAuthorityDashboardSections
    {
        public SolutionAuthorityDashboardSections(ISolution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            IEnumerable<IClaimedCapability> claimedCapabilityList = solution.Capabilities.ToList();

            Capabilities = DashboardSection.Mandatory(claimedCapabilityList.Any());
            Epics = DashboardSection.Mandatory(claimedCapabilityList.Any(capability => capability.ClaimedEpics.Any()));
        }

        [JsonProperty("capabilities")]
        public DashboardSection Capabilities { get; }

        [JsonProperty("epics")]
        public DashboardSection Epics { get; }
    }
}
