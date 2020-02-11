using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    internal class ClaimedCapability
    {
        public string Name { get; }
        public string Version { get; }
        public string Description { get; }
        public string Link { get; }

        public ClaimedCapability(ISolutionCapabilityListResult solutionCapabilityListResult)
        {
            Name = solutionCapabilityListResult?.CapabilityName;
            Version = solutionCapabilityListResult?.CapabilityVersion;
            Description = solutionCapabilityListResult?.CapabilityDescription;
            Link = solutionCapabilityListResult?.CapabilitySourceUrl;
        }
    }
}
