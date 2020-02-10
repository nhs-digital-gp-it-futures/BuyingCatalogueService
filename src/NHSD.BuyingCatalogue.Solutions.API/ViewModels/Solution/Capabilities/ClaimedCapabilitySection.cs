using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Capabilities
{
    public class ClaimedCapabilitySection
    {
        public string Name { get; }
        public string Version { get; }
        public string Description { get; }
        public string Link { get; }

        public ClaimedCapabilitySection(IClaimedCapability capability)
        {
            Name = capability?.Name.NullIfWhitespace();
            Version = capability?.Version.NullIfWhitespace();
            Description = capability?.Description.NullIfWhitespace();
            Link = capability?.Link.NullIfWhitespace();
        }

        public bool IsPopulated()
            => Name != null
               || Version != null
               || Description != null
               || Link != null;
    }
}
