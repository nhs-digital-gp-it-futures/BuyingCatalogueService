using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Capabilities
{
    public sealed class CapabilitiesSectionAnswers
    {
        public CapabilitiesSectionAnswers(IEnumerable<IClaimedCapability> capabilities)
        {
            CapabilitiesMet = capabilities?.Select(c => new ClaimedCapabilitySection(c)).ToArray()
                ?? Array.Empty<ClaimedCapabilitySection>();
        }

        [JsonProperty("capabilities-met")]
        public IEnumerable<ClaimedCapabilitySection> CapabilitiesMet { get; }

        public bool HasData() => CapabilitiesMet.Any();
    }
}
