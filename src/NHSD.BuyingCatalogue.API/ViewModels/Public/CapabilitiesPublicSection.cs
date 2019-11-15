using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.API.ViewModels.Public
{
    public class CapabilitiesPublicSection
    {
        public CapabilitiesPublicSection(IEnumerable<string> capabilities)
            => CapabilitiesMet = new CapabilitiesMetPublicSection(capabilities);

        [JsonProperty("answers")]
        public CapabilitiesMetPublicSection CapabilitiesMet { get; }
    }
}
