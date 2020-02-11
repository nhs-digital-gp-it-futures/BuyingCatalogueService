using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Capabilities
{
    public sealed class ClaimedCapabilitySection
    {
        public string Name { get; }
        public string Version { get; }
        public string Description { get; }
        public string Link { get; }

        public ClaimedCapabilitySection(IClaimedCapability capability)
        {
            capability = capability.ThrowIfNull();
            Name = capability.Name.NullIfWhitespace();
            Version = capability.Version.NullIfWhitespace();
            Description = capability.Description.NullIfWhitespace();
            Link = capability.Link.NullIfWhitespace();
        }

    }
}
