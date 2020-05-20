using System;
using System.Linq;
using Newtonsoft.Json;
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

        [JsonProperty("epic")]
        public ClaimedCapabilityEpicSection EpicSection { get; }

        public ClaimedCapabilitySection(IClaimedCapability capability)
        {
            if (capability is null)
            {
                throw new ArgumentNullException(nameof(capability));
            }

            Name = capability.Name.NullIfWhitespace();
            Version = capability.Version.NullIfWhitespace();
            Description = capability.Description.NullIfWhitespace();
            Link = capability.Link.NullIfWhitespace();

            if (capability?.ClaimedEpics.Any() == true)
            {
                EpicSection = new ClaimedCapabilityEpicSection(capability.ClaimedEpics);
            }
        }
    }
}
