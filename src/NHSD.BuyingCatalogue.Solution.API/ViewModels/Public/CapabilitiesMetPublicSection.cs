using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Public
{
    public class CapabilitiesMetPublicSection
    {
        [JsonProperty("capabilities-met")]
        public IEnumerable<string> CapabilitiesMet { get; }

        public CapabilitiesMetPublicSection(IEnumerable<string> capabilities)
            => CapabilitiesMet = new HashSet<string>(capabilities ?? new List<string>()); 
    }
}
