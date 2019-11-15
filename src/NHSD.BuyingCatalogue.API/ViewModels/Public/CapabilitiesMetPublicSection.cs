using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.API.ViewModels.Public
{
    public class CapabilitiesMetPublicSection
    {
        [JsonProperty("capabilities-met")]
        public IEnumerable<string> Listing { get; }

        // Canned Data --TODO
        public CapabilitiesMetPublicSection()
        {
            Listing = new List<string>()
            {
                "capability1",
                "capability2",
                "capability3"
            };

        }
    }
}
