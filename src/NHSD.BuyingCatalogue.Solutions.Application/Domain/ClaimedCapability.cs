using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    internal sealed class ClaimedCapability
    {
        public string Name { get; }
        public string Version { get; }
        public string Description { get; }
        public string Link { get; }

        public IEnumerable<ClaimedCapabilityEpic> ClaimedEpics { get; }

        public ClaimedCapability(ISolutionCapabilityListResult solutionCapabilityListResult,
            IEnumerable<ISolutionEpicListResult> capabilityEpics)
        {
            Name = solutionCapabilityListResult?.CapabilityName;
            Version = solutionCapabilityListResult?.CapabilityVersion;
            Description = solutionCapabilityListResult?.CapabilityDescription;
            Link = solutionCapabilityListResult?.CapabilitySourceUrl;
            ClaimedEpics = capabilityEpics?.Select(ce => new ClaimedCapabilityEpic(ce)).ToList();
        }
    }
}
