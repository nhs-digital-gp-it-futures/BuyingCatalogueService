using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Capabilities
{
    public class CapabilitiesSectionAnswers
    {
        [JsonProperty("capabilities-met")]
        public IEnumerable<ClaimedCapabilitySection> CapabilitiesMet { get; }

        public CapabilitiesSectionAnswers(IEnumerable<IClaimedCapability> capabilities)
            => CapabilitiesMet = capabilities != null
                ? capabilities.Select(c => new ClaimedCapabilitySection(c)).Where(c => c.IsPopulated()).ToArray()
                : Array.Empty<ClaimedCapabilitySection>();

        public bool HasData()
            => CapabilitiesMet.Any();
    }
}
