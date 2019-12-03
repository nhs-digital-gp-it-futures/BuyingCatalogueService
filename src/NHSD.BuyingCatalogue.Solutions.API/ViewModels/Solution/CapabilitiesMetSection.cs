using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class CapabilitiesMetSection
    {
        [JsonProperty("capabilities-met")]
        public IEnumerable<string> CapabilitiesMet { get; }

        public CapabilitiesMetSection(IEnumerable<string> capabilities)
            => CapabilitiesMet = new HashSet<string>(capabilities ?? new List<string>()); 
    }
}
